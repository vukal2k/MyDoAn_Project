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
            var phay = "'";
            $("#listMember" + roleId).append("<div id='row" + userName + "'></div>");
            $("#row" + userName).append("<label class='control-label'>" +
                $('#user option:selected').text() + " - "
                + $('#role  option:selected').text() + "</label>");
            $("#row" + userName).append('<a type="button" class="btn btn-danger pull-right" onclick="RemoveMember(' + phay + '' + userName + '' + phay + ',' + roleId + ')"><i class="fas fa-user-times"></i></a>');
            $("#row" + userName).append("<hr/>");
        }
        ResetSelect();
        $('#memberModal').modal('hide');
    }
    else {
        return false;
    }
}

function InitListMember(listMember) {
    var roleInProject;
    for (var i = 0; i < listMember.length; i++) {
        if (listMember[i].RoleId != 2) {
            roleInProject = new RoleInProjectViewModel(listMember[i].Username, listMember[i].RoleId);
            listMemeberAndRole.push(roleInProject);
            listUserSelected.push(listMember[i].Username);
        }
    }
    ResetSelect();
}

function AddMember() {
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
        //post data
        $.ajax({
            url: "../../Module/AddMember?username=" + userName + "&roleId=" + roleId,
            cache: false,
            processData: false,
            contentType: false,
            type: 'GET',
            data: {
                "username": userName
            },
            success: function (result) {
                if (result !== "0") {
                    $("#listMember").append(result);
                    ResetSelect();
                    $('#memberModal').modal('hide');
                } else {
                    $.notify("Failed!", "error");
                }
            }
        });
    }
    else {
        return false;
    }
}

function AddWatcher() {
    var isValid = true;
    var userName = $('#user').val();
    if (userName === "0") {
        $('#user').parent().addClass('has-error');
        isValid = false;
    } else {
        $('#user').parent().removeClass('has-error');
    }

    if (isValid === true) {
        var roleInProject = new RoleInProjectViewModel(userName, "3");
        listMemeberAndRole.push(roleInProject);
        listUserSelected.push(userName);

        //post data
        $.ajax({
            url: "../../Project/AddWatcher?username=" + userName,
            cache: false,
            processData: false,
            contentType: false,
            type: 'GET',
            data: {
                "username": userName
            },
            success: function (result) {
                if (result !== "0") {
                    $("#listMember").append(result);
                    ResetSelect();
                    $('#memberModal').modal('hide');
                } else {
                    $.notify("Failed!", "error");
                }
            }
        });
    }
    else {
        return false;
    }
}

function SelectMember(membersJson) {
    var phay = "'";
    var members = JSON.parse(membersJson);
    for (var i = 0; i < members.length; i++) {
        var roleInProject = new RoleInProjectViewModel(members[i].Username, members[i].RoleId);
        listMemeberAndRole.push(roleInProject);
        if (members[i].RoleId != 1 && members[i].RoleId != 2) {
            listUserSelected.push(members[i].Username);
        }

        if (members[i].RoleId!=1) {
            $("#listMember" + members[i].RoleId).append("<div id='row" + members[i].Username + "'></div>");
            $("#row" + members[i].Username).append("<label class='control-label'>" +
                members[i].FullName + "(" + members[i].Username + ") - "
                + members[i].RoleTitle + "</label>");
            if (members[i].RoleId!=2) {
                $("#row" + members[i].Username).append('<a type="button" class="btn btn-danger pull-right" onclick="RemoveMember(' + phay + '' + members[i].Username + '' + phay + ', ' + members[i].RoleId + ')"><i class="fas fa-user-times"></i></a>');
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

function RemoveMember(userName,roleId) {
    $("#row" + userName).remove();

    var memberRemoveIndex = listUserSelected.indexOf(userName);
    listUserSelected.splice(memberRemoveIndex, 1);

    for (var i = 0; i < listMemeberAndRole.length; i++) {
        if (listMemeberAndRole[i].username == userName && listMemeberAndRole[i].roleId == roleId) {
            listMemeberAndRole.splice(i, 1);
            ResetSelect();
            return;
        }
    }
}

function RemoveWatcher(userName) {
    $("#row" + userName).remove();

    var memberRemoveIndex = listUserSelected.indexOf(userName);
    listUserSelected.splice(memberRemoveIndex, 1);

    for (var i = 0; i < listMemeberAndRole.length; i++) {
        if (listMemeberAndRole[i].username == userName) {
            listMemeberAndRole.splice(i, 1);
            ResetSelect();
            return;
        }
    }
}