/// ***********  ADD REFERENCES HERE FOR INTELLISENSE SUPPORTING **********
/// <reference path="~/Scripts/jquery-1.7.1-vsdoc.js" />
/// <reference path="~/Scripts/jquery.validate.unobtrusive.js" />
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

function registerFormsForUnobtrusiveValidation(selector) {
    $.validator.unobtrusive.parse(selector);
}

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



//Client Logger
function clientLogger(options) {
    this._options = options;

    if (this._options.verb == null || this._options.verb == undefined)
        this._options.verb = 'POST';

    if (this._options.showErrorAlerts == null || this._options.showErrorAlerts == undefined)
        this._options.showErrorAlerts = false;

    window.onerror = function (msg, url, line) {
        this._error(msg, url, line);
    };
};
clientLogger.prototype._log = function (logEntry) {
    $.ajax({
        url: this._options.url,
        type: this._options.verb,
        data: logEntry
    });
}
clientLogger.prototype.error = function (error, url, line) {
    if (this._options.showErrorAlerts) {
        var errorMsg = '[Error Text]:' + error;

        if (url != null && url != undefined) {
            errorMsg += '\r\n\r\n';
            errorMsg += '[URL]:' + url;
        }
        if (line != null && line != undefined) {
            errorMsg += '\r\n\r\n';
            errorMsg += '[Line]: ' + line.toString();
        }

        alert(errorMsg);
    }
    this._log({ etype: 'error', msg: error, url: url, line: line });
};
clientLogger.prototype.warn = function (warning, url, line) {
    this._log({ entrytype: 'warning', text: warning, url: url, line: line });
};
clientLogger.prototype.info = function (info, url, line) {
    this._log({ entrytype: 'info', text: info, url: url, line: line });
};


