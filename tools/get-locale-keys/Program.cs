using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace GetLocaleKeys
{
    class Program
    {
        static void Main()
        {
            try
            {
                const string localizationstring = "LocalizationString";
                var regex = new Regex(string.Format("{0}\\(\".*\"\\)", localizationstring));

                var keyList = new List<string>();

                var viewFiles = new DirectoryInfo(@"C:\Work\set-locale\src\client\SetLocale.Client.Web\Views").GetFiles("*.cshtml", SearchOption.AllDirectories).ToList();
                var controllerFiles = new DirectoryInfo(@"C:\Work\set-locale\src\client\SetLocale.Client.Web\Controllers").GetFiles("*.cs", SearchOption.AllDirectories).ToList();

                var files = new List<FileInfo>();
                files.AddRange(viewFiles);
                files.AddRange(controllerFiles);

                foreach (var file in files)
                {
                    var content = File.ReadAllText(file.FullName);

                    if (!content.Contains(localizationstring)) continue;

                    var m = regex.Match(content);
                    while (m.Success)
                    {
                        var key = m.Groups[0].Value;
                        if (key.LastIndexOf(localizationstring, StringComparison.Ordinal) > 0)
                        {
                            var items = key.Split(new[] { localizationstring }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var item in items)
                            {
                                if (item.StartsWith("("))
                                {
                                    var theKey = item.Replace("(\"", string.Empty);
                                    theKey = theKey.Substring(0, theKey.IndexOf('"'));
                                    if (!keyList.Contains(theKey))
                                    {
                                        keyList.Add(theKey);
                                        Console.WriteLine(theKey);
                                    }
                                }
                            }
                        }
                        else
                        {
                            var theKey = key.Replace(localizationstring + "(\"", string.Empty);
                            theKey = theKey.Substring(0, theKey.IndexOf('"'));
                            if (!keyList.Contains(theKey))
                            {
                                keyList.Add(theKey);
                                Console.WriteLine(theKey);
                            }
                        }

                        m = m.NextMatch();
                    }
                }

                Console.WriteLine("total " + keyList.Count);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.Read();
        }
    }
}
