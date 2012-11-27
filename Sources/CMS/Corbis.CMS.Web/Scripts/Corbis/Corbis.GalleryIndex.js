
/// ***********  ADD REFERENCES HERE FOR INTELLISENSE SUPPORTING **********
/// <reference path="~/Scripts/jquery-1.7.1-vsdoc.js" />
/// <reference path="~/Scripts/Corbis/Corbis.Common.js" />
/// ***********************************************************************
var GalleryIndexPage = {
    publishUrl: null
};
function DeleteGallery(id, url) {
    if (!confirm("Do you really want to detele gallery?"))
        return false;

    var onsuccess = function (result) {
        if (result.success)
            $('div.galleryItem[corbis-item-id="' + id + '"]').parent().remove();
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

function publishGallery(id) {
    showPopupWindow({
        popupID: 'publishGalleryPopup',
        title: 'Publish Period',
        url: GalleryIndexPage.publishUrl,
        params: { galleryID: id },
        onLoaded: function () {
            GalleryPublicationPopup.onPublishSucceeded = function (result) {
                if (result.success == true) {
                    var onSuccess = function (data) {
                        $('div.galleryItem[corbis-item-id="' + id + '"]').parent().replaceWith(data);
                    };
                    $.ajax({
                        url: GalleryIndexPage.getGalleryItemUrl,
                        type: "POST",
                        data: { id: id },
                        success: onSuccess
                    });
                }
                else if (result.success == false) {
                    alert(result.error);
                }
                else {
                    throw 'Unexpected ajax action result';
                }
            };
        }
    });
}
function unPublishGallery(id) {
    if (!confirm('Do you really want to un-publish the gallery?'))
        return;

    var onUnpublishSuccess = function (result) {
        if (result.success == true) {
            var onSuccess = function (data) {
                $('div.galleryItem[corbis-item-id="' + id + '"]').parent().replaceWith(data);
            };

            $.ajax({
                url: GalleryIndexPage.getGalleryItemUrl,
                type: "POST",
                data: { id: id },
                success: onSuccess
            });
        }
        else {
            alert('Error');
        }
    }

    $.ajax({
        url: GalleryIndexPage.unpublishUrl,
        type: "POST",
        data: { id: id },
        success: onUnpublishSuccess
    });
}


