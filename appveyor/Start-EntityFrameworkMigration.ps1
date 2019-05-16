[cmdletbinding()]
param(
    [Parameter(ValueFromPipeline = $true)]
    [string] $ConnectionString =
    "Data Source=$($env:SQL_SERVER_INSTANCE);Initial Catalog=$($env:SQL_SERVER_DATABASE);Integrated Security=SSPI;Application Name=Start-EntityFrameworkMigration;Column Encryption Setting=enabled"
)

# TODO: Dynamically extract bin\debug
$DllPath = Join-Path "$($PsScriptRoot)" "..\AlwaysEncryptedSample.Models\bin\Debug\AlwaysEncryptedSample.Models.dll"
Add-Type -Path $DllPath

$cn = [System.Data.SqlClient.SqlConnection] $ConnectionString
Write-Verbose "Initializing Log4Net Schema"
[AlwaysEncryptedSample.Models.DbInit]::InitLog4NetDb($cn)
Write-Verbose "Initializing ASP.NET Identity Schema"
[AlwaysEncryptedSample.Models.DbInit]::CreateAuthContext($ConnectionString)
Write-Verbose "Initializing Application Schema"
[AlwaysEncryptedSample.Models.DbInit]::CreatePurchasingContext($ConnectionString)
