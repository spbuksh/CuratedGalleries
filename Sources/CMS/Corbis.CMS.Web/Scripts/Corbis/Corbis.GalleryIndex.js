
/// ***********  ADD REFERENCES HERE FOR INTELLISENSE SUPPORTING **********
/// <reference path="~/Scripts/jquery-1.7.1-vsdoc.js" />
/// <reference path="~/Scripts/Corbis/Corbis.Common.js" />
/// ***********************************************************************

function DeleteGallery(id, url) {
    if (!confirm("Do you really want to detele gallery?"))
        return false;

    var onsuccess = function (result) {
        if (result.success)
            $('div.galleryItem[corbis-item-id="' + id + '"]').remove();
    };
    $.ajax({
        url: url,
        type: 'POST',
        data: { id: id },
        success: onsuccess
    });
}


$(function () {
    $('div.galleryItem[corbis-item-id]')
});

function onLockGalleryClick(lockUrl, galleryID, link) {
    var jlink = $(link);

    if (jlink.is('[corbis-exec-submit]'))
        return true;

    var onsuccess = function (result) {
        if (result.success) {
            jlink.attr('corbis-exec-submit', '1');
            window.location = jlink.attr('href');
        }
        else {
            alert('Gallery can not be locked');
            //$('div.galleryItem[corbis-item-id="' + galleryID + '"]').replaceWith(result);
        }
    };
    $.ajax({
        url: lockUrl,
        type: 'POST',
        data: { id: galleryID },
        success: onsuccess
    });

    return false;
}
