using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using OfficeOpenXml;
using OfficeOpenXml.Style;

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

                const string path = @"C:\Work\set-meta\sources";
                var viewFiles = new DirectoryInfo(string.Format(@"{0}\Views", path)).GetFiles("*.cshtml", SearchOption.AllDirectories).ToList();
                var controllerFiles = new DirectoryInfo(string.Format(@"{0}\Controllers", path)).GetFiles("*.cs", SearchOption.AllDirectories).ToList();

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

                PrepareExcel(keyList, "set-locale");

                

                Console.WriteLine("total " + keyList.Count);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.Read();
        }

        private static void PrepareExcel(List<string> keyList, string tag)
        {
            using (var p = new ExcelPackage())
            {
                p.Workbook.Properties.Title = "Words";

                p.Workbook.Worksheets.Add("Words");
                var workSheet = p.Workbook.Worksheets[1];

                //display table header
                workSheet.Cells[1, 1].Value = "key";
                workSheet.Cells[1, 2].Value = "tags";

                //set styling of header
                workSheet.Cells[1, 1, 1, 2].Style.Font.Bold = true;
                workSheet.Cells[1, 1, 1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                
                for (var i = 0; i < keyList.Count; i++)
                {
                    var row = i + 2;
                    workSheet.Cells[row, 1].Value = keyList[i];
                    workSheet.Cells[row, 2].Value = tag;
                }

                for (var i = 1; i <= 2; i++)
                {
                    workSheet.Column(i).AutoFit();
                }

                File.WriteAllBytes(string.Format("{0}-{1}.xlsx", tag, DateTime.Now.ToString("s").Replace(':', '-').Replace("T", "-")), p.GetAsByteArray());
            }
        }
    }
}
