jQuery.fn.textaligment = function () {
    var textaligwrapper;
    var buttonlist;
    var parametr;
    this.each(function() {
        textaligwrapper = $(this).find('.txtAlign');
        buttonlist = textaligwrapper.find('li');
        buttonlist.each(function() {
            $(this).click(function () {
                $(this).parents('.txtAlign').siblings('.imgPresenter').children('.textBlock').remove();
                $(this).parents('.txtAlign').siblings('.imgPresenter').append('<div class="textBlock"></div>');
                parametr = $(this).attr('class');
                positionStyles(parametr);
            });
        });
    });
    function positionStyles(parametr) {
        switch (parametr) {
        case 'left':
            $('.textBlock').css({
                'position': 'absolute',
                'left': 0,
                'top': 0,
                'width': 50 + '%',
                'height': 100 + '%'
            });
            break;
        case 'right':
            $('.textBlock').css({
                'position': 'absolute',
                'right': 0,
                'top': 0,
                'width': 50 + '%',
                'height': 100 + '%'
            });
            break;
        case 'center':
            $('.textBlock').css({
                'position': 'absolute',
                'left': 50 + '%',
                'top': 50 + '%',
                'margin-left': -25 + '%',
                'margin-top': -25 + '%',
                'width': 50 + '%',
                'height': 50 + '%'
            });
            break;
        case 'top':
            $('.textBlock').css({
                'position': 'absolute',
                'left': 0,
                'top': 0,
                'width': 100 + '%',
                'height': 50 + '%'
            });
            break;
        case 'bottom':
            $('.textBlock').css({
                'position': 'absolute',
                'left': 0,
                'top': 0,
                'width': 50 + '%',
                'height': 100 + '%'
            });
            break;
        case 'topLeft':
            $('.textBlock').css({
                'position': 'absolute',
                'left': 0,
                'top': 0,
                'width': 50 + '%',
                'height': 50 + '%'
            });
            break;
        case 'topRight':
            $('.textBlock').css({
                'position': 'absolute',
                'right': 0,
                'top': 0,
                'width': 50 + '%',
                'height': 50 + '%'
            });
            break;
        case 'bottomLeft':
            $('.textBlock').css({
                'position': 'absolute',
                'left': 0,
                'bottom': 0,
                'width': 50 + '%',
                'height': 50 + '%'
            });
            break;
        case 'bottomRight':
            $('.textBlock').css({
                'position': 'absolute',
                'right': 0,
                'bottom': 0,
                'width': 50 + '%',
                'height': 50 + '%'
            });
            break;
        }
        ;
    }
}