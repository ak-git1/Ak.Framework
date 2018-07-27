using System;
using System.IO;
using Ak.Framework.Core.IO.Extensions;

namespace Ak.Framework.Core.IO
{
    /// <summary>
    /// Класс для работы с потоками
    /// </summary>
    public static class StreamHelper
    {
        /// <summary>
        /// Преобразование потока в битовый массив
        /// </summary>
        /// <param name="s">Поток</param>
        /// <returns></returns>
        public static byte[] ReadByteArray(Stream s)
        {
            byte[] rawLength = new byte[sizeof(int)];
            if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
            {
                throw new SystemException("Stream did not contain properly formatted byte array");
            }

            byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
            if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                throw new SystemException("Did not read byte array properly");
            }

            return buffer;
        }

        /// <summary>
        /// Копирование потока
        /// </summary>
        /// <param name="input">Входной поток</param>
        /// <param name="output">Выходной поток</param>
        public static void CopyStream(Stream input, Stream output)
        {
            byte[] numArray = new byte[16384];
            while (true)
            {
                int num = input.Read(numArray, 0, numArray.Length);
                int num1 = num;
                if (num <= 0)
                    break;
                
                output.Write(numArray, 0, num1);
            }
        }

        /// <summary>
        /// Безопасная запись в файл
        /// </summary>
        /// <param name="inputStream">Входной поток</param>
        /// <param name="targetPath">Путь</param>
        public static void WriteToFileSafely(Stream inputStream, string targetPath)
        {
            FileInfo fileInfo = new FileInfo(string.Concat(targetPath, ".tmp"));
            try
            {
                FileStream fileStream = fileInfo.OpenWriteLocked();
                try
                {
                    StreamHelper.CopyStream(inputStream, fileStream);
                    fileStream.Flush(true);
                    fileStream.Close();
                }
                finally
                {
                    ((IDisposable)fileStream)?.Dispose();
                }

                if (!File.Exists(targetPath))
                    File.Move(fileInfo.FullName, targetPath);
                else
                    File.Replace(fileInfo.FullName, targetPath, null);
            }
            finally
            {
                if (fileInfo.Exists)
                    fileInfo.Delete();
            }
        }
    }
}