﻿/// ***********  ADD REFERENCES HERE FOR INTELLISENSE SUPPORTING **********
/// <reference path="~/Scripts/jquery-1.7.1-vsdoc.js" />
/// <reference path="~/Scripts/jquery-ui-1.8.20.js" />
/// <reference path="~/Scripts/jquery-ui-timepicker-addon.js" />
/// <reference path="~/Scripts/Corbis/Corbis.Common.js" />
/// ***********************************************************************


var GalleryPageMngr =
{
    galleryID: null,
    changeImageOrderUrl: null
}

//LOAD DOCUMENT
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

    $('div.coverImage div.txtAlign a').live('click', function () {
        var jthis = $(this);
        _setCoverTextPosition(jthis.closest('li'));
    });

    //******* image toolbar handling
    //delete image
    $('div[corbis-content-image] span.toolbar a.close').live('click', function () {
        DeleteGalleryContentImage($(this).closest('div.contentImage'));
    });

    //delete image
    $('div[corbis-cover-image] span.toolbar a.close').live('click', function () {
        $('a.clear-content-trigger').trigger('click');
    });

    var _expandCollapseHandler = function (parent) {
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
    };
    $('div.contentImage span.toolbar a.minimal').live('click', function () {
        _expandCollapseHandler($(this).closest('div.contentImage'));
    });
    $('div.coverImage span.toolbar a.minimal').live('click', function () {
        _expandCollapseHandler($(this).closest('div.coverImage'));
    });


    //****** move images up/down
    //Move image up
    $('div.contentImage span.up_down a.up').live('click', function () {
        var jcurImg = $(this).closest('div.contentImage');

        var jimg = jcurImg.prev('div.contentImage');

        if (jimg.length == 0)
            return;

        _swapImages(jcurImg, jimg, 'up');
    });

    //Move image down
    $('div.contentImage span.up_down a.down').live('click', function () {
        var jcurImg = $(this).closest('div.contentImage');

        var jimg = jcurImg.next('div.contentImage');

        if (jimg.length == 0)
            return;

        _swapImages(jcurImg, jimg, 'down');
    });

    var _swapImages = function (jimag1, jimag2, direction) {
        var onsuccess = function (result) {
            if (result.success) {
                if (direction == 'up') {
                    $(jimag1).insertBefore(jimag2);
                }
                else {
                    $(jimag1).insertAfter(jimag2);
                }
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

    //****** adding live preview 	
    check();
    $('.contentImage .txtAlign li').bind('click', function () {
        refreshLivePreview($(this).parent().parent().parent().find('.imgPresenter .livePreview'), $(this).attr('corbis-data-position'));
    });
});

function check() {
    var content_image_position, active_content_wizard, counter, preview_width, preview_height, preview_image;

    $('.contentImage').each(function () {
        content_image_position = $(this).find('.imgArea .txtAlign li.active').attr('corbis-data-position');

        $(this).find('.txtArea .radioGroup input[type="radio"]').each(function () {
            if ($(this)[0].checked == true) {
                active_content_wizard = $(this).attr('corbis-txt-type')
            }
        });

        switch (active_content_wizard) {
            case 'empty':
            case 'text':
                preview_width = 0;
                preview_height = 0;
                break;
            default:
                preview_width = $(this).find('.txtInputs .' + active_content_wizard + ' #Width').val();
                preview_height = $(this).find('.txtInputs .' + active_content_wizard + ' #Height').val();

        }
        console.log(preview_height);
        preview_width = 100 * parseInt(preview_width) / 1024;
        preview_height = 100 * parseInt(preview_height) / 662;

        preview_image = $(this).find('.imgArea .imgPresenter .livePreview');
        console.log(preview_height);
        refreshLivePreview(preview_image, content_image_position, preview_width, preview_height);

    });
}


function refreshLivePreview(obj, pos, width, height) {

    var image = obj.parent().find('.imgPresenterTemplate');

    if ((width == 'undefined') || (width == null)) {
        width = 100 * parseInt(obj.width()) / 300;
    }
    if ((height == 'undefined') || (height == null)) {
        height = 100 * parseInt(obj.height()) / 193;
    }

    obj.removeAttr('style');
    image.removeAttr('style');

    obj.css('width', width + '%');
    obj.css('height', height + '%');

    switch (pos) {
        case 'left':
        case 'topleft':
        case 'bottomleft':
            obj.css('left', '0px');
            obj.css('top', '50%');
            obj.css('margin-top', '-' + parseInt(obj.height() / 2) + 'px');
            image.css('right', '0px');
            image.css('left', 'auto');
            break;
        case 'right':
        case 'topright':
        case 'bottomright':
            obj.css('right', '0px');
            obj.css('top', '50%');
            obj.css('margin-top', '-' + parseInt(obj.height() / 2) + 'px');
            image.css('left', '0px');
            image.css('right', 'auto');
            break;
        case 'top':
            obj.css('top', '0px');
            obj.css('left', '50%');
            obj.css('margin-left', '-' + parseInt(obj.width() / 2) + 'px');
            image.css('bottom', '0px');
            image.css('top', 'auto');
            break;
        case 'bottom':
            obj.css('bottom', '0px');
            obj.css('left', '50%');
            obj.css('margin-left', '-' + parseInt(obj.width() / 2) + 'px');
            image.css('top', '0px');
            image.css('bottom', 'auto');
            break;
        case 'center':
            obj.css('left', '50%');
            obj.css('margin-left', '-' + parseInt(obj.width() / 2) + 'px');
            obj.css('top', '50%');
            obj.css('margin-top', '-' + parseInt(obj.height() / 2) + 'px');
            image.css('top', '0px');
            break;
    }
}

function checkContentDimension(selector) {
    var width = $(selector).find('.#Width').val();
    var height = $(selector).find('.#Height').val();
    var pos = $(selector).parents('.expanded').find('.txtAlign li.active').attr('corbis-data-position');
    var obj = $(selector).parents('.expanded').find('.livePreview');

    width = 100 * parseInt(width) / 1024;
    height = 100 * parseInt(height) / 662;

    refreshLivePreview(obj, pos, width, height);
}

function initGalleryImageDragDrop(id) {
    //!!! See http://threedubmedia.com/code/event/drag !!!
    var jimg = $('div.contentImage[corbis-item-id="' + id + '"]');

    jimg.find('span.dragndrop').draggable({
        helper: function () {
            var jelem = $(this).closest('div.contentImage');
            var out = ($(jelem).find('div.collapsed').is(':visible') ? $(jelem).find('div.collapsed img') : jelem.find('div.imgPresenter img')).clone();
            out.attr('corbis-item-id', jelem.attr('corbis-item-id'));
            return out;
        },
        opacity: 0.8, 
        start: function (event, ui) {
            $('div.contentImage[corbis-item-id="' + ui.helper.attr('corbis-item-id') + '"]').addClass('corbis-drop-ignore');
        },
        stop: function (event, ui) {
            $('div.contentImage').removeClass('corbis-drop-ignore');
            endWaitCursor();
        },
        scrollSpeed: 10,
        revert: false,
        zIndex: 100
    });
    jimg.droppable({
        tolerance: 'touch',
        over: function (event, ui) {
        },
        drop: function (event, ui) {
            var dragID = ui.helper.attr('corbis-item-id');
            var dropID = $(event.target).attr('corbis-item-id');
            if (dragID == dropID) return;

            beginWaitCursor();

            var first = null, second = null;

            var drag = $('div.contentImage[corbis-item-id="' + dragID + '"]');
            var drop = $(drag).next('div.contentImage[corbis-item-id="' + dropID + '"]');

            if (drop.length != 0) {
                first = drop;
                second = drag;
            }
            else {
                first = drag;
                second = $('div.contentImage[corbis-item-id="' + dropID + '"]');
            }

            if (first.length == 0 || second.length == 0) {
                endWaitCursor();
                return;
            }

            $.ajax({
                url: GalleryPageMngr.changeImageOrderUrl,
                type: 'POST',
                data: { galleryID: GalleryPageMngr.galleryID, firstImageID: first.attr('corbis-item-id'), secondImageID: second.attr('corbis-item-id') },
                success: function (result) {
                    if (!result.success) return;
                    first.insertBefore(second);
                },
                complete: function () {
                    endWaitCursor();
                }
            });
        }
    });
}


function onCoverContentSaveSuccess(data) {
    if (data.success) {
        var helper = new validationSummaryHelper($('form#coverTextContentForm').find('div.validation-summary-errors'));
        helper.reset();
        alert('Cover text content was saved successfully');
    }
}
function onSaveGalleryAttributes(data) {
    if (data.success) {
        alert('Gallery updated successfully');
        var helper = new validationSummaryHelper($('#galleryAttributesForm div.validation-summary-errors'));
        helper.reset();
    }
    else { 
    }
}

function ExpandImagesAll() {
    $('div.contentImage div.collapsed,div.coverImage div.collapsed').hide();
    $('div.contentImage div.expanded,div.coverImage div.expanded').show();
}
function CollapseImagesAll() {
    $('div.contentImage div.collapsed,div.coverImage div.collapsed').show();
    $('div.contentImage div.expanded,div.coverImage div.expanded').hide();
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
function _setCoverTextPosition(data) {
    var jli = (typeof data == "string") ? $('div.coverImage div.txtAlign li[corbis-data-position="' + data + '"]') : data;
    jli.addClass('active');
    jli.siblings('li').removeClass('active');
    jli.closest('div.coverImage').find('div.txtInputs input[name="TextPosition"]').val(jli.attr('corbis-data-position'));
}
function _onTextContentSaveSuccess(data, selector) {
    if (data.success) {
        alert('Image updated successfully');

	checkContentDimension(selector);
		
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
        $(jroot).find('div.radioGroup input[type="radio"]').removeAttr('checked');
        $(jroot).find('div.radioGroup input[corbis-txt-type="' + options.txtMode + '"]').attr('checked', 'checked');
        check();
    }

    initGalleryImageDragDrop(options.imageID);
}


function ClearGalleryContent(id, url) {
    if (!confirm("Do you really want to detele gallery image(s)?"))
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
        if (result.success) {
            jimage.remove();

            var jclose = $('div[corbis-cover-image] span.toolbar>a.close');

            if ($('div[corbis-content-image]').length == 0)
                jclose.show();                
        }
    };
    $.ajax({
        url: GalleryPageMngr.deleteContentImageURL,
        type: 'POST',
        data: { galleryID: GalleryPageMngr.galleryID, id: jimage.attr('corbis-item-id') },
        success: onsuccess
    });
}

function uploadImageErrorCallback(type, errorMessage) {
    alert("[errorCallBack] : " + type + " ::: " + errorMessage);
    return "OK";
}

//function reUploadGalleryImageCallback(result) {
//    var data = jQuery.parseJSON(result);
//    var jimg = $('div[corbis-item-id="' + data.ID + '"] div.imgPresenter img');
//    jimg.attr('src', data.EditUrls.Large);
//    jimg.attr('alt', data.FileName);
//    $('div.galleryContentImage').append(data);
//    return "OK";
//}

function onGalleryPublishSuccess(result) { 
}








