/// ***********  ADD REFERENCES HERE FOR INTELLISENSE SUPPORTING **********
/// <reference path="~/Scripts/jquery-1.7.1-vsdoc.js" />
/// ***********************************************************************

function beginWaitCursor(jload) {
    if (!jload) jload = $('#loading');
    jload.show();
    if ($.blockUI) $.blockUI({ css: { backgroundColor: '#f00', color: '#fff'} });
}
function endWaitCursor(jload) {
    if (!jload) jload = $('#loading');
    jload.hide();
    if ($.blockUI) $.unblockUI();
}

$(function () {
    var jload = $('#loading');

    jload.ajaxStart(function () {
        beginWaitCursor(jload);
    });
    jload.ajaxComplete(function () {
        endWaitCursor(jload);
    });
    jload.ajaxError(function (request, status, error) {
        alert('Error');
    });
});