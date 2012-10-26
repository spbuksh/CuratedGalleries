/// ***********  ADD REFERENCES HERE FOR INTELLISENSE SUPPORTING **********
/// <reference path="~/Scripts/jquery-1.7.1-vsdoc.js" />
/// <reference path="~/Scripts/Corbis/Corbis.Common.js" />
/// ***********************************************************************

function DeleteGallery(id, url) {
    if (!confirm("Do you really want to detele gallery?"))
        return false;

    var onsuccess = function (result) {
        if (result.success)
            $('p.galleryItem[corbis-item-id="' + id + '"]').remove();
    };
    $.ajax({
        url: url,
        type: 'POST',
        data: { id: id },
        success: onsuccess
    });
}
function DeleteGalleryContentImage(id, url) {
    if (!confirm("Do you really want to detele this image?"))
        return;

    var onsuccess = function (result) {
        if (result.success)
            $('p[corbis-item-id="' + id + '"]').remove();
    };
    $.ajax({
        url: url,
        type: 'POST',
        data: { id: id },
        success: onsuccess
    });
}






