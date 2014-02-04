using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

using OfficeOpenXml;
using OfficeOpenXml.Style;
using ServiceStack.Text;

namespace GetLocaleKeys
{
    class Program
    {
        static void Main()
        {
            try
            {
                const string path = @"C:\work\set-meta\sources";
                const string tag = "set-meta";

                //string path = args[0];
                //string tag = args[1];

                if (!Directory.Exists(path))
                {
                    Console.WriteLine("{0} is not a valid directory.", path);
                }
                else if (tag.Trim() == "")
                {
                    Console.WriteLine("Tag shouldnt be empty.");
                }
                else
                {

                    const string localizationstring = "LocalizationString";
                    var regex = new Regex(string.Format("{0}\\(\".*\"\\)", localizationstring));

                    var keyList = new List<string>();

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

                    var newKeyList = PrepareLocalizationStrings(keyList, tag);

                    PrepareExcel(newKeyList, tag);

                    Console.WriteLine("total " + keyList.Count);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "testt1");
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

        private static List<string> PrepareLocalizationStrings(List<string> keyList, string tag)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("418001e2fb1d44e0acbe151dad8f1eca");

                return SetLocalizationStringsDictionary(client, keyList, tag);
            }

        }

        private static List<string> SetLocalizationStringsDictionary(HttpClient client, List<string> keylList, string tag)
        {
            var currentKeyList = new List<string>();

            var items = new List<NameValue>();

            try
            {
                var page = 1;
                while (page > 0)
                {
                    var response = client.GetStringAsync(string.Format("http://setlocale.azurewebsites.net/api/locales?page={0}", page));
                    response.Wait();

                    var responseBody = response.Result;

                    var responseItems = JsonSerializer.DeserializeFromString<List<NameValue>>(responseBody);

                    if (responseItems == null || !responseItems.Any())
                    {
                        page = 0;
                        continue;
                    }

                    items.AddRange(responseItems);

                    page++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "testt2");
            }

            try
            {
                foreach (var key in keylList)
                {
                    if (!items.Exists(value => value.Name == key))
                    {
                        currentKeyList.Add(key);
                    }
                }

                var keys = "";

                foreach (var item in currentKeyList)
                {
                    keys = keys + item + ",";
                }

                var asd = client.GetStringAsync(string.Format("http://setlocale.azurewebsites.net/api/AddKeys?keys={0}&tag={1}", keys, tag));
                asd.Wait();

                var qwe = asd.Result;

                var zxc = JsonSerializer.DeserializeFromString<bool>(qwe);
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            

            return currentKeyList;
        }
    }
}
