
$(document).ready(function () {
    $(document).ready(function () {
        $('[data-toggle="tooltip"]').tooltip();
    });
    $('#prevBtn,#nextBtn').on('click', function () {

        var Month = parseInt($('#Month').val());
        var Year = parseInt($('#Year').val());
        var nextYear = parseInt($('#Year').val()) + 1;
        var previousYear = parseInt($('#Year').val()) - 1;

        if (($(this)).hasClass("previous")) {
            Month -= 1;
            if (Month < 1) {
                if ($('#Year option[value=' + previousYear + ']').length > 0) {
                    Month = 12;
                    Year += -1;
                } else {
                    Month += 1;
                }
            }
        }
        if (($(this)).hasClass("next")) {
            Month += 1;
            if (Month > 12) {
                if ($('#Year option[value=' + nextYear + ']').length > 0) {
                    Month = 1;
                    Year += 1;
                } else {
                    Month -= 1;
                }
            }
        }

        $('#Month').val(Month);
        $('#Year').val(Year);
        redirectToLocation();
    });

    $('#Month,#Year').change(function () {
        redirectToLocation();
    });


});

function redirectToLocation() {
    document.location = document.location.origin + "/Calendar/Index?year=" + $("#Year").val() + "&month=" + $('#Month').val();
}

