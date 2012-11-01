/// ***********  ADD REFERENCES HERE FOR INTELLISENSE SUPPORTING **********
/// <reference path="~/Scripts/jquery-1.7.1-vsdoc.js" />
/// ***********************************************************************

//------- jQuery Extensions --------------------
jQuery.fn.swapWith = function (to) {
    return this.each(function () {
        var copy_to = $(to).clone(true);
        var copy_from = $(this).clone(true);
        $(to).replaceWith(copy_from);
        $(this).replaceWith(copy_to);
    });
};
//----------------------------------------------

//---- ONLOAD
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

//
function beginWaitCursor(jload) {
    if (!jload) jload = $('#loading');
    jload.show();
    if ($.blockUI) $.blockUI({ css: { backgroundColor: '#f00', color: '#fff'} });
}
//
function endWaitCursor(jload) {
    if (!jload) jload = $('#loading');
    jload.hide();
    if ($.blockUI) $.unblockUI();
}


var validationSummaryHelper = function (selector) {
    this._selector = selector;
}
validationSummaryHelper.prototype.reset = function reset() { 
    var jelem = $(this._selector);

    var jul = jelem.children('ul').html('');
    $(jul).append('<li style="display:none"></li>');

    jelem.removeClass('validation-summary-errors');
    jelem.addClass('validation-summary-valid');
}


function onBeginMsAjaxRequest() {
    beginWaitCursor();
}
function onCompleteMsAjaxRequest() {
    endWaitCursor();
}




