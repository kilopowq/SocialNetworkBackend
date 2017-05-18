$(document).ready(function (e) {
    $('#logInBtn').click(function () {
        var login = $('#login').val();
        var pswd = $('#pswd').val();
        $.ajax({
            url: "/api/account",
            type: "GET",
            data: { login: login, password: pswd },
            success: function (user) {
                $.cookie('userId', user.Id, {
                    expires: 1
                });
                window.location.replace('/Main/Index');
            },
            error: function () {
                debugger;
            }
        });
    });


    $('#signUpBtn').click(function () {
        $("#signUpModal").modal("show");
    });
    $('#registerBtn').click(function () {
        var currentUser = {};
        currentUser.FirstName = $('#firstNameInput').val();
        currentUser.LastName = $('#lastNameInput').val();
        currentUser.Login = $('#loginInput').val();
        currentUser.Password = $('#passwordInput').val();
        currentUser.BirthDate = $('#birthDateInput').val();
        currentUser.Email = $('#emailInput').val();
        currentUser.Country = $('#countryInput').val();
        currentUser.City = $('#cityInput').val();
        currentUser.Phone = $('#phoneInput').val();
        currentUser.UserState = $('#statusInput').val();
        currentUser.Info = $('#infoInput').val();
        $.ajax({
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            url: '/api/account',
            data: JSON.stringify(currentUser),
            success: function (savedUser) {
                if (savedUser) {
                    $.ajax({
                        url: "/api/users/byLogin?login=" + savedUser.Login,
                        type: "GET",
                        success: function (savedUser) {
                            var savedUserId = savedUser.Id;
                            $.cookie('userId', savedUserId, {
                                expires: 1
                            });
                            window.location.replace('/Main/Index');
                        }
                    });
                }
                debugger;
            },
            error: function (jqXHR, textStatus, errorThrown) {
                debugger;
                return;
            }
        });
        $("#signUpModal").modal("hide");
    });
});