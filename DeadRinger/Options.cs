using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadRinger
{
    public class Options
    {
        [Option('p', "path", Required = true, HelpText = "Path of application to run.")]
        public string Path { get; set; }

        [Option('a', "arguments", HelpText = "Optional arguments for the application.")]
        public string Arguments { get; set; }

        [Option('d', "delay", DefaultValue = 0, HelpText = "Maximum amount of time (milliseconds) to delay application's execution.")]
        public int Delay { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
