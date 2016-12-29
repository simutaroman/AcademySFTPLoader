---------------
About
---------------

Performs "fast and furious" uploading files and directories to bpm'online (Terrasoft) Academy FTP server (https://academy.bpmonline.com) through SFTP.

Created by Roman Simuta for Terrasoft Academy team. Inspired by AcademyLoader by V.Nikonov.

Feel free to contact me by email: simutaroman@gmail.com.

---------------
How to use
---------------

Use Terrasoft.Academy.SFTPConsole app.config to set custom settings if you are going to use only one remote path for long time. 
Use application params for different pathes.

Available combinations of parameters:

1. One parameter supplied.
  1. -help (TODO)* Obvious parameter. Lists available parameters.

Example:
  Terrasoft.Academy.SFTPConsole.exe -help

2. Seven parameters and their values supplied.
  1. -ftp: FTP site name.
  2. -port: SFTP port allowed for connection.
  3. -username: Valid FTP user name.
  4. -ftpoperation:	Operation to do with files and directories. Available values:
    * upload : files from local path directory will be uploaded to remote path directory.
    * clean : empties remote path directory. (TODO)*
    * download: files from remote path directory will be downloaded to local path directory.(TODO)*
  5. -localpath: Local path directory.
  6. -remotepath: Remote path directory. Will be created if not exists.
    * If remote path should contain space, use %20 sequence. i.e. - remotepath some%20path/with/spaces
  7. -ppkpath: Path to SSH key file. See Terrasoft.Academy.SFTPLoader.Core/ThirdParty/How to convert ppk file.docx

Example:
  Terrasoft.Academy.SFTPConsole.exe -ftp somesite.com -port 3333 -username ftpuser -ftpoperation upload -localpath C:\test\ -remotepath docs/test -ppkpath C:\key\key.ppk

* Features marked as (TODO) are not realised yet.


---------------
Where to find
---------------
Last changes may be found here: https://github.com/simutaroman/AcademySFTPLoader


