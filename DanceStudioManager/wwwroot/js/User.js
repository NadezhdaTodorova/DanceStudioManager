

$("#Email").on("change", validate);

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