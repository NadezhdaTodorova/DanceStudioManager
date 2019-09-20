

$(document).ready(function () {

    var grid_selector = "#jqGrid";
    var pager_selector = "#jqGridPager";

    $grid_selector = $("#jqGrid");

    jQuery(grid_selector).jqGrid({
        url: '/Studio/GetStudents',
        datatype: "json",
        height: 450,
        colNames: ['Firstname', 'Lastname', 'Email', 'CellPhone', 'Gender'],
        colModel: [
            { name: 'firstname', index: 'Firstname', width: 200, firstsortorder: "desc"},
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
    }).navGrid('#jqGridPager', { add: false, edit: true, del: true, search: false, refresh: true });

    //createGrid($grid_selector, "studentsGrid", [
    //    { name: 'firstname', index: 'Firstname', width: 200, firstsortorder: "desc" },
    //    { name: 'lastname', index: 'Lastname', width: 200 },
    //    { name: 'email', index: 'Email', width: 200 },
    //    { name: 'cellPhone', index: 'CellPhone', width: 250 },
    //    { name: 'gender', index: 'Gender', width: 250 }
    //]);


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

        refreshGrid($grid_selector,
             "SearchStudent",
            function (data) {
                if (data.length === $("#maxRowsAlert").data('maxrows')) $("#maxRowsAlert").show();
                else $("#maxRowsAlert").hide();
            }, JSON.stringify(formDataSearch));
       
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

    function refreshGrid(grid, url, successFunc, jsonData, type, isSpecificPage) {
        successFunc = successFunc || function (data) { };
        if (grid.length === 0) return;
        grid[0].grid.beginReq.call(grid[0]);
        jsonData = jsonData || "";
        type = type || "POST";
        $.ajax({
            type: type,
            url: url,
            data: jsonData,
            contentType: 'application/json',
            dataType: 'json',
            error: function (jqXHR, textStatus, errorThrown) {
                if (jqXHR.responseJSON && jqXHR.responseJSON.error)
                    grid.jqGrid("displayErrorMessage", "Error from server: " + jqXHR.responseJSON.error);
                else
                    grid.jqGrid("displayErrorMessage", "Error from server: " + errorThrown);
                grid[0].grid.endReq.call(grid[0]);
            },

            success: function (data) {
                successFunc(data);
                var pageNum = 1;

                if (isSpecificPage) {
                    pageNum = getSpecificPageForCurrentElementInMonthPlan(grid[0], data);
                }
                grid[0].grid.endReq.call(grid[0]);
                grid.jqGrid('clearGridData').jqGrid('setGridParam',
                    {
                        data: data,
                        datatype: "local",
                        search: false,
                        page: pageNum
                    }).trigger("reloadGrid");
            }
        });
    }

    function createGrid(grid, outerdiv, colModel, loadComplete, width, fixeddiv, resizable, colNames, title, multiselect, showFooter, gridComplete1) {
        loadComplete = loadComplete || function (event, ui) { };
        gridComplete1 = gridComplete1 || function () { };
        width = width || $("#" + outerdiv).width();
        colNames = colNames || $('#' + outerdiv).data("columns");
        resizable = resizable !== true;
        multiselect = multiselect === true;
        divResize = fixeddiv || outerdiv;
        showFooter = showFooter === true;

        grid.jqGrid({
            data: [],
            locale: "bg",
            datatype: "local",
            colNames: colNames,
            colModel: colModel,
            cmTemplate: { editable: true, autoResizable: true },
            rowNum: 10,
            iconSet: "fontAwesomeSolid",
            loadFilterDefaults: false,
            autoResizing: { compact: true },
            rowList: [5, 10, 20, "10000:All"],
            viewrecords: true,
            pager: true,
            rownumbers: true,
            sortname: "invdate",
            loadui: "block",
            sortorder: "desc",
            threeStateSort: true,
            sortIconsBeforeText: true,
            loadonce: true,
            loadComplete: loadComplete,
            caption: title,
            multiselect: multiselect,
            multiselectWidth: 50,
            footerrow: showFooter,
            gridComplete: function (event, ui) {
                $('[data-toggle="tooltip"]').tooltip({
                    animated: 'fade',
                    container: 'body'
                });
                gridComplete1();
            }
        }).setGridWidth(width);

        if (resizable) {
            $(window).bind('resize', function () {
                var newSize = $('#' + divResize).width();
                if (newSize === 0)
                    newSize = $('#' + outerdiv).width();
                if (newSize !== 0)
                    grid.setGridWidth(newSize);
            }).trigger('resize');
        }
    }
});

    