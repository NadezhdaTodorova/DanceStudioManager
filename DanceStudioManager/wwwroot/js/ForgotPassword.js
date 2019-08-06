
$(document).ready(function () {
    function validatePassword() {
        data = $('#Password').val();
        var len = data.length;
        var $result = $("#passRes");

        if (len < 8) {
            $result.text("Password must be at least 8 characters!");
            $result.css("color", "red");
            $result.show();
            // Prevent form submission
            event.preventDefault();
        } else {
            $result.hide();
        }

    }

    function validateConfPassword() {
        var $result = $("#confPassRes");
        if ($('#Password').val() != $('#ConfirmPassword').val()) {
            $result.text("Password and confirm password don't match!");
            $result.css("color", "red");
            $result.show();
            // Prevent form submission
            event.preventDefault();
        } else {
            $result.hide();
        }
    }

    function validateEmail(email) {
        var re = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
        return re.test(email);
    }

    function validate() {
        var $result = $("#result");
        var email = $("#Email").val();
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

    $("#Email").on("change", validate);
    $("#ConfirmPassword").on("change", validateConfPassword);
    $("#Password").on("change", validatePassword);

});