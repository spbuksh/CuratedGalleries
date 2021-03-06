﻿/// ***********  ADD REFERENCES HERE FOR INTELLISENSE SUPPORTING **********
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
validationSummaryHelper.prototype.reset = function (inputSelector) {
    var jelem = $(this._selector);
    if (jelem.length == 0) return;
    var jform = jelem.closest('form');
    if (jform.length == 0) return;
    jform.data("validator").resetForm();
    jform.find(".validation-summary-errors")
            .addClass("validation-summary-valid")
            .removeClass("validation-summary-errors");
    jform.find(".field-validation-error")
            .addClass("field-validation-valid")
            .removeClass("field-validation-error")
            .removeData("unobtrusiveContainer")
            .find(">*")
            .removeData("unobtrusiveContainer");
}
validationSummaryHelper.prototype.showErrors = function (errors, inputSelector) {
    var jform = $(this._selector).closest('form');
    var container = jform.find("[data-valmsg-summary=true]"), list = container.find("ul");
    list.empty();
    container.addClass("validation-summary-errors").removeClass("validation-summary-valid");
    Array.forEach(errors, function (item, index, array) {
        $("<li />").html(item).appendTo(list);
    });
};


function onBeginMsAjaxRequest() {
    beginWaitCursor();
}
function onCompleteMsAjaxRequest() {
    endWaitCursor();
}


