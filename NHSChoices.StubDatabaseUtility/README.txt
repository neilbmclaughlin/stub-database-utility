Introduction

Using the stub database utility, stub databases can be created and loaded up with test data for 
running integration or acceptance tests against. The utility will create a copy of a database without 
any data in it. The utility provides two methods which should be used when using created stub 
databases. 

	- DropAndRecreateDatabase should be used once at the start of each test run. This method creates 
	  a script which creates all tables, stored procs, functions and indexes from the source database.
	  The method then drops and recreates the test database to remove any created database from 
	  previous test runs and create a fresh version based on the source server (an integration server 
	  for the appropriate release branch).
	- DeleteTableData - This function should be run in the setup of each test. It goes through each 
	  table in the test database and clears down any created test data.

Parameters

The two functions take an object of type DatabaseStubArguments as input. The properties of this object
contain the information required when performing the two functions above.
	- DestinationServer is the sql server containing the test database which will be created. This can 
	  be the address of sql server on your VM eg. VHCDEVSRVXXX\SQL2008R2 or SQL Express on your host
	  machine eg. HCDEVPCXXX\SQLEXPRESS.
	- SourceServer is the sql server containing the database to take a copy of. This should be the 
	  integration server for the relevant release. This is only required when using the 
	  DropAndRecreateDatabase function.
	- DatabaseName is the name of the database to take a copy of.
	- StubDatabasePrefix is the prefix to attach to the database name when creating a test database. 
	  e.g if the database name is PIMS and the stub database prefix is testdb then the created 
	  database will be called testdb_PIMS. If nothing is supplied for this property then it will 
	  default to 'acceptance', but it is recommended that something related to the product being 
	  tested is assigned to this property so concurrent builds can be run wihtout any conflicts.

Usage

If testing using multiple databases, the functions will have to be called once for each database. This 
is currently done by storing the database names in a comma seperated list in app config and iterating 
over this list.

The app config in a project which is using test database created with this utility will need to 
contain connection strings to point the code to the test databases. This connection string should 
point to the same server which is supplied in the destination server property and should point to a 
database named with the StubdatabasePrefix argument prefixed to the database name.

In NUnit, the DropAndRecreateDatabase function can be called in a class tagged with the SetUpFixture 
attribute containing a method tagged with the SetUp attribute. This SetUp method will be called once 
at the beggining of each test run for tests within the same namespace as the setup fixture class.

If using SpecFlow then the binding attribute BeforeTestRun will achieve the same result.

Before each test is run the DeleteTableData function should be called followed by and scripts to 
create seeded data . This can be done in NUnit using a SetUp method in a master class which all tests 
inherit from. Specflow can achieve this using the BeforeScenario attribute.