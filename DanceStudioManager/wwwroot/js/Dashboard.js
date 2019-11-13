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
                var content = "<tbody>";
                $.each(data.students, function () {
                    content += '<tr><td>' + this.firstname + " " + this.lastname + '</td><td>' + '<button type="button" class="close" aria-label="Close"><span aria-hidden="true">&times;</span> </button>' + '</td></tr>';
                });
                content += "</tbody>";
                $('#StudentsTable').append(content);

                var content2 = "<tbody>";
                $.each(data.instructors, function () {
                    content2 += '<tr><td>' + this.firstname + " " + this.lastname + '</td><td>' + '<button type="button" class="close"  aria-label="Close"><span aria-hidden="true">&times;</span> </button>' + '</td></tr>';
                });
                content2 += "</tbody>";

                $('#InstructorTable').append(content);
                $('#price').append(data.pricePerHour);
                $('#level').append(data.level);
            },
            error: function (xhr, thrownError) {
                if (xhr.status === 404) {
                    alert(thrownError);
                }
            }
        });
    }); 

    var ctx = document.getElementById('bigDashboardChart').getContext("2d");

    var gradientStroke = ctx.createLinearGradient(500, 0, 100, 0);
    gradientStroke.addColorStop(0, '#80b6f4');
    gradientStroke.addColorStop(1, chartColor);

    var b = ctx.createLinearGradient(0, 200, 0, 50);
    b.addColorStop(0, "rgba(128, 182, 244, 0)");
    b.addColorStop(1, "rgba(255, 255, 255, 0.24)");

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
                        backgroundColor: b,
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



    var e = document.getElementById("ProfitChart").getContext("2d");

    var gradientFill = e.createLinearGradient(0, 170, 0, 50);
    gradientFill.addColorStop(0, "rgba(128, 182, 244, 0)");
    gradientFill.addColorStop(1, hexToRGB('#2CA8FF', 0.6));

    $.ajax({
        type: "POST",
        url: '/Studio/ProfitDashboardChart',
        dataType: "json",
        success: function (data) {
            var myChart = new Chart(e, {
                type: "bar",
                data: {
                    labels: data.dats,
                    datasets: [{
                        label: "profit",
                        backgroundColor: gradientFill,
                        borderColor: "#2CA8FF",
                        pointBorderColor: "#FFF",
                        pointBackgroundColor: "#2CA8FF",
                        pointBorderWidth: 2,
                        pointHoverRadius: 4,
                        pointHoverBorderWidth: 1,
                        pointRadius: 4,
                        fill: true,
                        borderWidth: 1,
                        data: data.dataProft
                    }]
                },
                options: {
                    maintainAspectRatio: false,
                    legend: {
                        display: false
                    },
                    tooltips: {
                        bodySpacing: 4,
                        mode: "nearest",
                        intersect: 0,
                        position: "nearest",
                        xPadding: 10,
                        yPadding: 10,
                        caretPadding: 10
                    },
                    responsive: 1,
                    scales: {
                        yAxes: [{
                            gridLines: 0,
                            gridLines: {
                                zeroLineColor: "transparent",
                                drawBorder: false
                            },
                            ticks: {
                                beginAtZero: true,
                            }
                        }],
                        xAxes: [{
                            display: 0,
                            gridLines: 0,
                            ticks: {
                                display: false
                            },
                            gridLines: {
                                zeroLineColor: "transparent",
                                drawTicks: false,
                                display: false,
                                drawBorder: false
                            }
                        }]
                    },
                    layout: {
                        padding: {
                            left: 0,
                            right: 0,
                            top: 15,
                            bottom: 15
                        }
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

    var viewsChart = new Chart(e);
});