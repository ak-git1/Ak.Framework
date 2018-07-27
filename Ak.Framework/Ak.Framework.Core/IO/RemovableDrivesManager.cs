using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;

namespace Ak.Framework.Core.IO
{
    /// <summary>
    /// Класс для работы со съемными дисками
    /// </summary>
    public class RemovableDrivesManager : IDisposable
    {
        #region Переменные и константы

        /// <summary>
        /// WMI запрос на событие монтирования устройства
        /// </summary>
        private const string RemovableDriveAddedQuery = "SELECT * FROM __InstanceCreationEvent WITHIN 1 WHERE TargetInstance ISA 'Win32_DiskDriveToDiskPartition'";

        /// <summary>
        /// WMI запрос на событие размонтирования устройства
        /// </summary>
        private const string RemovableDriveRemovedQuery = "SELECT * FROM __InstanceDeletionEvent WITHIN 1 WHERE TargetInstance ISA 'Win32_DiskDriveToDiskPartition'";

        /// <summary>
        /// Чтение файла
        /// </summary>
        private const int GenericRead = unchecked((int)0x80000000);

        /// <summary>
        /// Запись в файл
        /// </summary>
        private const int GenericWrite = 0x40000000;

        /// <summary>
        /// Совместный доступ к чтению файла
        /// </summary>
        private const int FileShareRead = 0x00000001;

        /// <summary>
        /// Совместная запись в файл
        /// </summary>
        private const int FileShareWrite = 0x00000002;

        /// <summary>
        /// Открыть существующий файл
        /// </summary>
        private const int OpenExisting = 3;

        /// <summary>
        /// Запрос на блокировку тома
        /// </summary>
        const int FsctlLockVolume = 0x00090018;

        /// <summary>
        /// Запрос на размонтирование тома
        /// </summary>
        const int FsctlDismountVolume = 0x00090020;

        /// <summary>
        /// Запрос на запрет извлечение устройства
        /// </summary>
        private const int IoctlStorageMediaRemoval = 0x002D4804;

        /// <summary>
        /// Запрос на извлечение съемного носителя
        /// </summary>
        const int IoctlStorageEjectMedia = 0x002D4808;

        /// <summary>
        /// Инвалидный хэндл
        /// </summary>
        private const long InvalidHandleValue = -1;

        /// <summary>
        /// Время ожидания после неудачной попытки выполнения операции (в миллисекундах)
        /// </summary>
        private const int OperationWaitTimeout = 3000;

        /// <summary>
        /// Количество попыток выполнения операции
        /// </summary>
        private const int OperationRetries = 3;

        /// <summary>
        /// Объект, оповещающий о появлении новых съемных носителей
        /// </summary>
        private readonly ManagementEventWatcher _mewDiskDriveToPartitionCreation;

        /// <summary>
        /// Объект, оповещающий об исчезновении съемных носителей
        /// </summary>
        private readonly ManagementEventWatcher _mewDiskDriveToPartitionDeletion;

        /// <summary>
        /// Признак того, что ресурсы освобождены
        /// </summary>
        private bool _isDisposed;


        #endregion

        #region Свойства

        /// <summary>
        /// Список пригодных для работы съемных носителей
        /// </summary>
        public static IEnumerable<DriveInfo> RemovableDrives => DriveInfo.GetDrives().Where(drive => drive.DriveType == DriveType.Removable && drive.IsReady);

        #endregion        

        #region События

        /// <summary>
        /// Срабатывает при появлении нового съемного устройства
        /// </summary>
        public event EventHandler RemovableDevicesAdded;

        /// <summary>
        /// Срабатывает при исчезновении съемного устройства
        /// </summary>
        public event EventHandler RemovableDevicesRemoved;

        #endregion

        #region Конструктор

        /// <summary>
        /// Конструктор
        /// </summary>
        public RemovableDrivesManager()
        {
            _mewDiskDriveToPartitionCreation = new ManagementEventWatcher(new WqlEventQuery(RemovableDriveAddedQuery));
            _mewDiskDriveToPartitionCreation.EventArrived += (sender, args) => RemovableDevicesAdded?.Invoke(this, EventArgs.Empty);
            _mewDiskDriveToPartitionCreation.Start();

            _mewDiskDriveToPartitionDeletion = new ManagementEventWatcher(new WqlEventQuery(RemovableDriveRemovedQuery));
            _mewDiskDriveToPartitionDeletion.EventArrived += (sender, args) => RemovableDevicesRemoved?.Invoke(this, EventArgs.Empty);
            _mewDiskDriveToPartitionDeletion.Start();
        }

        #endregion

        #region Импорт системных функций

