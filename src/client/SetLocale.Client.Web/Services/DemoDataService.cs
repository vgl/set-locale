using System;
using System.Collections.Generic;

using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Helpers;

namespace SetLocale.Client.Web.Services
{
    public interface IDemoDataService
    {
        UserModel GetAUser();

        KeyModel GetAKey();
        List<AppModel> GetUsersApps();
        List<AppModel> GetAllApps();
        List<UserModel> GetAllUsers();
        List<TagModel> GetSomeTag();
        List<LanguageModel> GetSomeLanguage();
        List<KeyModel> GetMyKeys();

        List<KeyModel> GetAllKeys();
        List<KeyModel> GetNotTranslatedKeys();
        TranslationModel GetATranslation();
        AppModel GetAnApp();
    }

    public class DemoDataService : IDemoDataService
    {
        public UserModel GetAUser()
        {
            return new UserModel
             {
                 Language = ConstHelper.en,
                 Id = 1,
                 IsActive = true,
                 Email = "hserdarb@gmail.com",
                 Name = "Serdar Büyüktemiz",
                 RoleName = ConstHelper.User
             };
        }

        public KeyModel GetAKey()
        {
            return new KeyModel
            {
                Key = "btn_save",
                Description = "kaydet butonu için",
                Tags = GetSomeTag(),
                Languages = GetSomeLanguage(),
                IsTranslated = true,
                Translations = new List<TranslationModel>()
                {
                    new TranslationModel
                    {
                        Value = "Kaydet",
                        Language = new LanguageModel
                        {
                            Key = "tr",
                            Name = "Türkçe",
                            ImageUrl = "/public/img/tr.png"
                        }
                    },
                    new TranslationModel
                    {
                        Value = "Save",
                        Language = new LanguageModel
                        {
                            Key = "en",
                            Name = "English",
                            ImageUrl = "/public/img/en.png"
                        }
                    },
                }
            };
        }

        public List<AppModel> GetUsersApps()
        {
            var result = new List<AppModel>();
            result.Add(new AppModel
            {
                Id = 1,
                Email = "setlocale@test.com",
                Name = "SetLocale",
                Description = "a localization management application.",
                Url = "setlocale.com",
                UsageCount = 1356,
                IsActive = true
            });
            result.Add(new AppModel
            {
                Id = 2,
                Email = "setcrm@test.com",
                Name = "SetCrm",
                Description = "a brand new crm application.",
                Url = "setcrm.com",
                UsageCount = 64212,
                IsActive = false
            });

            return result;
        }

        public List<AppModel> GetAllApps()
        {
            var result = GetUsersApps();
            result.Add(new AppModel
            {
                Id = 2,
                Email = "drone@test.com",
                Name = "Marmara Drone",
                Description = "a wireless control dashboard for humanless flying planes.",
                Url = "marmaradrone.github.io",
                UsageCount = 125493,
                IsActive = true
            });

            result.Add(new AppModel
            {
                Id = 2,
                Email = "collade@test.com",
                Name = "Collade",
                Description = "a task management and team collaboration application.",
                Url = "marmaradrone.github.io",
                UsageCount = 9852,
                IsActive = true
            });

            return result;
        }

        public List<UserModel> GetAllUsers()
        {
            var result = new List<UserModel>();
            result.Add(new UserModel
            {
                Id = 1,
                Email = "admin@test.com",
                Name = "Admin",
                RoleName = ConstHelper.Admin,
                IsActive = true
            });
            result.Add(new UserModel
            {
                Id = 2,
                Email = "dev@test.com",
                Name = "Developer",
                RoleName = ConstHelper.Developer,
                IsActive = true
            });
            result.Add(new UserModel
            {
                Id = 3,
                Email = "user@test.com",
                Name = "Translator",
                RoleName = ConstHelper.User,
                IsActive = true
            });
            result.Add(new UserModel
            {
                Id = 4,
                Email = "user2@test.com",
                Name = "Another Translator",
                RoleName = ConstHelper.User,
                IsActive = false
            });

            return result;
        }

