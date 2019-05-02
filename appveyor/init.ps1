$ConnectionString = "Server=(local)\SQL2016;Database=master;User ID=sa;Password=$($env:SQL_SERVER_SA_PASSWORD)"
Invoke-SqlCmd -ConnectionString $ConnectionString -Query @" 
CREATE DATABASE ${env:SQL_SERVER_DATABASE};
GO
CREATE LOGIN ${env:SQL_SERVER_USER} WITH PASSWORD = '${env:SQL_SERVER_PASSWORD}';
GO
USE ${env:SQL_SERVER_DATABASE};
GO
CREATE USER ${env:SQL_SERVER_USER};
ALTER ROLE  db_owner  
       ADD MEMBER ${env:SQL_SERVER_USER}; 
"@
