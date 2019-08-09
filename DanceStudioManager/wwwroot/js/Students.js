$(document).ready(function () {

    $(function () {
        $grid = $("jqGrid").jqGrid
        $grid.jqGrid({
            url: '/Studio/GetStudents',
            mtype: 'GET',
            datatype: 'json',
            colModel: [
                { name: 'Firstname', index: 'Firstname', width: 55 },
                { name: 'Lastname', index: 'Lastname', width: 95 },
                { name: 'Email', index: 'Email', width: 300 },
                { name: 'CellPhone', index: 'CellPhone', width: 400, sortable: false },
                { name: 'Gender', index: 'Gender', width: 400, sortable: false }
            ],
            loadonce: true
        });

    });
});