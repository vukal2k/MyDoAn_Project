//function SelectContact() {
//    var ContactId = $('input[name="chk_contact"]').val()
//    var ContactName = $('#ContactName+id').text()
//    $('#ContactId').val(ContactId)
//    $('#ContactName').val(ContactName)
//    $('#contactLookUpModal').modal('hide')
//}


$(document).ready(() => {
    $('#table > tbody tr').click(function () {
        var row = $(this);
        row.find('input[type="radio"]').prop('checked', true)
    })
})

function ShowUpdateContactModal() {
    var contactId = $('input[name="chk_contact"]').val()
    $.ajax({
        url: '/Contact/UpdateContact',
        type: 'GET',
        dataType: "html",
        data: {
            'contactId' : contactId
        },
        success: function (response) {
            if (response == "failed") {
                $.notify("Update contact failed", "error");

            } else {
                $("#noiUpdateContactModalDungChan").html(response)
                $('#contactUpdateModal' + contactId).modal('show')
                UpdateContact(contactId)
            }
        }
    });
}

function UpdateContact(contactId) {
    $("<span id='FirstNameContactError" + contactId + "' style='color:red;'></span>").insertAfter("#FirstNameContact" + contactId);
    $("<span id='SurNameContactError" + contactId + "' style='color:red;'></span>").insertAfter("#SurNameContact" + contactId);
    $("<span id='id='SurNameContactError" + contactId + "' style='color:red;'></span>").insertAfter("#ContactTypeId" + contactId + ":parent");

    $("#formUpdateContact" + contactId).validate({
        onfocusout: false,
        onkeyup: false,
        onclick: false,
        submitHandler: function (form) {
            var isValid = true;
            if ($("#FirstNameContact" + contactId).val().trim() == "") {
                $("#FirstNameContact" + contactId).parent().addClass('has-error')
                $("#FirstNameContactError" + contactId).text("First Name is required!")
                isValid = false;
            }
            if ($("#SurNameContact" + contactId).val().trim() == "") {
                $("#SurNameContact" + contactId).parent().addClass('has-error')
                $("#SurNameContactError" + contactId).text("Surname is required!")
                isValid = false;
            }
            if ($("#ContactTypeIdNull" + contactId).prop('selected')) {
                $("#ContactTypeIdNull" + contactId).parent().parent().addClass('has-error')
                $("#SurNameContactError" + contactId).text("Contact Type is required!")
                isValid = false;
            }
            if (isValid == true) {
                $.ajax({
                    url: '/Contact/UpdateContact',
                    type: 'POST',
                    dataType: "html",
                    data: $(form).serialize(),
                    success: function (response) {
                        if (response == "failed") {
                            $.notify("Add contact failed", "error");

                        } else {
                            $("#FirstNameContactError").text("")
                            $("#SurNameContactError").text("")
                            $("#noiTableContactDungChan").html(response)
                            $("#SurNameContactError").text("")
                            SetDataTable();
                            $.notify("Update contact success", "success");
                        }
                    }
                });
            }
            return false;
        },
    });
}

$(document).ready(() => {
    $("<span id='FirstNameContactError' style='color:red;'></span>").insertAfter("#FirstNameContact");
    $("<span id='SurNameContactError' style='color:red;'></span>").insertAfter("#SurNameContact");
    $("<span id='id='SurNameContactError' style='color:red;'></span>").insertAfter("#ContactTypeId:parent");

    $("#formContact").validate({
        onfocusout: false,
        onkeyup: false,
        onclick: false,
        submitHandler: function (form) {
            var isValid = true;
            if ($("#FirstNameContact").val().trim() == "") {
                $("#FirstNameContact").parent().addClass('has-error')
                $("#FirstNameContactError").text("First Name is required!")
                isValid = false;
            }
            if ($("#SurNameContact").val().trim() == "") {
                $("#SurNameContact").parent().addClass('has-error')
                $("#SurNameContactError").text("Surname is required!")
                isValid = false;
            }
            if ($("#ContactTypeIdNull").prop('selected')) {
                $("#ContactTypeIdNull").parent().parent().addClass('has-error')
                $("#SurNameContactError").text("Contact Type is required!")
                isValid = false;
            }
            if (isValid == true) {
                $.ajax({
                    url: '/Contact/CreateContact',
                    type: 'POST',
                    dataType:"html",
                    data: $(form).serialize(),
                    success: function (response) {
                        if (response == "failed") {
                            $.notify("Add contact failed", "error");
                            
                        } else {
                            $("#FirstNameContactError").text("")
                            $("#SurNameContactError").text("")
                            $("#noiTableContactDungChan").html(response)
                            $("#SurNameContactError").text("")
                            SetDataTable();
                            $.notify("Add contact success", "success");
                        }
                    }
                });
            }
            return false;
        },
    });
})