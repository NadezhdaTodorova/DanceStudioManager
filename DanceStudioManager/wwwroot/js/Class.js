

$(document).ready(function () {
    var grid_selector = "#jqGrid";
    var pager_selector = "#jqGridPager";

    jQuery(grid_selector).jqGrid({
        url: '/Studio/GetClasses',
        datatype: "json",
        height: 450,
        colNames: ['Genre', 'Level', 'PricePerHour', 'Shedule', 'ClassType', 'NumberOfStudents', 'Instructors'],
        colModel: [
            { name: 'genre', index: 'Genre', width: 200, sortable: true },
            { name: 'level', index: 'Level', width: 150 },
            { name: 'pricePerHour', index: 'PricePerHour', width: 100 },
            { name: 'shedule', index: 'Shedule', width: 350 },
            { name: 'classType', index: 'ClassType', width: 100 },
            { name: 'numberOfStudents', index: 'NumberOfStudents', width: 105 },
            { name: 'instructors', index: 'Instructors', width: 150 }
        ],
        loadonce: true,
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: pager_selector,
        altRows: true,
        multiselect: true,
        multiboxonly: true
    });

    $("#Students").mousedown(function (e) {
        e.preventDefault();

        var select = this;
        var scroll = select.scrollTop;

        e.target.selected = !e.target.selected;

        setTimeout(function () { select.scrollTop = scroll; }, 0);

        $("#Students").focus();
    }).mousemove(function (e) { e.preventDefault() });

    $("#Instructors").mousedown(function (e) {
        e.preventDefault();

        var select = this;
        var scroll = select.scrollTop;

        e.target.selected = !e.target.selected;

        setTimeout(function () { select.scrollTop = scroll; }, 0);

        $("#Instructors").focus();
    }).mousemove(function (e) { e.preventDefault() });

    $("#SheduleDays").mousedown(function (e) {
        e.preventDefault();

        var select = this;
        var scroll = select.scrollTop;

        e.target.selected = !e.target.selected;

        setTimeout(function () { select.scrollTop = scroll; }, 0);

        $("#Students").focus();
    }).mousemove(function (e) { e.preventDefault() });

    $(function () {
        $('[data-toggle="tooltip"]').tooltip()
    })

});

