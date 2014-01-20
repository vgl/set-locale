set-locale
==========

set-locale is a localization string provider as a service...

## api request examples

### http://setlocale.azurewebsites.net/api/locales/tr/1
### http://setlocale.azurewebsites.net/api/locales/tr/set-locale/2

{
    "Key": "btn_activate",
    "Value": "Aktif"
},
{
    "Key": "btn_deactivate",
    "Value": "Pasif"
},
{
    "Key": "btn_new_word",
    "Value": "Yeni Çeviri Ekle"
},
{
    "Key": "btn_create_new_token",
    "Value": "Yeni Token Oluştur"
},
{
    "Key": "btn_password_reset",
    "Value": "Şifre Sıfırlama Linki Gönder"
},
{
    "Key": "btn_sign_up",
    "Value": "Kayıt Ol"
},
{
    "Key": "btn_login",
    "Value": "Giriş"
}


### http://setlocale.azurewebsites.net/api/locale/tr/save_and_close

{
    "Key": "save_and_close",
    "Lang": "tr",
    "Value": "Kaydet & Kapat",
    "Msg": null
}


## who can use this

when you want to build a multilangual app and don't want to translate your strings you can use set-locale service.


## what does set stand for

"set" is argeset's products prefix,
something like apple's "i"

you can sign up from the web site 
and create an app in set-locale
then you can request data for your app with the token
it is now in alpha tests, and we are open to your feedbacks...

http://setlocale.azurewebsites.net
