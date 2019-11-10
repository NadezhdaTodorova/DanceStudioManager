

$(document).ready(function () {
    var genre = '';
    var level = '';
    var type = '';

    createGrid(genre, level, type);

    $(".Students").mousedown(function (e) {
        e.preventDefault();

        var select = this;
        var scroll = select.scrollTop;

        e.target.selected = !e.target.selected;

        setTimeout(function () { select.scrollTop = scroll; }, 0);

        $(".Students").focus();
    }).mousemove(function (e) { e.preventDefault() });

    $(".Instructors").mousedown(function (e) {
        e.preventDefault();

        var select = this;
        var scroll = select.scrollTop;

        e.target.selected = !e.target.selected;

        setTimeout(function () { select.scrollTop = scroll; }, 0);

        $(".Instructors").focus();
    }).mousemove(function (e) { e.preventDefault() });

    $(".SheduleDays").mousedown(function (e) {
        e.preventDefault();

        var select = this;
        var scroll = select.scrollTop;

        e.target.selected = !e.target.selected;

        setTimeout(function () { select.scrollTop = scroll; }, 0);

        $(".SheduleDays").focus();
    }).mousemove(function (e) { e.preventDefault() });

    $(function () {
        $('[data-toggle="tooltip"]').tooltip()
    })
});

$("#EditClassModal").on("hidden.bs.modal", function (e) {
    $(this).find('form')[0].reset();
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


var myCustomEdit = function () {
    return "<button type='button' class='btn btn-light btn-sm' data-toggle='modal' data-target='#EditClassModal' onclick='EditF()'>Edit</button>";
};

 function EditF () {

    var grid = $("#jqGrid");
    var rowKey = grid.jqGrid('getGridParam', "selrow");

    $.ajax({
        type: "POST",
        url: '/Studio/Dashboard?classId=' + rowKey,
        dataType: "json",
        success: function (data) {
            var content = "<tbody>";
            $.each(data.students, function () {
                content += '<tr><td>' + this.firstname + " " + this.lastname + '</td><td>' + '<button type="button" class="close" aria-label="Close"><span aria-hidden="true">&times;</span> </button>' + '</td></tr>';
            });
            content += "</tbody>";
            $('#StudentsTable').append(content);

             var content2 = "<tbody>";
            $.each(data.instructors, function () {
                content2 += '<tr><td>' + this.firstname + " " + this.lastname + '</td><td>' + '<button type="button" class="close"  aria-label="Close"><span aria-hidden="true">&times;</span> </button>' + '</td></tr>';
            });
            content2 += "</tbody>";

            $('#InstructorTable').append(content);
            $('#price').append(data.pricePerHour);
            $('#level').append(data.level);
            $('#Shedule').append(data.shedule);
        },
        error: function (xhr, thrownError) {
            if (xhr.status === 404) {
                alert(thrownError);
            }
        }
    });
}; 

var myCustomDelete = function () {
    return "<button type='button' class='btn btn-light btn-sm' id='deleteButton' onclick='deleteF()'>Delete</button>";
};

function deleteF() {
    var grid = $("#jqGrid");
    var rowKey = grid.jqGrid('getGridParam', "selrow");
    $.ajax({
        type: "GET",
        url: '/Studio/DeleteClass?classId=' + rowKey,
        success: function () {
            location.reload(); 
        },
        error: function (xhr, thrownError) {
            if (xhr.status === 404) {
                alert(thrownError);
            }
        }
    });
}

function createGrid(genre, level, type) {
    jQuery(grid_selector).jqGrid({
        url: '/Studio/GetClasses?genre=' + genre + '&level=' + level + '&type=' + type,
        datatype: "json",
        height: 450,
        type: "POST",
        colNames: ['Genre', 'Level', 'PricePerHour', 'Shedule', 'ClassType', 'NumberOfStudents', 'Instructors', 'Edit', 'Delete'],
        colModel: [
            { name: 'genre', index: 'Genre', width: 100, sortable: true, classes: 'pointer', editable: true },
            { name: 'level', index: 'Level', width: 100, classes: 'pointer', editable: true },
            { name: 'pricePerHour', index: 'PricePerHour', width: 100, classes: 'pointer', editable: true },
            { name: 'shedule', index: 'Shedule', width: 350, classes: 'pointer', editable: true },
            { name: 'classType', index: 'ClassType', width: 100, classes: 'pointer', editable: true },
            { name: 'numberOfStudents', index: 'NumberOfStudents', width: 105, classes: 'pointer', editable: true },
            { name: 'instructors', index: 'Instructors', width: 150, classes: 'pointer', editable: true },
            { name: 'act', index: 'act', width: 75, align: 'center', sortable: false, formatter: myCustomEdit },
            { name: 'act', index: 'act', width: 75, align: 'center', sortable: false, formatter: myCustomDelete }
        ],
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: pager_selector,
        altRows: true
    });
};

