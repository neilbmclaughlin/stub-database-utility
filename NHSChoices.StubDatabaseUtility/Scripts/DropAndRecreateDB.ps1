Add-PSSnapin SqlServerCmdletSnapin100
Add-PSSnapin SqlServerProviderSnapin100

[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.SMO") | out-null
$smoserver = New-Object ('Microsoft.SqlServer.Management.Smo.Server') -argumentlist $server

# drop database if it exists
if ($smoserver.Databases[$dbName] -ne $null)  
{
	$smoserver.killallprocesses($dbName)
    $smoserver.Databases[$dbName].drop()  
}

# re create the database

$database = New-Object Microsoft.SqlServer.Management.Smo.Database($server, $dbName)
$database.Create()

#run the sql script

invoke-sqlcmd -inputfile $file -serverinstance $server -database $dbName