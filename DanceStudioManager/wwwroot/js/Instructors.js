

$(document).ready(function () {
    var grid_selector = "#jqGrid";
    var pager_selector = "#jqGridPager";

    jQuery(grid_selector).jqGrid({
        url: '/Studio/GetInstructors',
        datatype: "json",
        height: 450,
        colNames: ['Firstname', 'Lastname', 'Email', 'CellPhone', 'Gender'],
        colModel: [
            { name: 'firstname', index: 'Firstname', width: 250, sortable: true },
            { name: 'lastname', index: 'Lastname', width: 250 },
            { name: 'email', index: 'Email', width: 250 },
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

