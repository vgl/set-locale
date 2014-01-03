var textBtnDanger = "btn-danger";
var textBtnSuccess = "btn-success";
var textId = "id";
var textIsActive = "isactive";

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