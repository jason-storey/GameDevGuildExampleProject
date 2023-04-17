using System.Collections.Generic;

namespace JCore.Commands
{
    public class CommandLineParser
    {
        public Dictionary<string, List<string>> Commands { get; }

        public CommandLineParser(string args)
        {
            Commands = new Dictionary<string, List<string>>();

            if (string.IsNullOrEmpty(args))
                return;

            string[] tokens = args.Split(' ');
            string currentCommand = null;

            foreach (var token in tokens)
            {
                if (token.StartsWith("-"))
                {
                    currentCommand = token.Substring(1);
                    Commands[currentCommand] = new List<string>();
                }
                else if (currentCommand != null) Commands[currentCommand].Add(token);
            }
        }
    }
}