using System;
using System.Diagnostics;
using System.IO;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace DeadRinger
{
    public class ApplicationRunner
    {
        public Options Options;
        public log4net.ILog Log { get; set; }

        public ApplicationRunner()
        {
            Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }

        public void Execute(params string[] args)
        {
            Options = new Options();
            if (!CommandLine.Parser.Default.ParseArguments(args, Options))
                return;

            // Wait some ramdom amount of time, the maximum of which is specified by the delay user
            Random random = new Random(DateTime.Now.Millisecond);
            var delay = random.Next(Options.Delay);
            System.Threading.Thread.Sleep(delay);

            // Do not execute the application if the application does not exist/cannot access it.
            if (!File.Exists(Options.Path))
            {
                Log.Error(String.Format("Cannot run {0}. It either does not exist or is inaccessible. Exiting...", Options.Path));
                return;
            }

            // Add the app path and optional arguments to the output              
            var output = String.Format("Application: {0}", Options.Path);
            if (!String.IsNullOrWhiteSpace(Options.Arguments))
                output += " " + Options.Arguments;
            output += Environment.NewLine;

            // Run the application and log the result
            var processStartInfo = new ProcessStartInfo(Options.Path, Options.Arguments)
            {
                UseShellExecute = false,
                ErrorDialog = false,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
            };
            var process = new Process();
            process.StartInfo = processStartInfo;
            var processStarted = process.Start();
            var inputWriter = process.StandardInput;
            var outputReader = process.StandardOutput;
            var errorReader = process.StandardError;
            process.WaitForExit();

            String line;
            output += "StdOut: " + Environment.NewLine;
            while ((line = outputReader.ReadLine()) != null)
                output += line + Environment.NewLine;

            output += Environment.NewLine + "StdErr: " + Environment.NewLine;
            while ((line = errorReader.ReadLine()) != null)
                output += line + Environment.NewLine;

            Log.Info(output);
        }
    }
}
