/// ***********  ADD REFERENCES HERE FOR INTELLISENSE SUPPORTING **********
/// <reference path="~/Scripts/jquery-1.7.1-vsdoc.js" />
/// <reference path="~/Scripts/jquery-ui-1.8.20.js" />
/// <reference path="~/Scripts/Corbis/Corbis.Common.js" />
/// ***********************************************************************

var CorbisUserMngr =
{
    delAction: null,
    activationAction: null,
    changePasswordAction: null
};

$(function () {
    $('table.corbis-users input[type=checkbox]').click(function () {
        var jthis = $(this);

        var attribute = 'corbis-post-canceled';

        if (jthis.attr(attribute) != undefined) {
            jthis.removeAttr(attribute);
            return;
        }

        var isActive = jthis.is(':checked');

        var onSuccess = function (result) {
            if (result.success == true) {
                //do nothing of show notification about success
            }
            else if (result.success == false) {
                alert(result.error);
                jthis.attr(attribute, '1');
                jthis.trigger('click');
            }
            else {
                throw 'Not implemented case';
            }
        };
        var onError = function () {
            jthis.attr(attribute, '1');
            jthis.trigger('click');
        }

        $.ajax({
            type: 'post',
            url: CorbisUserMngr.activationAction,
            data: { id: jthis.closest('tr').attr('corbis-item-id'), isActive: isActive },
            success: onSuccess,
            error: onError
        });
    });
    $('table.corbis-users select[name=roles]').change(function () {
        var jthis = $(this);
        $.ajax({
            type: 'post',
            url: CorbisUserMngr.changeRoleAction,
            data: { id: jthis.closest('tr').attr('corbis-item-id'), roles: jthis.val() },
        });

    });
});
function showResetPasswordPopup(userID) {
    showPopupWindow({
        popupID: 'changePasswordPopup',
        title: 'Change Password',
        url: CorbisUserMngr.changePasswordAction,
        params: { userID: userID }
    });
}
function deleteUser(user, trigger) {
    if (!confirm("Do you really want to delete '" + user.login + "'?")) return;
    $.ajax({
        url: CorbisUserMngr.delAction,
        type: 'POST',
        data: { id: user.id },
        success: function (result) {
            if (result.success) {
                $(trigger).closest('tr').remove();
            }
            else {
                alert('Error');
            }
        }
    });
}
