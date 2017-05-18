var currentUser;
//var friends;
var friendIds;

var getUserId = function () {
    var userId = $.cookie('userId');
    if (userId) {
        return userId;
    } else {
        window.location.replace('/Main/Index');
    }
};

$.ajax({
    type: 'GET',
    url: '/api/users/' + getUserId(),
    success: function (user) {
        currentUser = user;
    },
    error: function () {
        window.location.replace('/Main/Index');
    },
    async: false
});

$(document).ready(function (e) {


    $('#showUploadModalBtn').click(function () {
        $("#pictureModal").modal("show");
    });

    var files;
    function handleFileSelect(e) {
        files = e.target.files;
    }
    $('.file-input').on('change', handleFileSelect);
    $('#uploadBtn').click(function () {
        if (files) {
            formData = new FormData();
            formData.append("avatar", files[0]);
            $.ajax({
                url: '/api/files/avatar?userId=' + getUserId(),
                type: 'POST',
                xhr: function () {
                    var myXhr = $.ajaxSettings.xhr();
                    return myXhr;
                },
                success: function () {
                    getUserIcon();
                },
                error: function() {
                    debugger;
                },
                data: formData,
                cache: false,
                contentType: false,
                processData: false
            });
            $("#pictureModal").modal("hide");
        }
    });

    $('#showMatchesBtn').click(function () {
        var name = $('#users').val();
        if (name) {
            $.when(getMatchingUsers(name)).done(function () {
                $("#befriendModal").modal("show");
            });
        }
    });

    $('#befriendModal').on('hide.bs.modal', function (e) {
        $('.friend-icon-modal').remove();
    });

    $(document).on('click', '.befriend-btn:not(.disabled)', function () {
        var numId = $(this).closest('.friend-icon-modal').attr('id').substring(3);
        // TODO: implement follow functionality instead of this workaround
        var followUrl = '/api/follows?firstUser=' + getUserId() + '&secondUser=' + numId;
        $.ajax({
            type: 'PUT',
            url: followUrl,
            success: function () {
                var befriendUrl = '/api/friends?firstUser=' + getUserId() + '&secondUser=' + numId;
                $.ajax({
                    type: 'PUT',
                    url: befriendUrl,
                    success: function () {
                        getFriends(populateFriendIcons);
                    }
                });
                $("#befriendModal").modal("hide");
            }
        });
    });

    $(function () {
        var userNames = [];
        var getUsers = function () {
            return $.ajax({
                type: 'GET',
                dataType: "json",
                url: '/api/users',
                success: function (users) {
                    for (var i = 0; i < users.length; i++) {
                        var firstName = users[i].FirstName;
                        if (userNames.indexOf(firstName) == -1) {
                            userNames.push(firstName);
                        }
                        var lastName = users[i].LastName;
                        if (userNames.indexOf(lastName) == -1) {
                            userNames.push(lastName);
                        }
                    }
                }
            });
        };
        $.when(getUsers()).then(function () {
            $("#users").typeahead({
                source: userNames
            });
        });
    });

    $('.full-name').append('<h3>' + currentUser.FirstName + ' ' + currentUser.LastName + '</h3>');
    showPersonalInfo();
    $('#showUpdateBtn').click(function () {
        $('#countryInput').val($('#country').html());
        $('#cityInput').val($('#city').html());
        $('#phoneInput').val($('#phone').html());
        $('#statusInput').val($('#status').html());
        $('#infoInput').val($('#info').html());

        $("#infoModal").modal("show");
    });
    $('#updateBtn').click(function () {
        currentUser.Country = $('#countryInput').val();
        currentUser.City = $('#cityInput').val();
        currentUser.Phone = $('#phoneInput').val();
        currentUser.Info = $('#infoInput').val();
        $.ajax({
            type: 'PUT',
            contentType: "application/json; charset=utf-8",
            url: '/api/users/' + getUserId(),
            data: JSON.stringify(currentUser),
            success: function () {
                $.when(getCurrentUser()).then(showPersonalInfo());
            },
            error: function (jqXHR, textStatus, errorThrown){
                debugger;
            }
        });
        //TODO
        $("#infoModal").modal("hide");
    });
    getUserIcon();
    getFriends(populateFriendIcons)
});

