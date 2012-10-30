function DeleteTemplate(id, url) {
    if (!confirm("Do you really want to detele template?"))
        return false;

    var onsuccess = function (result) {
        if (result.success) {
            $('li.templateItem[corbis-item-id="' + id + '"]').remove();
        } 

    };
   
    $.ajax({
        url: url,
        type: 'POST',
        data: { id: id },
        success: onsuccess
    });
}