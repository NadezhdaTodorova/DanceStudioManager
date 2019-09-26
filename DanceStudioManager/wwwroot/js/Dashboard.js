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
                url: '/Studio/StartClass?id=' + this.id,
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

    $('[data-toggle="modal"]').on('click', function () {
        var myBookId = $(this).data('id');
        $.ajax({
            type: "POST",
            url: '/Studio/Dashboard?classId=' + myBookId,
            dataType: "json",
            success: function (data) {
                var content = "<tbody>"
                $.each(data.students, function () {
                    content += '<tr><td>' + this.firstname + " " + this.lastname + '</td></tr>';
                });
                content += "</tbody>"
                
                $('#StudentsTable').append(content);

                var content = "<tbody>"
                $.each(data.instructors, function () {
                    content += '<tr><td>' + this.firstname + " " + this.lastname + '</td></tr>';
                });
                content += "</tbody>"

                $('#InstructorTable').append(content);
                $('#price').append(data.pricePerHour);
                $('#level').append(data.level);

                console.log($('#price').val(data.pricePerHour));
            },
            error: function (xhr, thrownError) {
                if (xhr.status == 404) {
                    alert(thrownError);
                }
            }
        });
    });


});