var getCurrentUser = function () {
    return $.ajax({
        type: 'GET',
        url: '/api/users/' + getUserId(),
        success: function (user) {
            currentUser = user;
        },
        error: function () {
            window.location.replace('/Main/Index');
        }
    });
};

var showPersonalInfo = function () {
    $('#birthDate').html(getDate(currentUser.BirthDate));
    $('#birthDateInput').attr('placeholder', currentUser.BirthDate);
    $('#email').html(currentUser.Email);
    $('#emailInput').attr('placeholder', currentUser.Email);
    $('#country').html(currentUser.Country);
    $('#countryInput').attr('placeholder', currentUser.Country);
    $('#city').html(currentUser.City);
    $('#cityInput').attr('placeholder', currentUser.City);
    $('#phone').html(currentUser.Phone);
    $('#phoneInput').attr('placeholder', currentUser.Phone);
    $('#status').html(getState(currentUser.UserState));
    $('#statusInput').attr('placeholder', currentUser.UserState);
    $('#info').html(currentUser.Info);
    $('#infoInput').attr('placeholder', currentUser.Info);
}

var getFriends = function (callback) {
    var friendsUrl = "/api/friends/" + getUserId();
    return $.ajax({
        dataType: "json",
        url: friendsUrl,
        success: function (data) {
            callback(data);
        },
        error: function () {
            debugger;
        }
    });
}

var extractFriendIds = function (friends) {
    if (!friendIds) {
        friendIds = [];
    }
    for (var i = 0; i < friends.length; i++) {
        friendIds.push(friends[i].Id);
    }
}

var populateFriendIcons = function (friends) {
    $('.friend-icon').remove();
    for (var i = 0; i < friends.length; i++) {
        var friend = friends[i];
        var friendTag = '<div class="friend-icon lighty" id="FRD' + friend.Id +
            '">' + '<img class="friend-avatar" border="0" src="/Content/Img/no-picture.png">' +
            '<span class="friend-name">' + friend.FirstName + " " + friend.LastName + '</span></div>';
        $(".friends-icons-container").append(friendTag);
        getFriendIcon(friend.Id);
    }
    getColorScheme();
}

var getFriendIcon = function (friendId) {
    var friendIconUrl = "/api/files/avatar?userId=" + friendId;
    return $.ajax({
        type: 'GET',
        url: friendIconUrl,
        success: function () {
            $('#FRD' + friendId + ' img').attr("src", friendIconUrl);
        }
    });
};

var getUserIcon = function () {
    var userIconUrl = "/api/files/avatar?userId=" + getUserId();
    return $.ajax({
        type: 'GET',
        url: userIconUrl,
        success: function () {
            $('.avatar').attr("src", userIconUrl);
        }
    });
};

var getMatchingUsers = function (name) {
    var usersUrl = "/api/users/byName?name=" + name;
    return $.ajax({
        type: 'GET',
        dataType: "json",
        url: usersUrl,
        success: function (users) {
            $(".friend-picker").remove();
            $.when(getFriends(extractFriendIds)).done(function () {
                for (var i = 0; i < users.length; i++) {
                    if (users[i].Id != getUserId()) {
                        var alreadyFriend = friendIds.indexOf(users[i].Id) != -1;
                        var matchTag = '<div class="friend-icon friend-icon-modal lighty' + (alreadyFriend ? ' disabled' : '') + '" id="USR' + users[i].Id +
                                '">' + '<img class="friend-avatar" src="' + (users[i].Avatar ? users[i].Avatar : '/Content/Img/no-picture.png') + '">' +
                                '<span class="friend-name">' + users[i].FirstName + " " + users[i].LastName + ", born: " + getDate(users[i].BirthDate) +
                                ", lives in " + users[i].City + ", " + users[i].Country + '</span>' +
                                '<span class="befriend-btn custom-btn darky' + (alreadyFriend ? ' inactive' : '') + '">Befriend</span></div>';
                        $("#matches").append(matchTag);
                    }
                }
                getColorScheme();
            });
        }
    });
}

var getDate = function (dateStr) {
    return dateStr.slice(8, 10) + '.' + dateStr.slice(5, 7) + '.' + dateStr.slice(0, 4);
}

var getState = function (statusCode) {
    switch (statusCode) {
        case 0:
            return 'active';
        case 1:
            return 'blocked';
        case 2:
            return 'deleted';
        default:
            return 'offline';
    }
}