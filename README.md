set-locale
==========

set-locale is a localization string provider as a service...

## api request examples

### /api/locales?lang=tr
{ ... }

### /api/locales?lang=tr&tag=membership
{ key:"first_name",
  value: "Adınız" },
{ key:"last_name",
  value: "Soyadınız" },
{ key:"email",
  value: "E-posta Adresiniz" }

### /api/locale?lang=tr&key=first_name
{ key:"first_name",
  value: "Adınız" }


## who can use this

when you want to build a multilangual app and don't want to translate your strings you can use set-locale service.


## what does set stand for

"set" is argeset's products prefix,
something like apple's "i"

you can sign up from the web site 
and create an app in set-locale
then you can request data for your app with the token

