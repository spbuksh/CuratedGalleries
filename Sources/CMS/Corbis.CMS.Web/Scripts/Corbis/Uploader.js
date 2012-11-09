/// ***********  ADD REFERENCES HERE FOR INTELLISENSE SUPPORTING **********
/// <reference path="~/Scripts/jquery-1.7.1-vsdoc.js" />
/// <reference path="~/Scripts/swfobject.js" />
/// ***********************************************************************

function createImageUploader(options) {
    if (!options.url || options.url == '') throw 'Upload url is required';
    if (!options.contentDivID || options.contentDivID == '') throw 'Uploader content div id is required';
    if(!options.id || options.id == '') throw 'Uploader id is required';

    if(options.filter == null || options.filter == undefined) options.filter = "*.*";
    if(options.minFileSize == null || options.minFileSize == undefined) options.minFileSize = 0;
    if(options.maxFileSize == null || options.maxFileSize == undefined) options.maxFileSize = 62914560;
    if(options.bgcolor == null || options.bgcolor == undefined) options.bgcolor = "#003300";
    if(options.greetingText == null || options.greetingText == undefined) options.greetingText = "Press button to upload images";
    if(options.uploadAttemps == null || options.uploadAttemps == undefined) options.uploadAttemps = 1;
    if(options.viewMode == null || options.viewMode == undefined) options.viewMode = "multiple";
    if(options.name == null || options.name == undefined) options.name = "ImageUploader";

    if(options.filter == null || options.filter == undefined) options.filter = "*.*";

    var flashvars = {};
    flashvars.filterExtension = options.filter;
    flashvars.sizeLimitMax = options.maxFileSize ? options.maxFileSize.toString() : null;
    flashvars.sizeLimitMin = options.minFileSize ? options.minFileSize.toString() : "0",
    flashvars.uploadURL = options.url;
    flashvars.greetingText = options.greetingText;
    flashvars.uploadCallBack = options.uploadCallBack;
    flashvars.errorCallBack = options.errorCallBack;
    flashvars.uploadAttemps = options.uploadAttemps;
    flashvars.viewMode = options.viewMode;

    var params = {};
    params.quality = "high";
    params.bgcolor = options.bgcolor;
    params.allowscriptaccess = "always";
    params.allowfullscreen = "true";
    var attributes = {};
    attributes.id = options.id;
    attributes.name = options.name;
    attributes.align = "middle";
    swfobject.embedSWF(
                        "../../SimpleUploader.swf", options.contentDivID,
                        options.size.width.toString(), options.size.height.toString(),
                        "11.1.0", "playerProductInstall.swf",
                        flashvars, params, attributes);
}

