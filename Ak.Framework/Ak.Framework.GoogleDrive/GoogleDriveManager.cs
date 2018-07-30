using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using File = Google.Apis.Drive.v3.Data.File;

namespace Ak.Framework.GoogleDrive
{
    /// <summary>
    /// Класс для работы с Google Drive
    /// </summary>
    public class GoogleDriveManager
    {
        #region Переменные и константы

        /// <summary>
        /// Разрешения
        /// </summary>
        private static readonly string[] Scopes = { DriveService.Scope.Drive, DriveService.Scope.DriveFile, DriveService.Scope.DriveMetadata };

        /// <summary>
        /// Тип MIME директори в Google Drive
        /// </summary>
        private const string DirectoryMimeType = "application/vnd.google-apps.folder";

        #endregion

        #region Свойства

        /// <summary>
        /// Клиентский сертификат от Google
        /// </summary>
        private byte[] GoogleClientSecret { get; set; }

        /// <summary>
        /// Путь для хранения учетных данных
        /// </summary>
        private string CredentialPath { get; set; }

        #endregion

        #region Конструктор

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="googleClientSecret">Клиентский сертификат от Google</param>
        /// <param name="appName">Название приложения</param>
        /// <param name="credentialPath">Путь для хранения учетных данных</param>
        public GoogleDriveManager(byte[] googleClientSecret, string appName = "myapp", string credentialPath = "")
        {
            GoogleClientSecret = googleClientSecret;
            CredentialPath = string.IsNullOrEmpty(credentialPath)
                ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), $".credentials/{appName}-google-drive.json")
                : credentialPath;
        }

        #endregion

        #region Публичные методы

        /// <summary>
        /// Аутентификация
        /// </summary>
        /// <returns></returns>
        public UserCredential Authentificate()
        {
            UserCredential credential = null;

            try
            {
                using (Stream stream = new MemoryStream(GoogleClientSecret))
                {
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                                                                                GoogleClientSecrets.Load(stream).Secrets,
                                                                                Scopes,
                                                                                Environment.UserName,
                                                                                CancellationToken.None,
                                                                                new FileDataStore(CredentialPath, true)).Result;
                }
            }
            catch (Exception ex)
            {
            }

            return credential;
        }

        /// <summary>
        /// Сброс аутентификации
        /// </summary>
        public void DropAuthentification()
        {
            if (Directory.Exists(CredentialPath))
                Directory.Delete(CredentialPath, true);
        }

        /// <summary>
        /// Получение сервиса для работы Google Drive API
        /// </summary>
        /// <param name="credential">Учетные данные</param>
        /// <returns></returns>
        public DriveService GetService(UserCredential credential)
        {
            return new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "ScanImage"
            });
        }

        /// <summary>
        /// Создание директории
        /// </summary>
        /// <param name="service">Сервис</param>
        /// <param name="directoryName">Название директории</param>
        /// <param name="parentDirectoryId">Идентификатор родительской директории</param>
        /// <returns></returns>
        public File CreateDirectory(DriveService service, string directoryName, string parentDirectoryId = null)
        {
            File body = new File
            {
                Name = directoryName,
                Description = "Created by ScanImage",
                MimeType = DirectoryMimeType
            };
            if (!string.IsNullOrEmpty(parentDirectoryId))
                body.Parents = new List<string> { parentDirectoryId };

            FilesResource.CreateRequest request = service.Files.Create(body);
            File directory = request.Execute();

            return directory;
        }

        /// <summary>
        /// Проверка существования директории
        /// </summary>
        /// <param name="service">Сервис</param>
        /// <param name="directoryName">Название директории</param>
        /// <param name="parentDirectoryId">Идентификатор родительской директории</param>
        /// <returns></returns>
        public string DirectoryExists(DriveService service, string directoryName, string parentDirectoryId = null)
        {
            if (string.IsNullOrEmpty(directoryName))
                throw new Exception("Directory name must be set.");

            string result = null;

            FilesResource.ListRequest list = service.Files.List();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("mimeType = '{0}' and name = '{1}'", DirectoryMimeType, directoryName);
            if (!string.IsNullOrEmpty(parentDirectoryId))
                sb.AppendFormat(" and '{0}' in parents", parentDirectoryId);
            list.Q = sb.ToString();

            FileList filesList = list.Execute();
            if (filesList.Files != null)
            {
                File file = filesList.Files.FirstOrDefault();
                if (file != null)
                    result = file.Id;
            }

            return result;
        }

        /// <summary>
        /// Создание пути из директорий
        /// </summary>
        /// <param name="service">Сервис</param>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public string CreateDirectoriesPath(DriveService service, string path)
        {
            string parentDirectoryId = "root";
            string[] directories = path.Split('\\');


            foreach (string dir in directories)
                if (!string.IsNullOrEmpty(dir))
                {
                    string directoryId = DirectoryExists(service, dir, parentDirectoryId);
                    if (string.IsNullOrEmpty(directoryId))
                    {
                        File directory = CreateDirectory(service, dir, parentDirectoryId);
                        if (directory == null)
                            throw new Exception($"Unable to create directory. Name = {dir}, Path={parentDirectoryId}");
                        directoryId = directory.Id;
                    }

                    parentDirectoryId = directoryId;
                }

            return parentDirectoryId;
        }

        /// <summary>
        /// Загрузка файла на Google
        /// </summary>
        /// <param name="service">Сервис</param>
        /// <param name="localfilePath">Локальный путь к файлу</param>
        /// <param name="gooogleDrivePath">Путь в Google Drive</param>
        /// <returns></returns>
        public File UploadFile(DriveService service, string localfilePath, string gooogleDrivePath)
        {
            string parentGoogleDriveId = CreateDirectoriesPath(service, gooogleDrivePath);
            return UploadFileByParentId(service, localfilePath, parentGoogleDriveId);
        }

        /// <summary>
        /// Загрузка файла на Google
        /// </summary>
        /// <param name="service">Сервис</param>
        /// <param name="localfilePath">Локальный путь к файлу</param>
        /// <param name="parentGoogleDriveId">Идентификатор папки в Google Drive</param>
        /// <returns></returns>
        public File UploadFileByParentId(DriveService service, string localfilePath, string parentGoogleDriveId)
        {
            if (!System.IO.File.Exists(localfilePath))
                throw new Exception($"File '{localfilePath}' does not exist and can not be uploaded to Google Drive");

            string mimeType = GetMimeType(localfilePath);

            File body = new File
            {
                Name = Path.GetFileName(localfilePath),
                Description = "Created by ScanImage",
                MimeType = mimeType
            };
            if (!string.IsNullOrEmpty(parentGoogleDriveId))
                body.Parents = new List<string> { parentGoogleDriveId };

            byte[] byteArray = System.IO.File.ReadAllBytes(localfilePath);
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                FilesResource.CreateMediaUpload request = service.Files.Create(body, stream, mimeType);
                request.Upload();
                return request.ResponseBody;
            }
        }

        #endregion

        #region Приватные методы

        /// <summary>
        /// Получение типа файла
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        /// <returns></returns>
        private static string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey?.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        #endregion
    }
}
