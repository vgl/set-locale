using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using OfficeOpenXml;
using OfficeOpenXml.Style;
using ServiceStack.Text;

namespace set.locale.tool
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = @"C:\Work\set-crm\sources\set.crm.web";
            if (args.Any()
                && !string.IsNullOrWhiteSpace(args[0]))
            {
                if (!Directory.Exists(args[0]))
                {
                    throw new Exception(string.Format("directory not found > {0}", args[0]));
                }

                path = args[0];
            }

            var tag = "setcrm";
            if (args.Any()
                && !string.IsNullOrWhiteSpace(args[1]))
            {
                tag = args[1];
            }

            const string localizationstring = "Localize";
            var regexCsHtml = new Regex(string.Format("{0}\\(\".*\"\\)", localizationstring));
            var regexCs = new Regex(string.Format("\".*\".{0}\\(", localizationstring));

            var keyList = new List<string>();

            var viewFiles = new DirectoryInfo(string.Format(@"{0}\Views", path)).GetFiles("*.cshtml", SearchOption.AllDirectories).ToList();
            var csFiles = new DirectoryInfo(string.Format(@"{0}\", path)).GetFiles("*.cs", SearchOption.AllDirectories).ToList();

            GetStrings(viewFiles, localizationstring, regexCsHtml, keyList);

            GetStrings(csFiles, localizationstring, regexCs, keyList);

            PrepareLocalizationStrings(keyList, tag);

            PrepareExcel(keyList, tag);

            Console.WriteLine("total localization strings count > " + keyList.Count);
        }

        private static void GetStrings(IEnumerable<FileInfo> viewFiles, string localizationstring, Regex regexCsHtml, List<string> keyList)
        {
            foreach (var file in viewFiles)
            {
                var content = File.ReadAllText(file.FullName);

                if (!content.Contains(localizationstring)) continue;

                var m = regexCsHtml.Match(content);
                while (m.Success)
                {
                    var key = m.Groups[0].Value;
                    if (key.LastIndexOf(localizationstring, StringComparison.Ordinal) > 0)
                    {
                        var items = key.Split(new[] { localizationstring }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var item in items)
                        {
                            if (item.StartsWith("(")
                                && item != "(")
                            {
                                var theKey = item.Replace("(\"", string.Empty);
                                theKey = theKey.Substring(0, theKey.IndexOf('"'));
                                if (!keyList.Contains(theKey) && !string.IsNullOrWhiteSpace(theKey))
                                {
                                    keyList.Add(theKey);
                                    Console.WriteLine(theKey);
                                }
                            }
                            else if (item.StartsWith("\""))
                            {
                                var theKey = item.Replace("\"", string.Empty).Replace(".", string.Empty);
                                if (theKey.Contains(","))
                                {
                                    var keyItems = theKey.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                    theKey = keyItems.Last().Trim();
                                }

                                if (!keyList.Contains(theKey) && !string.IsNullOrWhiteSpace(theKey))
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
                        if (!keyList.Contains(theKey) && !string.IsNullOrWhiteSpace(theKey))
                        {
                            keyList.Add(theKey);
                            Console.WriteLine(theKey);
                        }
                    }

                    m = m.NextMatch();
                }
            }
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

        private static List<string> PrepareLocalizationStrings(IEnumerable<string> keyList, string tag)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(ConfigurationManager.AppSettings["ApiKey"]);

                return SetLocalizationStringsDictionary(client, keyList, tag);
            }
        }

        private static List<string> SetLocalizationStringsDictionary(HttpClient client, IEnumerable<string> keylList, string tag)
        {
            var currentKeyList = new List<string>();
            var items = new List<NameValue>();

            try
            {
                var page = 1;
                Task<string> response;

                while (page > 0)
                {
                    response = client.GetStringAsync(string.Format("http://locale.setcrm.com/api/locales?app={0}&page={1}", tag, page));
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

                var values = keylList as IList<string> ?? keylList.ToList();
                foreach (var key in values)
                {
                    if (!items.Exists(value => value.Name == key))
                    {
                        currentKeyList.Add(key);
                    }
                }

                var keys = string.Join(",", values);
                var pairs = new List<KeyValuePair<string,string>>
                {
                    new KeyValuePair<string, string>("keys", keys),
                    new KeyValuePair<string, string>("tag", tag)
                };

                var data = new FormUrlEncodedContent(pairs);
                var resp = client.PostAsync("http://locale.setcrm.com/api/addkeys", data);
                resp.Wait();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return currentKeyList;
        }
    }
}
