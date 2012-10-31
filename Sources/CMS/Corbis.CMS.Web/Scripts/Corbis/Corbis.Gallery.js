/// ***********  ADD REFERENCES HERE FOR INTELLISENSE SUPPORTING **********
/// <reference path="~/Scripts/jquery-1.7.1-vsdoc.js" />
/// <reference path="~/Scripts/Corbis/Corbis.Common.js" />
/// ***********************************************************************


var GalleryPageMngr =
{
    galleryID: null
}

$(function () {
    var jimageRoots = $('div.contentImage');

    //****** it activates/deactivates image text input data
    var handler = function (jradio) {
        jradio.closest('div.txtArea').find('div.' + jradio.attr('corbis-txt-type')).show().siblings().hide();
    };

    var jradios = jimageRoots.find('div.radioGroup input:radio');

    jradios.live('click', function () {
        handler($(this));
    });

    handler(jradios.filter(':checked'));

    $('div.contentImage div.txtAlign a').live('click', function () {
        var jthis = $(this);
        _setTextPosition(jthis.closest('div.contentImage'), null, jthis.closest('li'));
    });
    //******* image toolbar handling
    //delete image
    $('div.contentImage span.toolbar a.close').live('click', function () {
        DeleteGalleryContentImage($(this).closest('div.contentImage'));
    });
    $('div.contentImage span.toolbar a.minimal').live('click', function () {
        var parent = $(this).closest('div.contentImage');
        var jcol = parent.find('div.collapsed');
        var jexp = $(jcol).siblings('div.expanded');
        if (jcol.is(':visible')) {
            jcol.hide();
            jexp.show();
        }
        else {
            jcol.show();
            jexp.hide();
        }
    });

    //****** move images up/down
    //Move image up
    $('div.contentImage span.up_down a.up').live('click', function () {
        var jcurImg = $(this).closest('div.contentImage');

        var jimg = jcurImg.prev('div.contentImage');

        if (jimg.length == 0)
            return;

        _swapImages(jcurImg, jimg);
    });

    //Move image down
    $('div.contentImage span.up_down a.down').live('click', function () {
        var jcurImg = $(this).closest('div.contentImage');

        var jimg = jcurImg.next('div.contentImage');

        if (jimg.length == 0)
            return;

        _swapImages(jcurImg, jimg);
    });

    var _swapImages = function (jimag1, jimag2) {
        var onsuccess = function (result) {
            if (result.success) {
                jimag1.swapWith(jimag2);
            }
            else {
                alert(result.error ? result.error : 'Error. Please update the page');
            }
        };
        $.ajax({
            url: GalleryPageMngr.swapImageOrderURL,
            type: 'POST',
            data: { galleryID: GalleryPageMngr.galleryID, imageID1: jimag1.attr('corbis-item-id'), imageID2: jimag2.attr('corbis-item-id') },
            success: onsuccess
        });
    };

});

function ExpandImagesAll() {
    $('div.contentImage div.collapsed').hide();
    $('div.contentImage div.expanded').show();
}
function CollapseImagesAll() {
    $('div.contentImage div.collapsed').show();
    $('div.contentImage div.expanded').hide();
}

function _getContentImageJElem(imageID) {
    return $('div.contentImage[corbis-item-id="' + imageID + '"]');
}
function _setTextPosition(jroot, position, jli) {
    jroot.find('div.txtArea input[name=position]').val(position);
    if (position && !jli) {
        jli = jroot.find('div.txtAlign li[corbis-data-position="' + position + '"]');
        jroot.find('div.txtArea input[name=position]').val(position);
    } else if (!position && jli) {
        jroot.find('div.txtArea input[name=position]').val(jli.attr('corbis-data-position'));
    }
    else { 
        throw '';
    }
    jli.addClass('active');
    jli.siblings('li').removeClass('active');
}
function _onTextContentSaveSuccess(data, selector) {
    if (data.success) {
        alert('Image updated successfully');

        var jvs = $(selector).closest('form').find('div.validation-summary-errors');

        if (jvs.length != 0) {
            var helper = new validationSummaryHelper(jvs);
            helper.reset();
        }
    }
    else {
        var jelem = $(selector);
        jelem.html('');
        jelem.html(data);
    }
}

function InitContentImage(options) {
    var jroot = _getContentImageJElem(options.imageID);

    if (options.position)
        _setTextPosition(jroot, options.position, null);

    if (options.txtMode) {
        $(jroot).find('div.radioGroup input[corbis-txt-type="' + options.txtMode + '"]').trigger('click');
    }
}

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
function ClearGalleryContent(id, url) {
    if (!confirm("Do you really want to detele all gallery images?"))
        return;

    var onsuccess = function (result) {
        if (result.success)
            $('div.galleryContent').html('');
    };
    $.ajax({
        url: url,
        type: 'POST',
        data: { id: id },
        success: onsuccess
    });
}

function DeleteGalleryContentImage(data) {
    var jimage = (typeof data == "string") ? _getContentImageJElem(data) : data;

    if (!confirm("Do you really want to detele this image?"))
        return;

    var onsuccess = function (result) {
        if (result.success)
            jimage.remove();
    };
    $.ajax({
        url: GalleryPageMngr.deleteContentImageURL,
        type: 'POST',
        data: { galleryID: GalleryPageMngr.galleryID, id: jimage.attr('corbis-item-id') },
        success: onsuccess
    });
}
















