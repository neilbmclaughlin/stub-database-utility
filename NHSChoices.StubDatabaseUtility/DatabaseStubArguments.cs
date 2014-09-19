namespace NHSChoices.StubDatabaseUtility
{
  public class DatabaseStubArguments
  {
    public string DatabaseName { get; set; }

    /// <summary>
    /// The target server for creating the test database
    /// </summary>
    public string DestinationServer { get; set; }

    /// <summary>
    /// The server which contains the database to create an empty test database from. 
    /// This is not required when calling DeleteTableData
    /// </summary>
    public string SourceServer { get; set; }

    private string _stubDatabasePrefix = "acceptance";

    /// <summary>
    /// The prefix which preceeds the test database name. This will default to 'acceptance'
    /// </summary>
    public string StubDatabasePrefix
    {
      get { return _stubDatabasePrefix; }
      set { _stubDatabasePrefix = value; }
    }

    public string StubDatabaseName
    {
      get
      {
        return string.Format("{0}_{1}", StubDatabasePrefix, DatabaseName);
      }
    }
  }
}