        /// <summary>
        /// Создание файла (CreateFileW из kernel32.dll)
        /// </summary>
        [DllImport("kernel32.dll", EntryPoint = "CreateFileW", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern IntPtr CreateFile(string lpFileName, int dwDesiredAccess, int dwShareMode,
            IntPtr lpSecurityAttributes, int dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile);

        /// <summary>
        /// Управление устройством (DeviceIoControl из kernel32.dll)
        /// </summary>
        [DllImport("kernel32.dll")]
        private static extern bool DeviceIoControl(IntPtr deviceHandle, uint ioControlCode, IntPtr inBuffer,
            int inBufferSize, IntPtr outBuffer, int outBufferSize, out int bytesReturned, IntPtr overlapped);

        /// <summary>
        /// Управление устройством (DeviceIoControl из kernel32.dll).
        /// Перегрузка для передачи данных в буффере.
        /// </summary>
        [DllImport("kernel32.dll", EntryPoint = "DeviceIoControl", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool DeviceIoControl(IntPtr hDevice, uint dwIoControlCode, byte[] lpInBuffer,
            uint nInBufferSize, IntPtr lpOutBuffer, uint nOutBufferSize, out int lpBytesReturned, IntPtr lpOverlapped);

        /// <summary>
        /// Закрытие хэндла (CloseHandle из kernel32.dll)
        /// </summary>
        [DllImport("kernel32.dll")]
        private static extern int CloseHandle(IntPtr handle);

        #endregion

        #region Публичные методы

        /// <summary>
        /// Извлекает (размонтирует) съемный диск.
        /// Используется последовательность действий, приведенная в https://support.microsoft.com/en-us/help/165721/how-to-ejecting-removable-media-in-windows-nt-windows-2000-windows-xp
        /// </summary>
        /// <param name="drive">Информация о диске</param>
        /// <returns>true - операция прошла успешно, иначе false</returns>
        public static bool SafelyRemove(DriveInfo drive)
        {
            if (!drive.IsReady)
                return false;

            IntPtr hVolume = OpenVolume(drive);
            if ((long)hVolume == InvalidHandleValue)
                return false;

            try
            {
                return LockVolume(hVolume) && DismountVolume(hVolume) && PreventRemovalOfVolume(hVolume, false) && EjectMedia(hVolume);
            }
            finally
            {
                CloseHandle(hVolume);
            }
        }

        /// <summary>
        /// Освобождает используемые ресурсы
        /// </summary>
        public void Dispose()
        {
            _mewDiskDriveToPartitionCreation?.Stop();
            _mewDiskDriveToPartitionCreation?.Dispose();
            _mewDiskDriveToPartitionDeletion?.Stop();
            _mewDiskDriveToPartitionDeletion?.Dispose();
            _isDisposed = true;
        }

        /// <summary>
        /// Финалайзер
        /// </summary>
        ~RemovableDrivesManager()
        {
            if (_isDisposed)
                return;

            try
            {
                Dispose();
            }
            catch (Exception exception)
            {
            }
        }

        #endregion

        #region Приватные методы

        /// <summary>
        /// Открытие тома
        /// </summary>
        /// <param name="drive">Диск</param>
        /// <returns>Хэндл тома, если операция прошла успешно, иначе InvalidHandleValue</returns>
        private static IntPtr OpenVolume(DriveInfo drive)
        {
            Match match = new Regex(@"([^:]+\:)").Match(drive.RootDirectory.Name);
            if (!match.Success)
                return (IntPtr)InvalidHandleValue;

            string driveLetterWithColon = match.Groups[1].Value;

            return CreateFile($@"\\.\{driveLetterWithColon}", GenericRead | GenericWrite, FileShareRead | FileShareWrite, IntPtr.Zero, OpenExisting, 0, IntPtr.Zero);
        }

        /// <summary>
        /// Блокировка тома
        /// </summary>
        /// <param name="hVolume">Хэндл тома</param>
        /// <returns>true - операция прошла успешно, иначе false</returns>
        private static bool LockVolume(IntPtr hVolume)
        {
            for (int i = 0; i < OperationRetries; i++)
            {
                if (DeviceIoControl(hVolume, FsctlLockVolume, IntPtr.Zero, 0, IntPtr.Zero, 0, out _, IntPtr.Zero))
                    return true;

                Thread.Sleep(OperationWaitTimeout);
            }
            return false;
        }

        /// <summary>
        /// Запрещает/разрешает извлечение устройства
        /// </summary>
        /// <param name="hVolume">Хэндл тома</param>
        /// <param name="prevent">true - запретить, false - разрешить</param>
        /// <returns></returns>
        private static bool PreventRemovalOfVolume(IntPtr hVolume, bool prevent)
        {
            byte[] inBuffer = { prevent ? (byte)1 : (byte)0 };
            return DeviceIoControl(hVolume, IoctlStorageMediaRemoval, inBuffer, 1, IntPtr.Zero, 0, out _, IntPtr.Zero);
        }

        /// <summary>
        /// Размонтирование тома
        /// </summary>
        /// <param name="hVolume">Хэндл тома</param>
        /// <returns>true - операция прошла успешно, иначе false</returns>
        private static bool DismountVolume(IntPtr hVolume)
        {
            return DeviceIoControl(hVolume, FsctlDismountVolume, IntPtr.Zero, 0, IntPtr.Zero, 0, out _, IntPtr.Zero);
        }

        /// <summary>
        /// Извлечение медии
        /// </summary>
        /// <param name="hVolume">Хэндл тома</param>
        /// <returns>true - операция прошла успешно, иначе false</returns>
        private static bool EjectMedia(IntPtr hVolume)
        {
            return DeviceIoControl(hVolume, IoctlStorageEjectMedia, IntPtr.Zero, 0, IntPtr.Zero, 0, out _, IntPtr.Zero);
        }

        #endregion
    }
}