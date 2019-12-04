

$(document).ready(function () {
    var firstname = '';
    var lastname = '';
    var email = '';

    createGrid(firstname, lastname, email);

    $("#DateOfBirthToString").datepicker({ autoclose: true, todayBtn: 'linked' })

    function validateEmail(email) {
        var re = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
        return re.test(email);
    }

    function validate() {
        var $result = $("#result");
        var email = $("#Email2").val();
        $result.text("");

        if (validateEmail(email)) {
            $result.hide();
        } else {
            $result.text("Please enter a valid email!");
            $result.css("color", "red");
            $result.show();
        }
        return false;
    }

    $("#Email2").on("change", validate);

});

var grid_selector = "#jqGrid";
var pager_selector = "#jqGridPager";

$("#SubmitStudents").click(function (e) {
    e.preventDefault();
    var formDataSearch = getMyAppsDataFromScreen();
    var $grid1 = $(grid_selector);

    $grid1.jqGrid('clearGridData').jqGrid('setGridParam',
        {
            datatype: 'json',
            url: '/Studio/GetStudents?firstname=' + formDataSearch.firstname + '&lastname=' + formDataSearch.lastname + '&email=' + formDataSearch.email,
            search: false
        }).trigger("reloadGrid");

});

function getMyAppsDataFromScreen() {
    var formDataSearch = {};

    var firstname = $('#Firstname').val();
    if (!firstname || firstname.length === 0)
        formDataSearch.firstname = null;
    else
        formDataSearch.firstname = firstname;

    var lastname = $('#Lastname').val();
    if (!lastname || lastname.length === 0)
        formDataSearch.lastname = null;
    else
        formDataSearch.lastname = lastname;

    var email = $('#Email').val();
    if (!email || email.length === 0)
        formDataSearch.email = null;
    else
        formDataSearch.email = email;

    return formDataSearch;
}

function createGrid(firstname, lastname, email) {
    jQuery(grid_selector).jqGrid({
        url: '/Studio/GetStudents?firstname=' + firstname + '&lastname=' + lastname + '&email=' + email,
        datatype: "json",
        height: "100%",
        type: "POST",
        colNames: ['Firstname', 'Lastname', 'Email', 'CellPhone', 'Gender', 'Date of birth'],
        colModel: [
            { name: 'firstname', index: 'Firstname', width: 200,  editable: true, sortable:false, classes: 'pointer' },
            { name: 'lastname', index: 'Lastname', width: 200, editable: true, classes: 'pointer', sortable: false, },
            { name: 'email', index: 'Email', width: 200, editable: true, classes: 'pointer', sortable: false, },
            { name: 'cellPhone', index: 'CellPhone', width: 200, editable: true, classes: 'pointer', sortable: false, },
            { name: 'gender', index: 'Gender', width: 150, editable: true, classes: 'pointer', sortable: false, },
            { name: 'dateOfBirthToString', index: 'DateOfBirthToString', width: 250, editable: true, classes: 'pointer', sorttype: 'date', sortable: false, }
        ],
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: pager_selector,
        altRows: true,
        sortorder: "desc",
        loadonce: true,
        navOptions: { reloadGridOptions: { fromServer: true } },
        
    }).navGrid('#jqGridPager',
        { add: false, edit: true, edittitle:"Edit Student",  del: true, deltitle: "Delete Student", search: false, refresh: false },
        //Edit option
        {
            reloadAfterSubmit: true,
            jqModal: false,
            closeOnEscape: true,
            closeAfterEdit: true,
            url: "/Studio/EditStudent/",
            afterSubmit: function (response, postdata) {
                if (response.statusText = "OK") {
                    $(this).jqGrid("setGridParam", { datatype: 'json' });
                    jQuery("#success").show();
                    jQuery("#success").html("Student successfully updated");
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
            url: '/Studio/DeleteStudent',
            closeAfterDelete: true,
            afterSubmit: function (response) {
                if (response.statusText = "OK") {
                    $(this).jqGrid("setGridParam", { datatype: 'json' });
                    jQuery("#success").show();
                    jQuery("#success").html("Student successfully deleted");
                    jQuery("#success").fadeOut(6000);
                    return [true, response.responseText]
                }
                else {
                    return [false, response.responseText]
                }
            }

        });
};
