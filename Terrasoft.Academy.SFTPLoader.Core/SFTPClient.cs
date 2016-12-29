using System;
using System.Collections.Generic;
using Renci.SshNet;
using System.IO;

namespace Terrasoft.Academy.SFTPLoader.Core
{
    public class SFTPClient : IDisposable
    {
        SftpClient sftpClient = null;
        bool disposed = false;

        /// <summary>
        /// Constructor with parameters.
        /// </summary>
        /// <param name="keyPath">Path to *.ppk private key file</param>
        /// <param name="ftpPath">ftp server url</param>
        /// <param name="ftpPort">ftp server port</param>
        /// <param name="userName">ftp user name</param>
        public SFTPClient(string keyPath, string ftpPath, int ftpPort, string userName)
        {
            string currDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var keyFile = new PrivateKeyFile(keyPath);
            var keyFiles = new[] { keyFile };
            var methods = new List<AuthenticationMethod>();
            methods.Add(new PrivateKeyAuthenticationMethod(userName, keyFiles));
            var connectionInfo = new ConnectionInfo(ftpPath, ftpPort, userName, methods.ToArray());
            sftpClient = new SftpClient(connectionInfo);
        }

        /// <summary>
        /// Lists specified ftp directory.
        /// </summary>
        /// <param name="remotePath"> Relative path to remote directory. For example "some/path".</param>
        /// <returns>Collection of <see cref="Renci.SshNet.Sftp.SftpFile"/> objects. </returns>
        public IEnumerable<Renci.SshNet.Sftp.SftpFile> ListDirectory(string remotePath)
        {
            return sftpClient.ListDirectory(remotePath);
        }

        /// <summary>
        /// Connects to ftp server.
        /// </summary>
        public void Connect()
        {
            sftpClient.Connect();
        }

        /// <summary>
        /// Disconnects from ftp server.
        /// </summary>
        public void Disconnect()
        {
            sftpClient.Disconnect();
        }

        /// <summary>
        /// Recursively downloads remote directory.
        /// </summary>
        /// <param name="localPath">Path to local directory.</param>
        /// <param name="remotePath">Path to remote directory.</param>
        public void DownloadDiectory(string localPath, string remotePath)
        {
            var list = ListDirectory(remotePath);
            foreach (var file in list)
            {
                if (file.IsRegularFile)
                {
                    DownloadFile(localPath, file);
                }
                else if (file.IsSymbolicLink)
                {
                    Console.WriteLine("Symbolic files are ignored");
                }
                else if (file.Name != "." && file.Name != "..")
                {
                    var c = Path.Combine(localPath, file.Name);
                    
                    var dir = Directory.CreateDirectory(Path.Combine(localPath, file.Name));
                    DownloadDiectory(dir.FullName, file.FullName);
                }
            }
        }

        /// <summary>
        /// Downloads single file from remote ftp directory.
        /// </summary>
        /// <param name="localPath">Path to local directory.</param>
        /// <param name="file"><see cref="Renci.SshNet.Sftp.SftpFile"/> remote file.</param>
        public void DownloadFile(string localPath, Renci.SshNet.Sftp.SftpFile file)
        {
           
            using (var fs = new FileStream(Path.Combine(localPath, file.Name), FileMode.Create))
            {
                sftpClient.DownloadFile(file.FullName, fs);
                fs.Close();
            }
        }

        /// <summary>
        /// Uploads file to remote ftp directory.
        /// </summary>
        /// <param name="fileName"> Local file name.</param>
        /// <param name="remoteFileName">Remote file name. Does not include ftp server url.</param>
        public void UploadFile(string fileName, string remoteFileName)
        {
            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite))
            {
                sftpClient.BufferSize = 4 * 1024; // bypass Payload error large files
                sftpClient.UploadFile(fs, remoteFileName);
                fs.Close();
            }
        }

        /// <summary>
        /// Creates remote directory if it does not exist.
        /// </summary>
        /// <param name="remotePath"></param>
        public void CreateDirectory(string remotePath)
        {
            if (!sftpClient.Exists(remotePath)) sftpClient.CreateDirectory(remotePath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="remotePath"></param>
        public void CreateRemotePath(string remotePath)
        {
            string[] pathes = remotePath.Split('/');
            string currPath = "";
            foreach (var path in pathes)
            {
                currPath = currPath + "/" + path;
                if (!Exists(currPath))
                {
                    sftpClient.CreateDirectory(currPath);
                }
            }
        }

        /// <summary>
        /// Checks whether path or directory exists.
        /// </summary>
        /// <param name="remotePath"> Path to remote file or directory</param>
        /// <returns><c>true</c> if file or directory exists. Othervise returns <c>false</c>.></returns>
        public bool Exists(string remotePath)
        {
            return sftpClient.Exists(remotePath);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (sftpClient != null)
                    {
                        sftpClient.Dispose();
                    }
                }

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }


}

