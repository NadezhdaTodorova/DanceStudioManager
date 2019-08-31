

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
            { name: 'level', index: 'Level', width: 200 },
            { name: 'pricePerHour', index: 'PricePerHour', width: 100 },
            { name: 'shedule', index: 'Shedule', width: 250 },
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
});

