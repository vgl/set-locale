var textBtnDanger = "btn-danger";
var textBtnSuccess = "btn-success";
var textId = "id";
var textIsActive = "isactive";

var allLanguages = [{ id: 'tr', text: 'Türkçe' },
    { id: 'en', text: 'English' },
    { id: 'sp', text: 'Español' },
    { id: 'cn', text: '中文 (zhōngwén)' },
    { id: 'ru', text: 'Русский язык' },
    { id: 'fr', text: 'Français' },
    { id: 'gr', text: 'Deutsch' },
    { id: 'it', text: 'Italiano' },
    { id: 'az', text: 'Azərbaycan dili' },
    { id: 'tk', text: 'түркmенче (türkmençe)' },
    { id: 'kz', text: 'Қазақ тілі' }];

$(function () {
    $("a.btnAction").click(function() {
        var textBtn = "input#btnModalAction";
        
        var id = $(this).data(textId);
        var isActive = $(this).data(textIsActive);
        
        $(textBtn).removeClass(textBtnDanger).removeClass(textBtnSuccess);
        if (isActive == "True") {
            $(textBtn).addClass(textBtnDanger);
        } else {
            $(textBtn).addClass(textBtnSuccess);
        }

        $(textBtn).data(textId, id).data(textIsActive, isActive);
    });
});