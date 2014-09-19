namespace NHSChoices.StubDatabaseUtility
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Management.Automation.Runspaces;
  using System.Reflection;

  internal class PowerShellScriptRunner
  {
    public static void RunPowerShellScript(
      string scriptPath,
      IEnumerable<KeyValuePair<string, string>> parameters)
    {
      using (var runspace = RunspaceFactory.CreateRunspace())
      using (var pipeline = runspace.CreatePipeline())
      {
        pipeline.Commands.AddScript(ScriptFromResource(scriptPath));

        runspace.Open();

        foreach (var parameter in parameters)
        {
          runspace.SessionStateProxy.SetVariable(parameter.Key, parameter.Value);
        }

        pipeline.Invoke();
      }
    }

    private static string ScriptFromResource(string scriptPath)
    {
      using (var resourceStream = Assembly
        .GetExecutingAssembly()
        .GetManifestResourceStream(scriptPath))
      {
        if (resourceStream == null)
        {
          throw new ApplicationException(
            string.Format("Error locating Powershell script '{0}'",
            scriptPath));
        }

        using (var streamReader = new StreamReader(resourceStream))
        {
          return streamReader.ReadToEnd();
        }
      }
    }
  }
}
