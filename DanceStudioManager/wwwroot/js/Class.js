

$(document).ready(function () {
    var genre = '';
    var level = '';
    var type = '';

    createGrid(genre, level, type);

    $("#StartDay").datepicker({ autoclose: true, todayBtn: 'linked' })

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


var myCustomEdit = function (cellvalue, options, rowObject) {
    return "<button type='button' class='btn btn-light btn-sm' data-toggle='modal' data-target='#EditClassModal' data-id='"+rowObject.id+"' onclick='EditF(this)'>Edit</button>";
};

function EditF(t) {
    
    $.ajax({
        type: "POST",
        url: '/Studio/Dashboard?classId=' + t.dataset.id,
        dataType: "json",
        success: function (data) {
            var content = "<tbody>";
            $.each(data.studentsList, function () {
                content += '<tr><td>' + this.firstname + " " + this.lastname + '</td><td>' + '<button type="button" class="close" data-id=' + this.id + ' onclick="DeleteStudent(this)" aria-label="Close"><span aria-hidden="true">&times;</span> </button>' + '</td></tr>';
            });
            content += "</tbody>";
            $('#StudentsTable').empty();
            $('#StudentsTable').append(content);

              content = "<tbody>";
            $.each(data.instructorsList, function () {
                content += '<tr><td>' + this.firstname + " " + this.lastname + '</td><td>' + '<button type="button" class="close" data-id=' + this.id + ' onclick="DeleteInstructor(this)"  aria-label="Close"><span aria-hidden="true">&times;</span> </button>' + '</td></tr>';
            });
            content += "</tbody>";

            $('#InstructorTable').empty();
            $('#InstructorTable').append(content);
            $('#price').empty();
            $('#price').append(data.pricePerHour);
            $('#level').empty();
            $('#level').append(data.level);
            $('#Shedule').empty();
            $('#Shedule').append(data.sheduleDays);
            $('#ClassId').val(data.classId);

        },
        error: function (xhr, thrownError) {
            if (xhr.status === 404) {
                alert(thrownError);
            }
        }
    });
}; 


function DeleteStudent(t) {

    $.ajax({
        type: "POST",
        url: '/Studio/DeleteStudent?id=' + t.dataset.id,
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

function DeleteInstructor(t) {

    $.ajax({
        type: "POST",
        url: '/Studio/DeleteInstructor?id=' + t.dataset.id,
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

var myCustomDelete = function (cellvalue, options, rowObject) {
    return "<button type='button' class='btn btn-light btn-sm' id='deleteButton' data-id='" + rowObject.id +"' onclick='deleteF(this)'>Delete</button>";
};

function deleteF(t) {

    $.ajax({
        type: "GET",
        url: '/Studio/DeleteClass?classId=' + t.dataset.id,
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

