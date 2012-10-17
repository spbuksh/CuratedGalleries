/// ***********  ADD REFERENCES HERE FOR INTELLISENSE SUPPORTING **********
/// <reference path="~/Scripts/jquery-1.7.1-vsdoc.js" />
/// <reference path="~/Scripts/swfobject.js" />
/// ***********************************************************************

var uploaderOptions = {
    url: "http://localhost:62506/api/Upload/FileUpload", //it is upload url
    callback: "callBackImageID",
    filter: "*.jpg;*.jpeg;*.tif;*.tiff;*.png",
    minFileSize: 0,
    maxFileSize: 62914560,
    bgcolor: "#003300",
    id: "SimpleUploader",
    name: "SimpleUploader",
    greetingText: "max file size 60Mb min file size 1 mb \nplease press button for upload images",
    size: { width: 700, height: 500 }
};

$(function () {
    var swfVersionStr = "11.1.0";

    var xiSwfUrlStr = "playerProductInstall.swf";

    var params = {};

    var flashvars = {};
    flashvars.filterExtension = uploaderOptions.filter;
    flashvars.sizeLimitMax = uploaderOptions.maxFileSize ? uploaderOptions.maxFileSize.toString() : null;
    flashvars.sizeLimitMin = uploaderOptions.minFileSize ? uploaderOptions.minFileSize.toString() : "0",
    flashvars.uploadURL = uploaderOptions.url;
    flashvars.greetingText = uploaderOptions.greetingText;

    params.quality = "high";
    params.bgcolor = uploaderOptions.bgcolor;
    params.allowscriptaccess = "sameDomain";
    params.allowfullscreen = "true";
    var attributes = {};
    attributes.id = uploaderOptions.id;
    attributes.name = uploaderOptions.name;
    attributes.align = "middle";
    swfobject.embedSWF(
                        "../../SimpleUploader.swf", "flashContent",
                        uploaderOptions.size.width.toString(), uploaderOptions.size.width.toString(),
                        swfVersionStr, xiSwfUrlStr,
                        flashvars, params, attributes);
    swfobject.createCSS("#flashContent", "display:block;text-align:left;");
});