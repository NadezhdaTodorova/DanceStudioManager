

$(document).ready(function () {

    var grid_selector = "#jqGrid";
    var pager_selector = "#jqGridPager";

    jQuery(grid_selector).jqGrid({
        url: '/Studio/GetStudents',
        datatype: "json",
        height: 450,
        colNames: ['Firstname', 'Lastname', 'Email', 'CellPhone', 'Gender'],
        colModel: [
            { name: 'firstname', index: 'Firstname', width: 200, sortable: true},
            { name: 'lastname', index: 'Lastname', width: 200 },
            { name: 'email', index: 'Email', width: 200 },
            { name: 'cellPhone', index: 'CellPhone', width: 250 },
            { name: 'gender', index: 'Gender', width: 250 }
        ],
        loadonce: true,
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: pager_selector,
        altRows: true,
        multiselect: true,
        multiboxonly: true
    });
});

    