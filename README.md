set.locale
=========

set.locale is a localization strings service.

it aims to decrease the translation operations of multi language app teams

if you use set.locale

  - developers don't need to send every new label to the translators
  - if you set the utiliy your every commit will be automaticly crawled for the new labels and pushed to set.locale panel
  - translators can easyly edit strings from a panel
  - tranaslators can see pending translations with no efford
  
API usage
----

you send a GET request with your token, service returns a json Name - Value array.

there is 3 query strings for request.

**tag** - to set desired key list (default is set)

**lang** - to set the language (default is tr)

**page** - to set the page number (deafult is 1 and the page item size is 100)

```
http://locale.setcrm.com/api/locales?lang=en&tag=setcrm
```

```
GET /api/locales?lang=en&tag=setcrm HTTP/1.1
Host: locale.setcrm.com
Authorization: {your-token}
Cache-Control: no-cache
````

```
[
    {
        "Name": "title_product_new",
        "Value": "New Product From"
    },
    {
        "Name": "menu_products",
        "Value": "Products"
    },
    {
        "Name": "link_account_new",
        "Value": "New Account"
    }
]

```


How to use in an asp.net app
-----------

a way to consume this service is getting all the strings on app start.


```
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            PrepareLocalizationStrings();
        }
    
        private void PrepareLocalizationStrings()
        {
            var enTexts = new Dictionary<string, string>();
            var trTexts = new Dictionary<string, string>();
    
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ConstHelper.MediaTypeJson));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(ConfigurationManager.AppSettings[ConstHelper.LocaleApiKey]);
    
                SetLocalizationStringsDictionary(client, trTexts, ConstHelper.tr);
                SetLocalizationStringsDictionary(client, enTexts, ConstHelper.en);
            }
    
            Application.Add(ConstHelper.CultureNameTR, trTexts);
            Application.Add(ConstHelper.CultureNameEN, enTexts);
        }
    
        private static void SetLocalizationStringsDictionary(HttpClient client, IDictionary<string, string> dictionary, string languageKey)
        {
            try
            {
                var page = 1;
                while (page > 0)
                {
                    var response = client.GetStringAsync(string.Format("http://locale.setcrm.com/api/locales?tag=set&lang={0}&page={1}", languageKey, page));
                    response.Wait();
    
                    var responseBody = response.Result;
                    var items = JsonSerializer.DeserializeFromString<List<NameValue>>(responseBody);
    
                    if (items == null
                        || !items.Any())
                    {
                        page = 0;
                        continue;
                    }
    
                    foreach (var item in items)
                    {
                        if (dictionary.ContainsKey(item.Name))
                        {
                            dictionary[item.Name] = item.Value;
                        }
                        else
                        {
                            dictionary.Add(item.Name, item.Value);
                        }
                    }
    
                    page++;
                }
            }
            catch { }
        }
    }
```

##### And you need a helper to get the localized string by key

```
    public static class LocalizationHelper
    {
        public static string LocalizationString(this HtmlHelper helper, string key)
        {
            try
            {
                return ((Dictionary<string, string>)HttpContext.Current.Application[Thread.CurrentThread.CurrentUICulture.Name])[key];
            }
            catch { return key; }
        }

        public static string LocalizationString(string key)
        {
            return LocalizationString(null, key);
        }
    }
```

##### you can use this helper in razor views like this.


```
<label for="Email">@Html.LocalizationString("email")</label>
```


##### you can use this helper in code behind like this.


```
model.Msg = LocalizationHelper.LocalizationString("please_check_the_fields_and_try_again");
```












