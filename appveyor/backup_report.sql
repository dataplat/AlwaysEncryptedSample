-- Originally from https://www.mssqltips.com/sqlservertip/2960/sql-server-backup-paths-and-file-management/
-- File name : Where are the backups.sql
-- Author : Graham Okely B App Sc
-- Scope : OK on SQL Server 2000-2019
-- Select the information we require to make a decision about which backup we want to use

select  top 5 a.server_name, a.database_name, backup_finish_date, a.backup_size,
CASE a.[type] -- Let's decode the three main types of backup here
 WHEN 'D' THEN 'Full'
 WHEN 'I' THEN 'Differential'
 WHEN 'L' THEN 'Transaction Log'
 ELSE a.[type]
END as BackupType
 ,b.physical_device_name
from msdb.dbo.backupset a join msdb.dbo.backupmediafamily b
  on a.media_set_id = b.media_set_id
  where a.database_name = '$(SQL_SERVER_DATABASE)'
order by a.backup_finish_date desc