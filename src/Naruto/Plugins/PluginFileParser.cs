using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Naruto.Plugins
{
    public class PluginFileParser
    {
        public static IList<string> ParseInstalledPluginsFile(string filePath)
        {
            //read and parse the file
            if (!File.Exists(filePath))
                return new List<string>();

            var text = File.ReadAllText(filePath);
            if (String.IsNullOrEmpty(text))
                return new List<string>();

            //Old way of file reading. This leads to unexpected behavior when a user's FTP program transfers these files as ASCII (\r\n becomes \n).
            //var lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            var lines = new List<string>();
            using (var reader = new StringReader(text))
            {
                string str;
                while ((str = reader.ReadLine()) != null)
                {
                    if (String.IsNullOrWhiteSpace(str))
                        continue;
                    lines.Add(str.Trim());
                }
            }
            return lines;
        }

        public static void SaveInstalledPluginsFile(IList<String> pluginSystemNames, string filePath)
        {
            string result = "";
            foreach (var sn in pluginSystemNames)
                result += $"{sn}{Environment.NewLine}";

            File.WriteAllText(filePath, result);
        }
    }
}
