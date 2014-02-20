using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

using set.locale.Data.Entities;
using set.locale.Helpers;

namespace set.locale.Data
{
    public class SetDbMigrationConfiguration : DbMigrationsConfiguration<SetDbContext>
    {

        public static string usrId { get; set; }
        public static string appId { get; set; }

        public SetDbMigrationConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(SetDbContext context)
        {
            if (context.Users.Any()) return;

            #region Users

            AddUser(context, ConstHelper.User, "user@test.com", ConstHelper.User);
            AddUser(context, ConstHelper.Translator, "translator@test.com", ConstHelper.Translator);
            AddUser(context, ConstHelper.Admin, "admin@test.com", ConstHelper.Admin);

            #endregion

            #region Apps & Words

            AddApplication(context, "info@set-locale.com", "set-locale", "a brand new crm application.", "set-locale.com");

            AddWord(context, "menu_words", "Kelimeler", "Words", "Слова", "set-locale");
            AddWord(context, "menu_words_words", "Tüm Kelimeler", "All Words", "Все Слова", "set-locale");
            AddWord(context, "menu_words_my_words", "Kelimelerim", "My Words", "Мои Слова", "set-locale");
            AddWord(context, "menu_words_new_word", "Yeni Kelime", "New Word", "Новое Слово", "set-locale");
            AddWord(context, "menu_words_not_translated", "Çevrilmeyen Kelime", "Word Not Translated", "Слово Без Перевода", "set-locale");
            AddWord(context, "menu_apps", "Uygulamalar", "Applications", "Приложения", "set-locale");
            AddWord(context, "menu_apps_apps", "Uygulamalarım", "My Applications", "Мои Приложения", "set-locale");
            AddWord(context, "menu_apps_new_app", "Yeni Uygulama", "New Application", "Новое Приложение", "set-locale");
            AddWord(context, "menu_settings", "Yönetim", "Administration", "Администратор", "set-locale");
            AddWord(context, "menu_settings_apps", "Tüm Uygulamalar", "All Applications", "Все Приложения", "set-locale");
            AddWord(context, "menu_settings_users", "Tüm Kullanıcılar", "All Users", "Все Пользователи", "set-locale");
            AddWord(context, "menu_settings_new_translator", "Yeni Çevirmen", "New Translator", "Новый Переводчик", "set-locale");
            AddWord(context, "menu_user_login", "Giriş", "Login", "Вход", "set-locale");
            AddWord(context, "menu_user_logout", "Çıkış", "Logout", "Выход", "set-locale");
            AddWord(context, "menu_user_sign_up", "Kayıt Ol", "Sign Up", "Войти", "set-locale");
            AddWord(context, "menu_user_reset", "Şifre Sıfırla", "Reset Password", "Сбросить Пароль", "set-locale");
            AddWord(context, "menu_search", "Ara", "Search", "Поиск", "set-locale");

            AddWord(context, "email", "E-posta", "Email", "Почта", "set-locale");
            AddWord(context, "name", "İsim", "Name", "Имя", "set-locale");
            AddWord(context, "app_owner_email", "Uygulama Sahibinin E-Postası", "Owner's E-mail", "Приложение Пользователей Почты", "set-locale");
            AddWord(context, "password", "Şifre", "Password", "Пароль", "set-locale");
            AddWord(context, "app_name", "Uygulama İsmi", "Application Name", "Название Приложения", "set-locale");
            AddWord(context, "description", "Açıklama", "Description", "Описание", "set-locale");
            AddWord(context, "usage_count", "Kullanım Sayısı", "Usage Count", "Количество Использования", "set-locale");
            AddWord(context, "url", "Url", "Url", "Url", "set-locale");
            AddWord(context, "token", "Token", "Token", "Признак", "set-locale");
            AddWord(context, "cretion_date", "Oluşturma Tarihi", "Creation Date", "Дата Создания", "set-locale");
            AddWord(context, "user_role", "Yetki Grubu", "Role", "Роль", "set-locale");
            AddWord(context, "word_key", "Anahtar", "Name", "Ключь", "set-locale");
            AddWord(context, "tag", "Etiket", "Tag", "Тег", "set-locale");
            AddWord(context, "translated_language", "Çevrilmiş Dil", "Translated Language", "Язык Перевода", "set-locale");
            AddWord(context, "translation", "Çeviri", "Translation", "Перевод", "set-locale");
            AddWord(context, "language", "Dil", "Language", "Язык", "set-locale");
            AddWord(context, "forgot_your_password", "Şifremi Unuttum", "Forgot My Password", "Забыл Пароль", "set-locale");
            AddWord(context, "total_page_count", "Toplam Sayfa Sayısı", "Total Page Count", "Всего Страниц", "set-locale");
            AddWord(context, "translator_name", "İsim", "Name", "Название", "set-locale");

            AddWord(context, "btn_login", "Giriş", "Login", "Вход", "set-locale");
            AddWord(context, "btn_sign_up", "Kayıt Ol", "Sign Up", "Выход", "set-locale");
            AddWord(context, "btn_password_reset", "Şifre Sıfırlama Linki Gönder", "Send Password Reset Link", "Отпарвить Ссылку Для Сброса Пароля", "set-locale");
            AddWord(context, "btn_create_new_token", "Yeni Token Oluştur", "Create New Token", "Новый Признак", "set-locale");
            AddWord(context, "btn_new_word", "Yeni Çeviri Ekle", "Add New Translation", "Добавить Перевод", "set-locale");
            AddWord(context, "btn_deactivate", "Pasif", "Deactivate", "Деактивировать", "set-locale");
            AddWord(context, "btn_activate", "Aktif", "Activate", "Активировать", "set-locale");
            AddWord(context, "btn_delete", "Sil", "Delete", "Удалить", "set-locale");
            AddWord(context, "btn_save", "Kaydet", "Save", "Сохранить", "set-locale");
            AddWord(context, "btn_edit", "Düzenle", "Edit", "Редактировать", "set-locale");
            AddWord(context, "btn_cancel", "İptal", "Cancel", "Отменить", "set-locale");
            AddWord(context, "btn_ok", "Tamam", "Ok", "Да", "set-locale");
            AddWord(context, "btn_export_to_excel", "Excel", "Excel", "Excel", "set-locale");
            AddWord(context, "column_header_translation_tr", "Tükçe", "Turkish", "Турецкий", "set-locale");
            AddWord(context, "column_header_translation_az", "Azerbeycan", "Azerbaijan", "Азербайджанский", "set-locale");
            AddWord(context, "column_header_translation_cn", "Çince", "Chinese", "Китайский", "set-locale");
            AddWord(context, "column_header_translation_fr", "Fransızca", "Français", "Французский", "set-locale");
            AddWord(context, "column_header_translation_gr", "Yunanca", "Greek", "Греческий", "set-locale");
            AddWord(context, "column_header_translation_it", "İtalyanca", "İtaliano", "Итальянский", "set-locale");
            AddWord(context, "column_header_translation_kz", "Kazakça", "Kazakh", "Казахский", "set-locale");
            AddWord(context, "column_header_translation_ru", "Rusça", "Russian", "Русский", "set-locale");
            AddWord(context, "column_header_translation_sp", "İspanyolca", "Espanol", "Испанский", "set-locale");
            AddWord(context, "column_header_translation_tk", "Türkmençe", "Turkic", "Турецкий", "set-locale");

            AddWord(context, "home_title", "Set-locale Servisine Hoş Geldiniz.", "Welcome To Set-locale.", "Добро Пожаловать В Set-locale.", "set-locale");
            AddWord(context, "words_key_listing_title", "Anahtar Listesi", "Name Listing", "Список Ключей", "set-locale");
            AddWord(context, "login_view_title", "Sisteme Giriş", "İnto The System", "Вход В Систему", "set-locale");
            AddWord(context, "new_user_title", "Yeni Kullanıcı", "New User", "Новый пользователь", "set-locale");
            AddWord(context, "password_reset_title", "Şifre Sıfırla", "Reset Password", "Сбросить Пароль", "set-locale");
            AddWord(context, "user_apps_title", "Uygulamalarım", "My Applications", "Мои приложения", "set-locale");
            AddWord(context, "all_apps_title", "Tüm Uygulamalar", "All Applications", "Все приложения", "set-locale");
            AddWord(context, "all_users_title", "Tüm Kullanıcılar", "All Users", "Все пользователи", "set-locale");
            AddWord(context, "word_new_key_title", "Yeni Anahtar", "New Name", "Новый Ключь", "set-locale");
            AddWord(context, "menu_words_my_words_key_listing_title", "Anahtar Listesi", "My Name Listing", "Мой Список Ключей", "set-locale");
            AddWord(context, "new_app_title", "Yeni Uygulama", "New Application", "Новое Приложение", "set-locale");
            AddWord(context, "new_translator_title", "Yeni Çevirmen", "New Translator", "Новый Переводчик", "set-locale");
            AddWord(context, "tag_keys_title", "Tag Anahtar Listesi", "Tag Keys Listing", "Тег Списка Ключей", "set-locale");

            AddWord(context, "modal_title_apps", "Uygulama Durumu", "Application Status", "Статус Приложения", "set-locale");
            AddWord(context, "modal_body_apps", "Değiştirmek İstediğinize Emin misiniz?", "Are You Sure Want To Change The Application's Status?", "Вы Уверены Что Хотите Изменить Статус Приложения?", "set-locale");
            AddWord(context, "modal_title_delete_token", "Token Sil", "Delete Token", "Удалить Признак", "set-locale");
            AddWord(context, "modal_body_delete_token", "Bu Anahtarı Silmek İstediğinize Emin misiniz?", "Are You Sure You Want To Delete This Token?", "Вы Уверены Что Хотите Удалить Этот Признак?", "set-locale");
            AddWord(context, "modal_title_users", "Kullanıcı Durumu", "User Status", "Статус Пользователя", "set-locale");
            AddWord(context, "modal_body_users", "Durumu Değiştirmek İstediğinize Emin misiniz?", "Are You Sure You Want To Change The User's Status?", "Вы Уверены Что Хотите Изменить Статус Пользователя?", "set-locale");

            AddApplication(context, "info@set-web.com", "set-web", "a brand new crm application.", "set-web.com");

            AddWord(context, "admin", "", "", "", "set-web");
            AddWord(context, "all_users_title", "", "", "", "set-web");
            AddWord(context, "btn_activate", "", "", "", "set-web");
            AddWord(context, "btn_cancel", "", "", "", "set-web");
            AddWord(context, "btn_deactivate", "", "", "", "set-web");
            AddWord(context, "btn_login", "", "", "", "set-web");
            AddWord(context, "btn_ok", "", "", "", "set-web");
            AddWord(context, "btn_open_feedback_popup", "", "", "", "set-web");
            AddWord(context, "btn_save", "", "", "", "set-web");
            AddWord(context, "btn_save_and_new", "", "", "", "set-web");
            AddWord(context, "btn_send", "", "", "", "set-web");
            AddWord(context, "btn_send_password_reset_link", "", "", "", "set-web");
            AddWord(context, "btn_sign_up", "", "", "", "set-web");
            AddWord(context, "contact_view_title", "", "", "", "set-web");
            AddWord(context, "contactmessages_listing_title", "", "", "", "set-web");
            AddWord(context, "domainobject_detail_view_title", "", "", "", "set-web");
            AddWord(context, "domainobject_listing_title", "", "", "", "set-web");
            AddWord(context, "email", "", "", "", "set-web");
            AddWord(context, "feedback_popup_title", "", "", "", "set-web");
            AddWord(context, "feedbacks_listing_title", "", "", "", "set-web");
            AddWord(context, "forgot_your_password", "", "", "", "set-web");
            AddWord(context, "login_view_title", "", "", "", "set-web");
            AddWord(context, "menu_contact", "", "", "", "set-web");
            AddWord(context, "menu_contactmessages", "", "", "", "set-web");
            AddWord(context, "menu_domain_object", "", "", "", "set-web");
            AddWord(context, "menu_domain_object_list", "", "", "", "set-web");
            AddWord(context, "menu_domain_object_new", "", "", "", "set-web");
            AddWord(context, "menu_feedbacks", "", "", "", "set-web");
            AddWord(context, "menu_login", "", "", "", "set-web");
            AddWord(context, "menu_signup", "", "", "", "set-web");
            AddWord(context, "menu_user_logout", "", "", "", "set-web");
            AddWord(context, "menu_user_profile", "", "", "", "set-web");
            AddWord(context, "menu_users", "", "", "", "set-web");
            AddWord(context, "message", "", "", "", "set-web");
            AddWord(context, "modal_body_users", "", "", "", "set-web");
            AddWord(context, "modal_title_users", "", "", "", "set-web");
            AddWord(context, "name", "", "", "", "set-web");
            AddWord(context, "new_domainobject_view_title", "", "", "", "set-web");
            AddWord(context, "new_password", "", "", "", "set-web");
            AddWord(context, "new_user_title", "", "", "", "set-web");
            AddWord(context, "nothing_found", "", "", "", "set-web");
            AddWord(context, "password", "", "", "", "set-web");
            AddWord(context, "password_change_title", "", "", "", "set-web");
            AddWord(context, "password_reset_title", "", "", "", "set-web");
            AddWord(context, "please_wait", "", "", "", "set-web");
            AddWord(context, "search", "", "", "", "set-web");
            AddWord(context, "subject", "", "", "", "set-web");
            AddWord(context, "total_page_count", "", "", "", "set-web");
            AddWord(context, "user_role", "", "", "", "set-web");
            #endregion

            context.SaveChanges();
        }

