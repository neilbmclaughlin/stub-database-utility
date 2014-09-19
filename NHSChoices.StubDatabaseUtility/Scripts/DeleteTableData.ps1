[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.SMO") | out-null
$smoserver = New-Object ('Microsoft.SqlServer.Management.Smo.Server') -argumentlist $server

foreach($table in $smoserver.Databases[$database].tables)
{
	$table.TruncateData()
}