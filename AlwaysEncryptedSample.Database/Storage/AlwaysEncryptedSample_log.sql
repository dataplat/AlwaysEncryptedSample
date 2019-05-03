ALTER DATABASE [$(DatabaseName)]
    ADD LOG FILE (NAME = [AlwaysEncryptedSample_log], FILENAME = 'AlwaysEncryptedSample_log.ldf', SIZE = 8192 KB, MAXSIZE = 2097152 MB, FILEGROWTH = 65536 KB);