        private static void AddUser(SetDbContext context, string name, string email, string role)
        {
            usrId = Guid.NewGuid().ToNoDashString();
            var user = new User
            {
                Id = usrId,
                Email = email,
                Name = name,
                RoleId = ConstHelper.BasicRoles[role],
                RoleName = role,
                ImageUrl = email.ToGravatar(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"),
                LastLoginAt = DateTime.Now,
                IsActive = true,
                Language = ConstHelper.CultureNameEN
            };
            context.Users.Add(user);
        }

        private static void AddApplication(SetDbContext context, string email, string name, string description, string url)
        {
            appId = Guid.NewGuid().ToNoDashString();
            var app = new App
            {
                Id = appId,
                UserEmail = email,
                Name = name,
                Description = description,
                Url = url,
                CreatedBy = usrId,
                Tokens = new List<Token>
                {
                    new Token
                    {
                        Key = Guid.NewGuid().ToNoDashString(),
                        UsageCount = 0,
                        CreatedBy = usrId,
                        IsAppActive = true
                    }
                },
                IsActive = true
            };

            context.Apps.Add(app);
        }

        private static void AddWord(SetDbContext context, string key, string tr, string en, string ru, string tagName)
        {
            var word = new Word
            {
                Key = key,
                IsTranslated = true,
                Translation_EN = en,
                Translation_TR = tr,
                Translation_RU = ru,
                CreatedBy = usrId,
                TranslationCount = 3,
                Tags = new List<Tag>
                {
                    new Tag
                    {
                        Name = tagName,
                        UrlName = tagName.ToUrlSlug(),
                        CreatedBy = usrId,
                        AppId = appId
                    }
                },
                AppId = appId
            };
            context.Words.Add(word);
        }
    }
}