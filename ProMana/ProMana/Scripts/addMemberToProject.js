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

        if (roleId != 1) {
            $("#listMember" + roleId).append("<div id='row" + userName + "'></div>");
            $("#row" + userName).append("<label class='control-label'>" +
                $('#user option:selected').text() + " - "
                + $('#role  option:selected').text() + "</label>");
            $("#row" + userName).append('<button class="btn btn-danger pull-right" onclick="RemoveMember(\'' + userName + '\')"><i class="fas fa-user-times"></i></button>');
        }
        ResetSelect();
        $('#memberModal').modal('hide');
    }
    else {
        return false;
    }
}

function SelectMember(membersJson) {
    var members = JSON.parse(membersJson);
    for (var i = 0; i < members.length; i++) {
        var roleInProject = new RoleInProjectViewModel(members[i].Username, members[i].RoleId);
        listMemeberAndRole.push(roleInProject);
        listUserSelected.push(members[i].Username);

        if (members[i].RoleId!=1) {
            $("#listMember" + members[i].RoleId).append("<div id='row" + members[i].Username + "'></div>");
            $("#row" + members[i].Username).append("<label class='control-label'>" +
                members[i].FullName + "(" + members[i].Username + ") - "
                + members[i].RoleTitle + "</label>");
            if (members[i].RoleId!=2) {
                $("#row" + members[i].Username).append('<button class="btn btn-danger pull-right" onclick="RemoveMember(\'' + members[i].Username + '\')"><i class="fas fa-user-times"></i></button>');
            }
            $("#row" + members[i].Username).append("<hr/>");
        }
    }
    ResetSelect();
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