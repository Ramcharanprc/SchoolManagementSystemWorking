$(document).ready(function () {
    var url = window.location.pathname;
    $('ul.index-main-tablist').find('a').each(function () {

            if ($(this).attr('href') === url) {
                $(this).parent().addClass('active');
            }
            else {
                $(this).parent().removeClass('active');
            }
        });
    
});

