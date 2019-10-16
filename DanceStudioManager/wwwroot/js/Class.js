

$(document).ready(function () {
    var genre = '';
    var level = '';
    var type = '';

    createGrid(genre, level, type);

    $("#Students").mousedown(function (e) {
        e.preventDefault();

        var select = this;
        var scroll = select.scrollTop;

        e.target.selected = !e.target.selected;

        setTimeout(function () { select.scrollTop = scroll; }, 0);

        $("#Students").focus();
    }).mousemove(function (e) { e.preventDefault() });

    $("#Instructors").mousedown(function (e) {
        e.preventDefault();

        var select = this;
        var scroll = select.scrollTop;

        e.target.selected = !e.target.selected;

        setTimeout(function () { select.scrollTop = scroll; }, 0);

        $("#Instructors").focus();
    }).mousemove(function (e) { e.preventDefault() });

    $("#SheduleDays").mousedown(function (e) {
        e.preventDefault();

        var select = this;
        var scroll = select.scrollTop;

        e.target.selected = !e.target.selected;

        setTimeout(function () { select.scrollTop = scroll; }, 0);

        $("#Students").focus();
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

var template = "<table id='editClass' class='EditTable ui-common-table'>";
template += "<tbody>";
template += "<tr class='FormData' rowpos='1'>";
template += "<td class='CaptionTD'>";
template += "<label for='genre'>Genre</label>";
template += "</td>";
template += "<td class='DataTD'>";
template += "<input id='genre' class='FormElement ui-widget-content ui-corner-all'";
template += "</td>";
template += "</tr>";
template += "<tr rowpos='2' class='FormData' id='tr_level'>";
template += "<td class='CaptionTD'><label for='level'>Level</label>";
template += "</td>";
template += "<td class='DataTD'>";
template +=  "<input type = 'text' role = 'textbox' id = 'level' name = 'level' rowid = '10' module = 'form' checkupdate = 'false' size = '20' class='FormElement ui-widget-content ui-corner-all>'";
template += "</td>";
template += "</tr>";
template += "<tr rowpos='3' class='FormData' id='tr_pricePerHour'>";
template += "<td class='CaptionTD'><label for='pricePerHour'>PricePerHour</label>";
template += "</td>";
template += "<td class='DataTD'>";
template += "<input type='text' role='textbox' id='pricePerHour' name='pricePerHour' rowid='12' module='form' checkupdate='false' size='20' class='FormElement ui - widget - content ui - corner - all'>";
template += "</td>";
template += "</tr>";
template += "<tr rowpos='4' class='FormData' id='tr_shedule'>";
template += "<td class='CaptionTD'><label for='shedule'>Shedule</label>";
template += "</td>";
template += "<td class='DataTD'><input type='text' role='textbox' id='shedule' name='shedule' rowid='12' module='form' checkupdate='false' size='20' class='FormElement ui - widget - content ui - corner - all'>";
template += "</td>";
template += "</tr>";


function createGrid(genre, level, type) {
    jQuery(grid_selector).jqGrid({
        url: '/Studio/GetClasses?genre=' + genre + '&level=' + level + '&type=' + type,
        datatype: "json",
        height: 450,
        type: "POST",
        colNames: ['Genre', 'Level', 'PricePerHour', 'Shedule', 'ClassType', 'NumberOfStudents', 'Instructors'],
        colModel: [
            { name: 'genre', index: 'Genre', width: 200, sortable: true, classes: 'pointer', editable: true },
            { name: 'level', index: 'Level', width: 150, classes: 'pointer', editable: true },
            { name: 'pricePerHour', index: 'PricePerHour', width: 100, classes: 'pointer', editable: true },
            { name: 'shedule', index: 'Shedule', width: 350, classes: 'pointer', editable: true },
            { name: 'classType', index: 'ClassType', width: 100, classes: 'pointer', editable: true },
            { name: 'numberOfStudents', index: 'NumberOfStudents', width: 105, classes: 'pointer', editable: true },
            { name: 'instructors', index: 'Instructors', width: 150, classes: 'pointer', editable: true }
        ],
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: pager_selector,
        altRows: true
    }).navGrid('#jqGridPager',
        { add: false, edit: true, edittitle: "Edit Class", del: true, deltitle: "Delete Class", search: false, refresh: true },
        //Edit option
        {
            reloadAfterSubmit: true,
            jqModal: false,
            closeOnEscape: true,
            closeAfterEdit: true,
            editCaption: "The Edit Dialog",
            //template: template,
            url: "/Studio/EditClass/",
            afterSubmit: function (response, postdata) {
                if (response.statusText = "OK") {
                    jQuery("#success").show();
                    jQuery("#success").html("Class successfully updated");
                    jQuery("#success").fadeOut(6000);
                    return [true, response.responseText]
                }
                else {
                    return [false, response.responseText]
                }
            }
        },
        {},//add option Don't delete it!
        //Delete option 
        {
            url: '/Studio/DeleteClass',
            closeAfterDelete: true,
            afterSubmit: function (response) {
                if (response.statusText = "OK") {
                    jQuery("#success").show();
                    jQuery("#success").html("Class successfully deleted");
                    jQuery("#success").fadeOut(6000);
                    return [true, response.responseText]
                }
                else {
                    return [false, response.responseText]
                }
            }
        });
};

