
function UserRolesDropdown(id) {
    var roleBox = $('#role-' + id);
    var currentRole = roleBox.text();

    $.ajax({
        method: 'GET',
        url: 'http://localhost:51202/UserRoles',
        success: function (roles) {
            roleBox.empty();
            roleBox.append('<select class="form-control" id="RoleList-' + id + '"></select>');

            $.each(roles, function (index, role) {
                if (currentRole == role.name) {
                    $("#RoleList-" + id).append('<option ' + 'value="' + role.name + '" ' + 'selected="selected">' + role.name + '</option>')
                } else {
                    $("#RoleList-" + id).append('<option ' + 'value="' + role.name + '" ' + '>' + role.name + '</option>')
                }
            })

            $("#Edit-" + id).text("Save").attr('onclick', 'SaveUserRole(' + id + ')');
        },
        error: function () {
            alert("Error Occured in web service");
        }
    })

};

function SaveUserRole(id) {
    var selectedRole = $('#RoleList-' + id + ' option:checked').val();
    var selectedUsername = $('#username-' + id).text();
    var roleBox = $('#role-' + id);

    $.ajax({
        method: "PUT",
        url: 'http://localhost:51202/User/' + selectedUsername + '/Role/' + selectedRole,
        success: function () {
            $("#Edit-" + id).text("Edit").attr('onclick', 'UserRolesDropdown(' + id + ')');
            roleBox.empty();
            roleBox.text(selectedRole);
        },
        error: function () {
            alert("Failed to update through web service");
        }
    })
}

function DeleteUser(id) {
    if (confirm("Are you sure you want to delete the user?")) {
        var row = $('#userrow-' + id);
        var selectedUsername = $('#username-' + id).text();

        $.ajax({
            type: "DELETE",
            url: 'http://localhost:51202/User/' + selectedUsername + '/',
            success: function (data) {
                location.reload();
            },
            error: function () {
                alert("Failed to delete user through web service");
            }
        })
    }
}