

$(document).ready(function () {
    var genre = '';
    var level = '';
    var type = '';

    createGrid(genre, level, type);

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

var grid_selector = "#jqGrid";
var pager_selector = "#jqGridPager";

$("#SearchClasses").click(function (e) {
    e.preventDefault();
    var formDataSearch = getMyAppsDataFromScreen();
    var $grid1 = $(grid_selector);
    $grid1.jqGrid('clearGridData').jqGrid('setGridParam',
        {
            url: '/Studio/GetClasses?genre=' + formDataSearch.genre + '&level=' + formDataSearch.level + '&type=' + formDataSearch.type,
            search: false
        }).trigger("reloadGrid");
});

function getMyAppsDataFromScreen() {
    var formDataSearch = {};

    var genre = $('#Genre').val();
    if (!genre || genre.length === 0)
        formDataSearch.genre = null;
    else
        formDataSearch.genre = genre;

    var level = $('#Level').val();
    if (!level || level.length === 0)
        formDataSearch.level = null;
    else
        formDataSearch.level = level;

    var type = $('#ClassType').val();
    if (!type || type.length === 0)
        formDataSearch.type = null;
    else
        formDataSearch.type = type;

    return formDataSearch;
}

function createGrid(genre, level, type) {
    jQuery(grid_selector).jqGrid({
        url: '/Studio/GetClasses?genre=' + genre + '&level=' + level + '&type=' + type,
        datatype: "json",
        height: 450,
        type: "POST",
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
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: pager_selector,
        altRows: true,
        multiselect: true,
    });
};

