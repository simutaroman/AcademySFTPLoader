using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Terrasoft.Academy.SFTPLoader.Core
{

    public class LoadCompletedEventArgs : EventArgs
    {
        public string ContentName { get; set; }

        public int ContentCount { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ContentManager
    {
        public event EventHandler LoadContentStarted;

        protected virtual void OnLoadContentStarted(LoadCompletedEventArgs e)
        {
            if (LoadContentStarted != null)
                LoadContentStarted(this, e);
        }

        public event EventHandler LoadCompleted;

        protected virtual void OnLoadCompleted(LoadCompletedEventArgs e)
        {
            if (LoadCompleted != null)
                LoadCompleted(this, e);
        }

        public void LoadDirectoryContent(SFTPConfig cfg)
        {
            using (SFTPClient client = new SFTPClient(cfg.PpkPath, cfg.FtpPath, cfg.Port, cfg.UserName))
            {
                DateTime startTime = DateTime.Now;

                client.Connect();

                Console.WriteLine("Uploading directories structure...");

                if (!client.Exists(cfg.RemotePath))
                {
                    client.CreateRemotePath(cfg.RemotePath);
                }
                Console.ForegroundColor = ConsoleColor.Green;
                FileHelper.ProcessDirectories(cfg.LocalPath, dir =>
                {
                    string ftpdir = cfg.RemotePath + "/" + dir.FullName.Replace(cfg.LocalPath, "").Replace("\\", "/").TrimEnd('/');
                    if (cfg.RemotePath.Equals(ftpdir))
                    {
                        return;
                    }
                    client.CreateDirectory(ftpdir);
                    Console.WriteLine(ftpdir);
                });

                Console.ResetColor();
                var midDateTime = DateTime.Now;

                Console.WriteLine();
                Console.WriteLine("Uploading files...");

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                FileHelper.ProcessFiles(cfg.LocalPath, file =>
                {
                    string ftpfile = cfg.RemotePath + "/" + file.FullName.Replace(cfg.LocalPath, "").Replace("\\", "/");
                    client.UploadFile(file.FullName, ftpfile);
                    Console.WriteLine(ftpfile);
                });

                Console.ResetColor();
                client.Disconnect();
                var endDateTime = DateTime.Now;
                Console.WriteLine("StartTime : " + startTime);
                Console.WriteLine("Folders EndTime : " + midDateTime);
                Console.WriteLine("EndTime : " + endDateTime);
                Console.WriteLine("Folders Execution Time : " + (midDateTime - startTime));
                Console.WriteLine("Total Execution Time : " + (endDateTime - startTime));
                Console.WriteLine();
                Console.WriteLine("Uploading completed..");
                Console.WriteLine();

            }
        }
    }
}
