

$(document).ready(function () {
    var grid_selector = "#jqGrid";
    var pager_selector = "#jqGridPager";

    jQuery(grid_selector).jqGrid({
        url: '/Studio/GetClasses',
        datatype: "json",
        height: 450,
        colNames: ['Genre', 'Level', 'PricePerHour', 'Shedule', 'ClassType', 'NumberOfStudents'],
        colModel: [
            { name: 'genre', index: 'Genre', width: 250, sortable: true },
            { name: 'level', index: 'Level', width: 250 },
            { name: 'pricePerHour', index: 'PricePerHour', width: 150 },
            { name: 'shedule', index: 'Shedule', width: 250 },
            { name: 'classType', index: 'ClassType', width: 200 },
            { name: 'numberOfStudents', index: 'NumberOfStudents', width: 150 }
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