//Client Logger
var clientLogger =
{
    initialize: function (options) {
        clientLogger._options = options;

        if (clientLogger._options.verb == null || clientLogger._options.verb == undefined)
            clientLogger._options.verb = 'POST';

        if (clientLogger._options.showErrorAlerts == null || clientLogger._options.showErrorAlerts == undefined)
            clientLogger._options.showErrorAlerts = false;

        window.onerror = function (msg, url, line) {
            clientLogger.error(msg, url, line);
        };
    },
    error: function (error, url, line) {
        if (clientLogger._options.showErrorAlerts) {
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
        clientLogger._log({ entrytype: 'error', msg: error, url: url, line: line });
    },
    warn: function (warning, url, line) {
        clientLogger._log({ entrytype: 'warning', text: warning, url: url, line: line });
    },
    info: function (info, url, line) {
        clientLogger._log({ entrytype: 'info', text: info, url: url, line: line });
    },

    _log: function (logEntry) {
        var onsuccess = function (result) {
            if (result != undefined) {
                if (result.success == undefined || result.success == false) {
                    alert('Error occured during client logging');
                }
            }
        };
        var onerror = function (pmtr1, pmtr2, pmtr3) {
            alert('error');
        };
        $.ajax({
            url: clientLogger._options.url,
            type: clientLogger._options.verb,
            data: logEntry,
            success: onsuccess,
            error: onerror
        });
    }
};





/* Popup Window */
// popupID - identifier of html element
// popupClass - css class for popup (will be removed to HTML attributes collection)
// action - Server URL (relative)
// params - parameters for server request
// popupOptions - see jQuery dialog options
// type - 'GET' or 'POST'
// onPopupLoadFunc - function that is called after the popup window has been loaded.
// onResultHandler - function with 1 parameter which has properties: 
//      'ShowDialog' - show or not dialog, 
//      'Data' - result of asynch operation. It can be or popup html or custom json data
// onUnblockFunc - function that is called after the popup window has loaded and the blocking panel has disappered

//var options = {
//    title: 'Dialog'
//    //-------- popup content -------
//    url: 'This url is used to get popup content',
//    verb: 'post || get (is used to load popup content)', //by default GET is used
//    params: {}, //parameters for popup content request
//    //-------- popup div element options ----------
//    popupID: 'Popup identifier string. It is value for "corbis-popup-id" attribute',
//    classes: ['class1', 'class2'],
//    //-------- jquery dialog options
//    jdialogOptions: null,

//    onPopupElementCreated: function (jdiv) { /* here you can set attributes or some else data for popup root element */ },
//    //popup element has been created and popup content was loaded but popup is not shown
//    onLoading: function (data) { /* data is object like { showDialog: true, html: 'html content of the popup'}. You can cancel of dialog displaying */ },
//    onLoaded: function () { /* this method is invoked on $(document).ready(). You here can set focus or something else */ },
//    onCreated: function(event, ui) { },
//    onOpened: function (event, ui) { },
//    onClosing: function(event, ui) { },
//    onClosed: function(event, ui) { }
//};
function showPopupWindow(options) {
    if (!options.url || options.url == '')
        throw 'Popup content url is required';

    if (!options.popupID || options.popupID == '')
        throw 'Popup identifier is required';

    beginWaitCursor();

    try {
        var jdiv = $(document.createElement("div"));

        jdiv.attr('title', options.title);

        if (options.popupID) {
            jdiv.attr('corbis-popup-id', options.popupID);
            if (!options.params) options.params = {};
            options.params.popupID = options.popupID;
        }

        //set classes for popup
        if (options.classes && options.classes.length != 0) {
            $.each(options.classes, function (i, item) {
                jdiv.addClass(item);
            });
        }

        //mark this div as popup with specific attribute
        jdiv.attr('corbis-popup', 'true');

        jdiv.css("display", "none");
        $(document.body).append(jdiv.get(0));

        if (options.onPopupCreating)
            options.onPopupCreating(jdiv);

        var onSuccess = function (htmlText) {
            try {
                if (options.onLoading) {
                    var resultWrapper = { showDialog: true, html: htmlText };
                    options.onLoading(resultWrapper);
                    if (!resultWrapper.showDialog) return;
                }

                jdiv.html('');
                jdiv.html(htmlText);
                $(document.body).append(jdiv.get(0));
                jdiv.css("display", "block");

                var jdialogOptions = options.jdialogOptions;

                //see http://api.jqueryui.com/dialog/
                var jdialog = jdiv.dialog({
                    modal: (jdialogOptions && jdialogOptions.modal != null && jdialogOptions.modal != undefined) ? jdialogOptions.modal : true,
                    resizable: (jdialogOptions && jdialogOptions.resizable != null && jdialogOptions.resizable != undefined) ? jdialogOptions.resizable : false,
                    draggable: false,
                    closeOnEscape: (jdialogOptions && jdialogOptions.closeOnEscape != null && jdialogOptions.closeOnEscape != undefined) ? jdialogOptions.closeOnEscape : true,
                    dialogClass: (jdialogOptions && jdialogOptions.dialogClass != null && jdialogOptions.dialogClass != undefined) ? jdialogOptions.dialogClass : '',
                    height: (jdialogOptions && jdialogOptions.height != null && jdialogOptions.height != undefined) ? jdialogOptions.height : 'auto',
                    width: (jdialogOptions && jdialogOptions.width != null && jdialogOptions.width != undefined) ? jdialogOptions.width : 400,
                    beforeClose: options.onClosing,
                    close: options.onClosed,
                    create: options.onCreated,
                    open: options.onOpened
                });
                jdiv.parent().draggable({ handle: "div.ui-dialog-titlebar" });

                $(function () {
                    $.validator.unobtrusive.parse(jdiv);

                    if (options.onLoaded)
                        options.onLoaded({ popup: jdiv });
                });
            }
            catch (ex) {
                alert(ex);
            }

            endWaitCursor();
        };

        var onFailure = function (request, status, error) {
            try {
                jdiv.remove();
                alert("ERROR\r\nStatus: " + request.status + "\r\nMessage: " + error);
            }
            catch (ex) {
                alert(ex);
            }

            endWaitCursor();
        };

        $.ajax({
            url: options.url,
            type: options.verb ? options.verb : "GET",
            data: options.params,
            cache: false,
            success: onSuccess,
            beforeSend: null,
            error: onFailure
        });
    }
    catch (exep) {
        window.showExceptionAlert(exep);
    }
    finally {
        endWaitCursor();
    }
}

function closePopupWindow(popupID) {
    var jpopup = $('div[corbis-popup-id="' + popupID + '"]');
    jpopup.dialog("close");
    jpopup.remove();
}
