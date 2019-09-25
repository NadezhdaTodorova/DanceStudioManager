$(document).ready(function () {

    //localStorage.clear();

    $('[data-toggle="tooltip"]').tooltip();

    var checkboxValues = JSON.parse(localStorage.getItem('checkboxValues')) || {};
    var disabledCheckboxes = JSON.parse(localStorage.getItem('disabledCheckboxes')) || {};

    $.each(checkboxValues, function (key, value) {
        $("#" + key).prop('checked', value);
    });

    $.each(disabledCheckboxes, function (key, value) {
        $("#" + key).prop('disabled', value);
    });


    $('input[id]').on('click', function () {
        if ($('#' + this.id).is(":checked")) {
            $.ajax({
                type: "POST",
                url: '/Studio/UpdateClass?id=' + this.id,
                success: function (data) {
                    $('#' + data).attr('disabled', true);

                    checkboxValues[data] = this.checked = true;
                    disabledCheckboxes[data] = this.disabled = true;

                    localStorage.setItem("checkboxValues", JSON.stringify(checkboxValues));
                    localStorage.setItem("disabledCheckboxes", JSON.stringify(disabledCheckboxes));
                },
                dataType: "json"
            });
        }
    });

});