Add-PSSnapin SqlServerCmdletSnapin100
Add-PSSnapin SqlServerProviderSnapin100

# define parameters

#delete the script file if it exists
if (test-path $file)
{
	remove-item $file
}

# retrieve set of schema objects
$path = "sqlserver:\sql\$server\databases\$database\schemas"
$schemaset =get-childitem $path -ErrorAction stop
 
# script each schema
foreach ($schema in $schemaset)
{
  $schema.script() | out-file $file -append -ErrorAction stop
  "GO" | out-file $file -append
}

# retrieve set of table objects
$path = "sqlserver:\sql\$server\databases\$database\tables"
$tableset =get-childitem $path -ErrorAction stop
 
# script each table
foreach ($table in $tableset)
{
  $table.script() | out-file $file -append -ErrorAction stop
  "GO" | out-file $file -append
  foreach($index in $table.Indexes)
  {
	if($index.SpatialIndexType -eq 0)
	{
		$index.script() | out-file $file -append -ErrorAction stop
		"GO" | out-file $file -append
	}
  }
  foreach($column in $table.Columns)
  {
	foreach($default in $column.DefaultConstraint)
	{
		if($default -ne $null)
		{
			$default.script() | out-file $file -append -ErrorAction stop
			"GO" | out-file $file -append
		}
	}
  }
}

$path = "sqlserver:\sql\$server\databases\$database\views"
$viewset = get-childitem $path -ErrorAction stop

# script each view
foreach ($view in $viewset)
{
  $view.script() -replace "SET ANSI_NULLS ON", "" -replace "SET QUOTED_IDENTIFIER ON", "" | out-file $file -append -ErrorAction stop
  "GO" | out-file $file -append
}

$path = "sqlserver:\sql\$server\databases\$database\storedprocedures"
$storedprocedureset = get-childitem $path -ErrorAction stop

# script each stored proc
foreach ($storedprocedure in $storedprocedureset)
{
  $storedprocedure.script() -replace "SET ANSI_NULLS ON", "" -replace "SET QUOTED_IDENTIFIER ON", "" -replace "SET QUOTED_IDENTIFIER OFF", ""| out-file $file -append -ErrorAction stop
  "GO" | out-file $file -append
}

$path = "sqlserver:\sql\$server\databases\$database\userdefinedfunctions"
$functionset = get-childitem $path -ErrorAction stop

# script each user defined function
foreach ($function in $functionset)
{
  $function.script() -replace "SET ANSI_NULLS ON", "" -replace "SET QUOTED_IDENTIFIER ON", "" -replace "SET ANSI_NULLS OFF", "" -replace "SET QUOTED_IDENTIFIER OFF", "" | out-file $file -append -ErrorAction stop
  "GO" | out-file $file -append
}
