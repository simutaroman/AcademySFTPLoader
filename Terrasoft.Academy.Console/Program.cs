using System;
using Terrasoft.Academy.SFTPLoader.Core;
using System.Configuration;

namespace Terrasoft.Academy.SFTPConsole
{

    class Program
    {
        static void Main(string[] args)
        {
            SFTPConfig config = new SFTPConfig();
            if (!IsArgumentsAreValid(args, ref config))
            {

                string ppkPath = ConfigurationManager.AppSettings["ppkFile"];
                if (string.IsNullOrWhiteSpace(ppkPath))
                {
                    ShowWrongConfigMessage("ppkFile");
                    return;
                }

                string ftpPath = ConfigurationManager.AppSettings["ftpPath"];
                if (string.IsNullOrWhiteSpace(ftpPath))
                {
                    ShowWrongConfigMessage("ftpPath");
                    return;
                }

                int port = Convert.ToInt32(ConfigurationManager.AppSettings["port"]);

                string userName = ConfigurationManager.AppSettings["userName"];
                if (string.IsNullOrWhiteSpace(userName))
                {
                    ShowWrongConfigMessage("userName");
                    return;
                }

                string remotePath = ConfigurationManager.AppSettings["remotePath"];
                if (string.IsNullOrWhiteSpace(remotePath))
                {
                    ShowWrongConfigMessage("remotePath");
                    return;
                }

                string localPath = ConfigurationManager.AppSettings["localPath"];
                if (string.IsNullOrWhiteSpace(localPath))
                {
                    ShowWrongConfigMessage("localPath");
                    return;
                }

                string ftpOperation = ConfigurationManager.AppSettings["ftpoperation"];
                if (string.IsNullOrWhiteSpace(localPath))
                {
                    ShowWrongConfigMessage("ftpoperation");
                    return;
                }

                FTPOperation operation = FTPOperation.None;
                if (ftpOperation == "upload") operation = FTPOperation.Upload;
                else if (ftpOperation == "download") operation = FTPOperation.Download;
                else if (ftpOperation == "clean") operation = FTPOperation.Clean;
                else throw new Exception("operation in app.config should be upload, download or clean");

                config.PpkPath = ppkPath;
                config.FtpPath = ftpPath;
                config.Port = port;
                config.UserName = userName;
                config.LocalPath = localPath;
                config.RemotePath = remotePath;
                config.FtpOperation = operation;

                Console.WriteLine("Values from config file:");
            }
            else
            {
                Console.WriteLine("Values from arguments:");
            }

            // TODO: perform help processing here!

            if (config.RemotePath.Contains("%20"))
            {
                config.RemotePath = config.RemotePath.Replace("%20", " ");
            }
            //config.WriteToConsole();// doesn't work in teamcity
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(config.GetConsoleString());
            Console.ResetColor();

            if (config.FtpOperation == FTPOperation.Upload)
            {
                ContentManager contentManager = new ContentManager();
                contentManager.LoadDirectoryContent(config);
            }
            else if (config.FtpOperation == FTPOperation.Download)
            {
                Console.WriteLine("Not supported yet. I'm so sorry...");
            }
            else if (config.FtpOperation == FTPOperation.Clean)
            {
                Console.WriteLine("Not supported yet. Intended to implement it soon");
            }
            else
                Console.WriteLine("Ooops! Something went wrong.");
        }

        static void ShowWrongConfigMessage(string value)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Wrong settings for {0} in app.config", value);
            Console.ResetColor();
        }

        static void ShowWrongArgumentMessage(string arg, string value)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Wrong argument value {0}. Should be {1}", arg, value);
            Console.ResetColor();
        }

        static bool IsArgumentsAreValid(string[] args, ref SFTPConfig config)
        {
            SFTPConfig cfg = new SFTPConfig();

            if ((args.Length == 1) && (args[0] == "-help"))
            {
                Console.WriteLine("Help for using Academy.SFTPConsole");
                return false;
            }
            if (args.Length == 0)
            {
                Console.WriteLine("There is no any input argument");
                return false;
            }
            if (args.Length == 14)
            {
                if (args[0] != "-ftp")
                {
                    ShowWrongArgumentMessage(args[0], "-ftp");
                    return false;
                }

                //Todo: check domain format here
                cfg.FtpPath = args[1];
                if (args[2] != "-port")
                {
                    ShowWrongArgumentMessage(args[2], "-port");
                    return false;
                }
                //Todo: check port value here
                cfg.Port = Convert.ToInt32(args[3]);

                if (args[4] != "-username")
                {
                    ShowWrongArgumentMessage(args[4], "-username");
                    return false;
                }
                cfg.UserName = args[5];

                if (args[6] != "-ftpoperation")
                {
                    ShowWrongArgumentMessage(args[6], "-ftpoperation");
                    return false;
                }

                if (args[7] == "upload")
                {
                    cfg.FtpOperation = FTPOperation.Upload;
                }
                else if (args[7] == "download")
                {
                    cfg.FtpOperation = FTPOperation.Download;
                }
                else if (args[7] == "clean")
                {
                    cfg.FtpOperation = FTPOperation.Clean;
                }
                else
                {
                    ShowWrongArgumentMessage(args[7], "upload, download or clean");
                    return false;
                }

                if (args[8] != "-localpath")
                {
                    ShowWrongArgumentMessage(args[8], "-localpath");
                    return false;
                }
                cfg.LocalPath = args[9];

                if (args[10] != "-remotepath")
                {
                    ShowWrongArgumentMessage(args[10], "-remotepath");
                    return false;
                }
                cfg.RemotePath = args[11];

                if (args[12] != "-ppkpath")
                {
                    ShowWrongArgumentMessage(args[12], "-ppkpath");
                    return false;
                }
                cfg.PpkPath = args[13];

                config.CopyFrom(cfg);

                return true;
            }
            return false;
        }
    }
}
