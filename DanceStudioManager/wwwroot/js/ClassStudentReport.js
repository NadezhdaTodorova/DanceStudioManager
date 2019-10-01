
$(document).ready(function () {

    var grid_selector = "#jqGrid";
    var pager_selector = "#jqGridPager";

    $("#SearchStudents").click(function () {
        var formDataSearch = getMyAppsDataFromScreen();
        createGrid(formDataSearch.genre, formDataSearch.level, formDataSearch.type);
    });

    function getMyAppsDataFromScreen() {
        var formDataSearch = {};

        var genre = $('#Genre').val();
        if (!genre || genre.length === 0)
            formDataSearch.genre = null;
        else
            formDataSearch.genre = genre;

        var level = $('#level').val();
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
            colNames: ['Firstname', 'Lastname', 'Email', 'CellPhone', 'Gender', 'Class name'],
            colModel: [
                { name: 'firstname', index: 'Firstname', width: 200, firstsortorder: "desc" },
                { name: 'lastname', index: 'Lastname', width: 200 },
                { name: 'email', index: 'Email', width: 200 },
                { name: 'cellPhone', index: 'CellPhone', width: 250 },
                { name: 'gender', index: 'Gender', width: 250 },
                { name: 'genre', index: 'Genre', width: 250 }
            ],
            rowNum: 10,
            rowList: [10, 20, 30],
            pager: pager_selector,
            altRows: true,
            multiselect: true,
        }).navGrid('#jqGridPager', { add: false, edit: true, del: true, search: false, refresh: true });
    }
});