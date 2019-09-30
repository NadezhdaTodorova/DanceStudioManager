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
        var classId = $('input[id]').attr('id');
        $.ajax({
            type: "POST",
            url: '/Studio/Dashboard?classId=' + classId,
            dataType: "json",
            success: function (data) {
                var content = "<tbody>"
                $.each(data.students, function () {
                    content += '<tr><td>' + this.firstname + " " + this.lastname + '</td><td>' + '<button type="button" class="close" aria-label="Close"><span aria-hidden="true">&times;</span> </button>' + '</td></tr>';
                });
                content += "</tbody>"
                $('#StudentsTable').append(content);

                var content = "<tbody>"
                $.each(data.instructors, function () {
                    content += '<tr><td>' + this.firstname + " " + this.lastname + '</td><td>' + '<button type="button" class="close"  aria-label="Close"><span aria-hidden="true">&times;</span> </button>' + '</td></tr>';
                });
                content += "</tbody>"

                $('#InstructorTable').append(content);
                $('#price').append(data.pricePerHour);
                $('#level').append(data.level);
            },
            error: function (xhr, thrownError) {
                if (xhr.status == 404) {
                    alert(thrownError);
                }
            }
        });
    }); 

    var ctx = document.getElementById('bigDashboardChart').getContext("2d");

    var gradientStroke = ctx.createLinearGradient(500, 0, 100, 0);
    gradientStroke.addColorStop(0, '#80b6f4');
    gradientStroke.addColorStop(1, chartColor);

    var gradientFill = ctx.createLinearGradient(0, 200, 0, 50);
    gradientFill.addColorStop(0, "rgba(128, 182, 244, 0)");
    gradientFill.addColorStop(1, "rgba(255, 255, 255, 0.24)");

    $.ajax({
        type: "POST",
        url: '/Studio/DashboardChart',
        dataType: "json",
        success: function (data) {
            var myChart = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: ["JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC"],
                    datasets: [{
                        label: "Attendance",
                        borderColor: chartColor,
                        pointBorderColor: chartColor,
                        pointBackgroundColor: "#1e3d60",
                        pointHoverBackgroundColor: "#1e3d60",
                        pointHoverBorderColor: chartColor,
                        pointBorderWidth: 1,
                        pointHoverRadius: 7,
                        pointHoverBorderWidth: 2,
                        pointRadius: 5,
                        fill: true,
                        backgroundColor: gradientFill,
                        borderWidth: 2,
                        data: data
                    }]
                },

                options: {
                    layout: {
                        padding: {
                            left: 20,
                            right: 20,
                            top: 0,
                            bottom: 0
                        }
                    },
                    //title: {
                    //    display: true,
                    //    text: 'People attendance during the year',
                    //    position: 'top',
                    //    fontSize: 15,
                    //    fontFamily: 'Arial',
                    //    fontColor: '#d0d0e1',
                    //    padding: 5
                    //},
                    maintainAspectRatio: false,
                    tooltips: {
                        backgroundColor: '#fff',
                        titleFontColor: '#333',
                        bodyFontColor: '#666',
                        bodySpacing: 4,
                        xPadding: 12,
                        mode: "nearest",
                        intersect: 0,
                        position: "nearest"
                    },
                    legend: {
                        position: "bottom",
                        fillStyle: "#FFF",
                        display: false
                    },
                    scales: {
                        yAxes: [{
                            ticks: {
                                fontColor: "rgba(255,255,255,0.4)",
                                fontStyle: "bold",
                                beginAtZero: true,
                                maxTicksLimit: 5,
                                padding: 10
                            },
                            gridLines: {
                                drawTicks: true,
                                drawBorder: false,
                                display: true,
                                color: "rgba(255,255,255,0.1)",
                                zeroLineColor: "transparent"
                            }

                        }],
                        xAxes: [{
                            gridLines: {
                                zeroLineColor: "transparent",
                                display: false,

                            },
                            ticks: {
                                padding: 10,
                                fontColor: "rgba(255,255,255,0.4)",
                                fontStyle: "bold"
                            }
                        }]
                    }
                }
            });
        },
        error: function (xhr, thrownError) {
            if (xhr.status === 404) {
                alert(thrownError);
            }
        }

   
    });
});