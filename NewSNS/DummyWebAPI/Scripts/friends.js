$(document).ready(function (e) {

    $(document).on('click', '.btn-conversation', function () {
        var numId = $(this).closest('.friend').attr('id').substring(3);
        var createConfUrl = '/api/conferences?firstUser=' + getUserId() + '&secondUser=' + numId + '&title=chat';
        $.post(createConfUrl, function() {
                window.location.replace('/');
        });
    });
    getFriends();
});

var getFriends = function () {
    var friendsUrl = '/api/friends/' + getUserId();
    return $.ajax({
        dataType: "json",
        url: friendsUrl,
        success: function (friends) {
            for (var i = 0; i < friends.length; i++) {
                var friend = friends[i];
                var iconUrl = (friend.Avatar ? friend.Avatar : '/Content/Img/no-picture.png');
                var friendTag = '<div class="friend" id="FRN' + friend.Id +
                    '"> <div class="simple-friend lighty">' +
                    '<img class="avatar" border="0" src="' + iconUrl + '"><span class="full-name">' +
                    friend.FirstName + ' ' + friend.LastName +
                    '</span><span class="btn-conversation custom-btn darky"><i class="fa fa-commenting"></i></span></div></div>';
                $(".friends-container").append(friendTag);
            }
            getColorScheme();
        },
    });
}

var getUserId = function () {
    var userId = $.cookie('userId');
    if (userId) {
        return userId;
    } else {
        window.location.replace('/Main/Index');
    }
}