$(document).ready(function () {
    $(".nav li").on("click", function () {
        $('.active').removeClass('active');
        $(this).addClass('active');
    });
});