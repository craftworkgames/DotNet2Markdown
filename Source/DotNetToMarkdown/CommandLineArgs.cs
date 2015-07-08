using System.Collections.Generic;

namespace DotNetToMarkdown
{
    public class CommandLineArgs
    {
        private CommandLineArgs()
        {
        }

        public string MarkdownPath { get; private set; }
        public string DllPath { get; private set; }
        public string UrlStringFormat { get; private set; }

        public static CommandLineArgs Parse(string[] args)
        {
            var dictionary = new Dictionary<string, string>();

            foreach (var a in args)
            {
                var keyValue = a.Split('=');
                var key = keyValue[0].Trim().ToLowerInvariant();
                var value = keyValue[1].Trim();

                dictionary.Add(key, value);
            }

            return new CommandLineArgs
            {
                MarkdownPath = dictionary.GetValueOrDefault("-output", "output.md"),
                DllPath = dictionary.GetValueOrDefault("-input", "."),
                UrlStringFormat = dictionary.GetValueOrDefault("-urlformat", "https://msdn.microsoft.com/en-us/library/{0}.aspx")
            };
        }
    }
}