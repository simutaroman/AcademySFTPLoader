using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;

namespace Terrasoft.Academy.SFTPLoader.Core
{
    /// <summary>
    /// Utility class to help process files and directories.
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// Performs action for directory and nested subdirectories.
        /// </summary>
        /// <param name="sourceDirName"> Local directory path.</param>
        /// <param name="action">Action to perform.</param>
        public static void ProcessDirectories(string sourceDirName, Action<DirectoryInfo> action)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }
            action(dir);
            Parallel.For(0, dirs.Length,
                index => { ProcessDirectories(dirs[index].FullName, action); }
                );
        }

        /// <summary>
        /// Performs action for every file in directory and nested subdrectory.
        /// </summary>
        /// <param name="sourceDirName">Local directory path.</param>
        /// <param name="action">Action to perform.</param>
        public static void ProcessFiles(string sourceDirName, Action<FileInfo> action)
        {
            List<string> filePaths = new List<string>();
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }
            // Get the files in the directory and copy them to the new location.
            var files = dir.GetFiles("*.*", SearchOption.AllDirectories);
            Parallel.ForEach<FileInfo>(files, action);
        }
    }
}
