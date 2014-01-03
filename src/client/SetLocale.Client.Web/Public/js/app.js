$(function() {
    $("a.btnAction").click(function() {
        var textBtn = "input#btnModalAction";
        var textBtnDanger = "btn-danger";
        var textBtnSuccess = "btn-success";
        var textId = "id";
        var textIsActive = "isactive";

        var id = $(this).data(textId);
        var isActive = $(this).data(textIsActive);
        
        $(textBtn).removeClass(textBtnDanger).removeClass(textBtnSuccess);
        console.log($(textBtn));
        if (isActive == "True") {
            $(textBtn).addClass(textBtnDanger);
        } else {
            $(textBtn).addClass(textBtnSuccess);
        }

        $(textBtn).data(textId, id).data(textIsActive, isActive);
    });
});