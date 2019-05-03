ALTER DATABASE [$(DatabaseName)]
    ADD FILE (NAME = [AlwaysEncryptedSample], FILENAME = 'AlwaysEncryptedSample.mdf', SIZE = 8192 KB, FILEGROWTH = 65536 KB) TO FILEGROUP [PRIMARY];

