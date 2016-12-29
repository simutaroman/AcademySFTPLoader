using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Terrasoft.Academy.SFTPLoader.Core
{

    /// <summary>
    /// Direction of SFTP operation.
    /// </summary>
    public enum FTPOperation {
        /// <summary>
        /// No or unknown operation
        /// </summary>
        None,
        /// <summary>
        /// Upload operation
        /// </summary>
        Upload,
        /// <summary>
        /// Dowload operation
        /// </summary>
        Download,
        /// <summary>
        /// Clean remote directory
        /// </summary>
        Clean
    };

    /// <summary>
    /// Configures SFTP connection.
    /// </summary>
    public class SFTPConfig
    {
        /// <summary>
        /// Path to ppk-key file.
        /// </summary>
        public string PpkPath { get; set; }

        /// <summary>
        /// FTP path.
        /// </summary>
        public string FtpPath { get; set; }

        /// <summary>
        /// Allowed connection port.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// FTP user name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Path to local directory.
        /// </summary>
        public string LocalPath { get; set; }

        /// <summary>
        /// Relaive path to remote directory.
        /// </summary>
        public string RemotePath { get; set; }

        /// <summary>
        /// Operation to perform.
        /// </summary>
        public FTPOperation FtpOperation { get; set; }

        /// <summary>
        /// Copies properties of other config instance.
        /// </summary>
        /// <param name="cfg">SFTPConfig instance.</param>
        public void CopyFrom(SFTPConfig cfg)
        {
            PpkPath = cfg.PpkPath;
            FtpPath = cfg.FtpPath;
            Port = cfg.Port;
            UserName = cfg.UserName;
            LocalPath = cfg.LocalPath;
            RemotePath = cfg.RemotePath;
            FtpOperation = cfg.FtpOperation;
        }

        /// <summary>
        /// Prints properties to console.
        /// </summary>
        public void WriteToConsole()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("{0}: {1}", "PpkPath", PpkPath);
            Console.WriteLine("{0}: {1}", "FtpPath", FtpPath);
            Console.WriteLine("{0}: {1}", "Port", Port);
            Console.WriteLine("{0}: {1}", "UserName", UserName);
            Console.WriteLine("{0}: {1}", "LocalPath", LocalPath);
            Console.WriteLine("{0}: {1}", "RemotePath", RemotePath);
            Console.WriteLine("{0}: {1}", "Operation", FtpOperation);
            Console.ResetColor();
        }

        /// <summary>
        /// Returns formated string with props values.
        /// </summary>
        /// <returns> formated string with props values.</returns>
        public string GetConsoleString()
        {
            string s = string.Format("{0}: {1}", "PpkPath", PpkPath) + System.Environment.NewLine;
            s += string.Format("{0}: {1}", "FtpPath", FtpPath) + System.Environment.NewLine;
            s += string.Format("{0}: {1}", "Port", Port) + System.Environment.NewLine;
            s += string.Format("{0}: {1}", "UserName", UserName) + System.Environment.NewLine;
            s += string.Format("{0}: {1}", "LocalPath", LocalPath) + System.Environment.NewLine;
            s += string.Format("{0}: {1}", "RemotePath", RemotePath) + System.Environment.NewLine;
            s += string.Format("{0}: {1}", "Operation", FtpOperation) + System.Environment.NewLine;
            return s;
        }
    }
}
