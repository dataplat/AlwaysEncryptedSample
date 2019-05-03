[cmdletbinding()]
param(
	[Parameter(ValueFromPipeline = $true)] [string] $ConnectionString = "Data Source=$($env:SQL_SERVER_INSTANCE);Initial Catalog=$($env:SQL_SERVER_DATABASE);Integrated Security=SSPI;Application Name=Start-EntityFrameworkMigration;Column Encryption Setting=enabled"
)

<#
$efPkg = ([xml](Get-Content .\AlwaysEncryptedSample.Models\packages.config)).packages.package | 
    Where-Object Id -eq 'EntityFramework'

Import-Module ".\packages\EntityFramework.$($efPkg.version)\tools\EntityFramework.psm1"

Get-Help Update-Database
#>

# TODO: Dynamically extract bin\debug
$DllPath = Join-Path "$($PsScriptRoot)" "..\AlwaysEncryptedSample.Models\bin\Debug\AlwaysEncryptedSample.Models.dll"
Add-Type -Path $DllPath

$cn = [System.Data.SqlClient.SqlConnection] $ConnectionString
[AlwaysEncryptedSample.Models.DbInit]::InitLog4NetDb($cn)
[AlwaysEncryptedSample.Models.DbInit]::CreateAuthContext($ConnectionString)
[AlwaysEncryptedSample.Models.DbInit]::CreatePurchasingContext($ConnectionString)