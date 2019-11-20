
$(document).ready(function () {

    var dateFrom = "";
    var dateTo = "";
    var classGenre = "";
    var level = "";

    createGrid(dateFrom, dateTo, classGenre, level);

    $("#DateFrom").datepicker({ autoclose: true, todayBtn: 'linked', orientation: 'auto down' })
    $("#DateTo").datepicker({ autoclose: true, todayBtn: 'linked' })
});


var grid_selector = "#jqGrid";
var pager_selector = "#jqGridPager";

$("#SearchProfitForClass").click(function (e) {
    e.preventDefault();
    var formDataSearch = getMyAppsDataFromScreen();
    var $grid1 = $(grid_selector);
    $grid1.jqGrid('clearGridData').jqGrid('setGridParam',
        {
            url: '/Reports/SearchProfitForPeriod?dateFrom=' + formDataSearch.dateFrom + '&dateTo=' + formDataSearch.dateTo + '&classGenre=' + formDataSearch.classGenre + '&level=' + formDataSearch.level + '&type=' + formDataSearch.type,
            search: false
        }).trigger("reloadGrid");
});
    
function getMyAppsDataFromScreen() {
    var formDataSearch = {};

    var dateFrom = $('#DateFrom').val();
    if (!dateFrom || dateFrom.length === 0)
        formDataSearch.dateFrom = null;
    else
        formDataSearch.dateFrom = dateFrom;

    var dateTo = $('#DateTo').val();
    if (!dateTo || dateTo.length === 0)
        formDataSearch.dateTo = null;
    else
        formDataSearch.dateTo = dateTo;

    var classGenre = $('#ClassGenre').val();
    if (!classGenre || classGenre.length === 0)
        formDataSearch.classGenre = null;
    else
        formDataSearch.classGenre = classGenre;

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

function createGrid(dateFrom, dateTo, classGenre, level, type) {
    jQuery(grid_selector).jqGrid({
        datatype: "json",
        url: '/Reports/SearchProfitForPeriod?dateFrom=' + dateFrom + '&dateTo=' + dateTo + '&classGenre=' + classGenre + '&level=' + level + '&type=' + type,
        height: 450,
        type: "POST",
        colNames: ['Class genre', 'Level', 'Type', 'Number of students','Attendances','Profit'],
        colModel: [
            { name: 'classGenre', index: 'ClassGenre', width: 200, firstsortorder: "desc" },
            { name: 'level', index: 'Level', width: 200 },
            { name: 'type', index: 'Type', width: 200 },
            { name: 'numberOfStudents', index: 'NumberOfStudents', width: 200 },
            { name: 'attendances', index: 'Attendances', width: 200 },
            { name: 'profitForPeriod', index: 'ProfitForPeriod', width: 200 }
            
        ],
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: pager_selector,
        altRows: true,
    });
};