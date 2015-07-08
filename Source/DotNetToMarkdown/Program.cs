using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DotNetToMarkdown
{
    class Program
    {
        static void Main(string[] args)
        {
            var arguments = CommandLineArgs.Parse(args);

            using (var writer = new StreamWriter(arguments.MarkdownPath))
            {
                var filePaths = Directory.GetFiles(arguments.DllPath, "*.dll")
                    .OrderBy(Path.GetFileNameWithoutExtension)
                    .ThenBy(i => i);

                foreach (var filePath in filePaths)
                {
                    var assembly = Assembly.LoadFrom(filePath);
                    var groupedTypes = assembly
                        .GetExportedTypes()
                        .Where(i => !i.IsGenericType)
                        .OrderBy(i => i.Namespace)
                        .GroupBy(i => i.Namespace);

                    writer.WriteLine("## {0}", Path.GetFileName(filePath));
                    writer.WriteLine();

                    foreach (var group in groupedTypes)
                    {
                        var ns = group.Key;
                        var nsUrl = CreateUrl(arguments.UrlStringFormat, ns);
                        writer.WriteLine("**Namespace:** [{0}]({1})", ns, nsUrl);
                        
                        writer.WriteLine();

                        foreach (var type in group.OrderBy(i => i.Name))
                            writer.WriteLine(" - [`{0}`]({1})", type.Name, CreateUrl(arguments.UrlStringFormat, type.FullName));

                        writer.WriteLine();
                    }
                    
                    writer.WriteLine();
                }
            }
        }

        private static string CreateUrl(string urlFormat, string name)
        {
            return string.Format(urlFormat, name.ToLower());
        }
    }
}
