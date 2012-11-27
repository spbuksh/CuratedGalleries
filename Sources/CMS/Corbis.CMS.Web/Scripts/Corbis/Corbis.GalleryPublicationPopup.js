/// ***********  ADD REFERENCES HERE FOR INTELLISENSE SUPPORTING **********
/// <reference path="~/Scripts/jquery-1.7.1-vsdoc.js" />
/// <reference path="~/Scripts/jquery-ui-1.8.20.js" />
/// <reference path="~/Scripts/jquery-ui-timepicker-addon.js" />
/// <reference path="~/Scripts/Corbis/Corbis.Common.js" />
/// ***********************************************************************

$(function () {
    var jform = $('div[corbis-popup-id="' + GalleryPublicationPopup.popupID + '"]').find('form');

    var now = new Date();

    var jfrom = jform.find('input[name=From]');
    var jto = jform.find('input[name=To]');

    var fromOptions = {
        dateFormat: 'dd-M-yy',
        timeFormat: 'HH:mm',
        showTimezone: false,
        stepHour: 1,
        stepMinute: 1,
        minDate: now,

        //datepicker trigger
        buttonText: '',
        buttonImageOnly: true,
        buttonImage: GalleryPublicationPopup.calendarIconUrl,
        showOn: 'button'
    };
    //    fromOptions.onClose = function (txtDate, inst) {
    //        if (jto.val() != '') {
    //            var dtfrom = jfrom.datetimepicker('getDate');
    //            var dtto = jto.datetimepicker('getDate');

    //            if (dtfrom > dtto)
    //                jto.datetimepicker('setDate', dtfrom);
    //        }
    //        else {
    //            jto.val(txtDate);
    //        }
    //    };
    fromOptions.onSelect = function (dt) {
        jto.datetimepicker('option', 'minDate', jfrom.datetimepicker('getDate'));
    };

    var toOptions = {
        dateFormat: 'dd-M-yy',
        timeFormat: 'HH:mm',
        showTimezone: false,
        stepHour: 1,
        stepMinute: 1,
        minDate: now,

        //datepicker trigger
        buttonText: '',
        buttonImageOnly: true,
        buttonImage: GalleryPublicationPopup.calendarIconUrl,
        showOn: 'button'
    };
    //    toOptions.onClose = function (txtDate, inst) {
    //        if (jfrom.val() != '') {
    //            var dtfrom = jfrom.datetimepicker('getDate');
    //            var dtto = jto.datetimepicker('getDate');

    //            if (dtfrom > dtto)
    //                jfrom.datetimepicker('setDate', dtto);
    //        }
    //        else {
    //            jfrom.val(txtDate);
    //        }
    //    };
    toOptions.onSelect = function (dt) {
        var value = jfrom.val();
        jfrom.datetimepicker('option', 'maxDate', jto.datetimepicker('getDate'));
        jfrom.val(value);

        if (value != '' && value != null) {
            value = jto.val();
            jto.datetimepicker('option', 'minDate', jfrom.datetimepicker('getDate'));
            jto.val(value);
        }
    };

    jfrom.datetimepicker(fromOptions);
    jto.datetimepicker(toOptions);

    $.validator.unobtrusive.parse(jform);
});

function onPublishSubmit(trigger) {
    var jform = $(trigger).closest('form');

    var jfrom = jform.find('input[name=From]');
    var jto = jform.find('input[name=To]');

    var validator = jform.validate();

    if (!jform.valid()) {
        return;
    }

    var onsuccess = function (result) {
        if (result.success == true) {
            closePopupWindow(GalleryPublicationPopup.popupID);

            if (GalleryPublicationPopup.onPublishSucceeded)
                GalleryPublicationPopup.onPublishSucceeded(result);
        }
        else if (result.success == false) {
            alert(result.error);
        }
        else {
            $('div.publishPeriodContent').parent().html(result);
        }
    }

    GalleryPublicationPopup.params.from = jfrom.val();
    GalleryPublicationPopup.params.to = jto.val();

    $.ajax({
        url: GalleryPublicationPopup.action,
        type: 'POST',
        data: GalleryPublicationPopup.params,
        success: onsuccess
    });
}
function onPublishCancel(trigger, popupID) {
    closePopupWindow(popupID);

    if (GalleryPublicationPopup.onPublishCanceled)
        GalleryPublicationPopup.onPublishCanceled();
}
