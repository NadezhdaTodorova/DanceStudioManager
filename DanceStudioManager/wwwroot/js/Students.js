

$(document).ready(function () {

    var grid_selector = "#jqGrid";
    var pager_selector = "#jqGridPager";

    jQuery(grid_selector).jqGrid({
        url: '/Studio/GetStudents',
        datatype: "json",
        height: 450,
        colNames: ['Firstname', 'Lastname', 'Email', 'CellPhone', 'Gender'],
        colModel: [
            {id:10, name: 'firstname', index: 'Firstname', width: 200, firstsortorder: "desc"},
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
        loadonce: true
    }).navGrid('#jqGridPager', {add: false, edit: true, del: true, search: false, refresh: true });


    $("#DateOfBirth").datepicker({ autoclose: true, todayBtn: 'linked' })

    $("#SubmitStudents").click(function () {
        searchStudents();
    });

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

    function searchStudents() {
        var formDataSearch = getMyAppsDataFromScreen();
       
    }

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
});

    