        public List<TagModel> GetSomeTag()
        {
            var tags = new List<TagModel>();

            tags.Add(new TagModel
            {
                Name = "SetMembership",
                UrlName = "set-membership"
            });
            tags.Add(new TagModel
            {
                Name = "SetLocale",
                UrlName = "set-locale"
            });
            tags.Add(new TagModel
            {
                Name = "SetCrm",
                UrlName = "set-crm"
            });

            return tags;
        }
        public List<KeyModel> GetMyKeys()
        {
            var result = new List<KeyModel>();
            result.Add(new KeyModel
            {
                Key = "btn_save",
                Description = "kaydet butonu için",
                Tags = GetSomeTag(),
                Languages = GetSomeLanguage(),
                IsTranslated = true
            });

            result.Add(new KeyModel
            {
                Key = "btn_update",
                Description = "güncelle butonu için",
                Tags = GetSomeTag(),
                Languages = GetSomeLanguage(),
                IsTranslated = true
            });

            result.Add(new KeyModel
            {
                Key = "btn_delete",
                Description = "sil butonu için",
                Tags = GetSomeTag(),
                Languages = GetSomeLanguage(),
                IsTranslated = true
            });

            return result;
        }

        public List<KeyModel> GetAllKeys()
        {
            var result = GetMyKeys();
            result.Add(new KeyModel
            {
                Key = "menu_home",
                Description = "menüde anasayfa'ya git düğmesi için",
                Tags = GetSomeTag(),
                Languages = GetSomeLanguage(),
                IsTranslated = true
            });
            result.Add(new KeyModel
            {
                Key = "home_welcome",
                Description = "sisteme hoşgeldiniz mesajı, ana sayfada büyükçe gözüken bir yerde kullanılıyor",
                Tags = GetSomeTag(),
                Languages = GetSomeLanguage(),
                IsTranslated = true
            });

            return result;
        }

        public List<KeyModel> GetNotTranslatedKeys()
        {
            var result = new List<KeyModel>();
            result.Add(new KeyModel
            {
                Key = "btn_search",
                Description = "arama butonu için",
                Tags = GetSomeTag(),
                Languages = GetSomeLanguage(),
                IsTranslated = true
            });

            return result;
        }

        public TranslationModel GetATranslation()
        {
            return new TranslationModel
            {
                Key = "sign_up",
                Value = "Üye Ol",
                Language = new LanguageModel
                {
                    Key = "tr",
                    Name = "Türkçe",
                    ImageUrl = "/public/img/tr.png"
                },
                Tags = GetSomeTag()
            };
        }

        public AppModel GetAnApp()
        {
            return new AppModel
            {
                Id = 1,
                Email = "setlocale@test.com",
                Name = "SetLocale",
                Description = "a localization management application.",
                Url = "setlocale.com",
                UsageCount = 1356,
                IsActive = true,
                Tokens = new List<TokenModel>
                {
                    new TokenModel
                    {
                        CreationDate = DateTime.Now,
                        CreationDateStr = DateTime.Now.ToString("f"),
                        Token = Guid.NewGuid().ToString().Replace("-", ""),
                        UsageCount = new Random().Next(3, 5555)
                    },
                    new TokenModel
                    {
                        CreationDate = DateTime.Now,
                        CreationDateStr = DateTime.Now.ToString("f"),
                        Token = Guid.NewGuid().ToString().Replace("-", ""),
                        UsageCount = new Random().Next(3, 5555)
                    }
                }
            };
        }

        public List<LanguageModel> GetSomeLanguage()
        {
            var result = new List<LanguageModel>();
            result.Add(new LanguageModel
            {
                Key = "tr",
                Name = "Türkçe",
                ImageUrl = "/public/img/tr.png"
            });
            result.Add(new LanguageModel
            {
                Key = "en",
                Name = "English",
                ImageUrl = "/public/img/en.png"
            });

            return result;
        }
    }
}