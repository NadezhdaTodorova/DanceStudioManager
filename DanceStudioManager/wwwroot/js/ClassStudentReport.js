
$(document).ready(function () {

    var genre = '';
    var level = '';
    var classtype = '';

    createGrid(genre, level, classtype);

});

var grid_selector = "#jqGrid";
var pager_selector = "#jqGridPager";

$("#SearchStudentsForClass").click(function (e) {
    e.preventDefault();
    var formDataSearch = getMyAppsDataFromScreen();
    var $grid1 = $(grid_selector);
    $grid1.jqGrid('clearGridData').jqGrid('setGridParam',
        {
            datatype: "json",
            url: '/Reports/SearchStudent?genre=' + formDataSearch.genre + '&level=' + formDataSearch.level + '&type=' + formDataSearch.type,
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
        url: '/Reports/SearchStudent?genre=' + genre + '&level=' + level + '&type=' + type,
        datatype: "json",
        height: 450,
        type: "POST",
        colNames: ['Firstname', 'Lastname', 'Email', 'Genre', 'Level'],
        colModel: [
            { name: 'firstname', index: 'Firstname', width: 200, sortable: false, },
            { name: 'lastname', index: 'Lastname', width: 200, sortable: false, },
            { name: 'email', index: 'Email', width: 200, sortable: false, },
            { name: 'genre', index: 'Genre', width: 250, sortable: false, },
            { name: 'level', index: 'Level', width: 250, sortable: false, }
        ],
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: pager_selector,
        altRows: true,
        loadonce: true,
    });
};

