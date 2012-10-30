/// ***********  ADD REFERENCES HERE FOR INTELLISENSE SUPPORTING **********
/// <reference path="~/Scripts/jquery-1.7.1-vsdoc.js" />
/// <reference path="~/Scripts/Corbis/Corbis.Common.js" />
/// ***********************************************************************



$(function () {
    //****** it activates/deactivates image text input data *************
    var handler = function (jradio) {
        jradio.closest('div.txtArea').find('div.' + jradio.attr('corbis-txt-type')).show().siblings().hide();
    };

    var jitems = $('div.contentImage div.radioGroup input:radio');

    jitems.live('click', function () {
        handler($(this));
    });

    handler(jitems.filter(':checked'));


    $('div.contentImage div.txtAlign a').live('click', function () {
        var jthis = $(this);
        _setTextPosition(jthis.closest('div.contentImage'), null, jthis.closest('li'));
    });
    //*******************************************************************
});


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
    }
    else {
        var jelem = $(selector);
        jelem.html('');
        jelem.html(data);
    }
}

function InitContentImage(options) {
    var jroot = $('div.contentImage[corbis-item-id="' + options.imageID + '"]');

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
function DeleteGalleryContentImage(id, url) {
    if (!confirm("Do you really want to detele this image?"))
        return;

    var onsuccess = function (result) {
        if (result.success)
            $('div.contentImage[corbis-item-id="' + id + '"]').remove();
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















