class RoleInProjectViewModel {
    constructor(username, roleId) {
        this.username = username;
        this.roleId = roleId
    }
}


var listMemeberAndRole = new Array();
var listUserSelected = new Array();
function AddProject()
{
    var isValid = true;
    var userName = $('#user').val();
    if (userName === "0") {
        $('#user').parent().addClass('has-error');
        isValid = false;
    } else {
        $('#user').parent().removeClass('has-error');
    }

    var roleId = $('#role').val();
    if (roleId === "0") {
        $('#role').parent().addClass("has-error");
        isValid = false;
    } else {
        $('#role').parent().removeClass("has-error");
    }

    if (isValid === true) {
        var roleInProject = new RoleInProjectViewModel(userName, roleId);
        listMemeberAndRole.push(roleInProject);
        listUserSelected.push(userName);

        $("#listMember").append("<div id='row" + userName + "'></div>");
        $("#row" + userName).append("<label class='control-label'>" +
                                        $('#user option:selected').text() + " - "
            + $('#role  option:selected').text() + "</label>");
        $("#row" + userName).append('<button class="btn btn-danger pull-right" onclick="RemoveMember(\'' + userName + '\')"><i class="fas fa-user-times"></i></button>');
        $("#row" + userName).append("<hr/>");
        ResetSelect();
        $('#memberModal').modal('hide');
    }
    else {
        return false;
    }
}

function ResetSelect() {
    $.each($('#user option'), function (index, item) {
        if (listUserSelected.indexOf($(item).val()) === -1) {
            $(item).prop('disabled', false);
        } else {
            $(item).prop('disabled', true);
        }

        $(item).prop('selected', false);
    })
}

function RemoveMember(userName) {
    $("#row" + userName).remove();

    var memberRemoveIndex = listUserSelected.indexOf(userName);
    listUserSelected.splice(memberRemoveIndex, 1);

    for (var i = 0; i < listMemeberAndRole.length; i++) {
        if (listMemeberAndRole[i].username === userName) {
            listMemeberAndRole.splice(i, 1);
            ResetSelect();
            return;
        }
    }
}