using System;
using System.Text;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Collections.ObjectModel;
using System.IO;

namespace Examples
{
    class ExecutePowershell
    {
        public static Collection<PSObject> RunScriptUsePowerShell(string filePath = @".\scriptsample.ps1")
        {
            Collection<PSObject> output;
            using (var ps = PowerShell.Create())
            {
                ps.AddScript(LoadScript(filePath));
                output = ps.Invoke();
            }

            return output;
        }
        public static string RunScriptUseRunspace(string filePath = @".\scriptsample.ps1")
        {
            var script = LoadScript(filePath);

            var result = new StringBuilder();
            using (var runspace = RunspaceFactory.CreateRunspace())
            {
                runspace.Open();
                using (var pipeline = runspace.CreatePipeline())
                {
                    pipeline.Commands.AddScript(script);
                    pipeline.Commands.Add("Out-String");
                    var outputs = pipeline.Invoke();

                    foreach (var pobject in outputs)
                    {
                        result.AppendLine(pobject.ToString());
                    }
                }
            }

            return result.ToString();
        }
        private static string LoadScript(string filePath)
        {
            var script = "";
            try
            {
                script = File.ReadAllText(filePath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return script;
        }
    }
}
