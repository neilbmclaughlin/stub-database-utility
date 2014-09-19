namespace NHSChoices.StubDatabaseUtility
{
  using System.Collections.Generic;

  public class StubDatabaseUtility
  {
    private const string SqlScriptFileName = "sqlScript.sql";
    private const string DropAndRecreateDatabaseScriptPath = "NHSChoices.StubDatabaseUtility.Scripts.DropAndRecreateDB.ps1";
    private const string CreateDatabaseSchemaScriptPath = "NHSChoices.StubDatabaseUtility.Scripts.ScriptDatabase.ps1";
    private const string DeleteTableDataScriptPath = "NHSChoices.StubDatabaseUtility.Scripts.DeleteTableData.ps1";

    /// <summary>
    /// This method will create a script based on the database name and SourceServer provided which will create a 
    /// copy of the database with no data in it. The script is then run against the DestinationServer to create the 
    /// database. This method should only be called once at the beggining of a test run.
    /// </summary>
    /// <param name="arguments">You must include the DestinationServer, SourceServer and DatabaseName parameters. 
    /// It is also recommended that you provide a value for StubDatabasePrefix to prevent conflicts with other projects
    /// using this utility.</param>
    public static void DropAndRecreateDatabase(DatabaseStubArguments arguments)
    {
      CreateDatabaseSchemaScript(arguments);

      var parameters = new Dictionary<string, string>
        {
          {"server", arguments.DestinationServer},
          {"dbName", arguments.StubDatabaseName},
          {"file", SqlScriptFileName}
        };

      PowerShellScriptRunner.RunPowerShellScript(DropAndRecreateDatabaseScriptPath, parameters);
    }

    /// <summary>
    /// This method clears out all the data from a created test database. It should be run before each test using the 
    /// test databases.
    /// </summary>
    /// <param name="arguments">You must include the DestinationServer and the DatabaseName parameters. If a StubDatabasePrefix 
    /// value was provided when creating the test database, then this must match what is passed in for this method.</param>
    public static void DeleteTableData(DatabaseStubArguments arguments)
    {
      var parameters = new Dictionary<string, string>
                           {
                             {
                               "server",
                               arguments.DestinationServer
                             },
                             {
                               "database",
                               arguments.StubDatabaseName
                             }
                           };

      PowerShellScriptRunner.RunPowerShellScript(DeleteTableDataScriptPath, parameters);
    }

    private static void CreateDatabaseSchemaScript(DatabaseStubArguments arguments)
    {
      var parameters = new Dictionary<string, string>
        {
          {"server", arguments.SourceServer},
          {"database", arguments.DatabaseName},
          {"file", SqlScriptFileName}
        };

      PowerShellScriptRunner.RunPowerShellScript(CreateDatabaseSchemaScriptPath, parameters);
    }
  }
}
