var monthsShort = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
var users = [];
var activeConferenceId;
var lastConferenceId;
var lastMessageId;
var updating = false;

$(document).ready(function (e) {
    $('#emoBtn').click(function () {
        $('#modal').show();
    });

    $('#source').click(function (e) { //Offset mouse Position
        var coords = {
            x: e.pageX - $(this).offset().left,
            y: e.pageY - $(this).offset().top
        };
        $('#messageText').val($('#messageText').val() + EmoticonHandler.emoticons[EmoticonHandler.getIndex(coords)]);
        $('#modal').hide();
    });

    $('#sendBtn').click(function (e) {
        var message = createNewMessage();
        debugger;
        $.ajax({
            url: "/api/messages",
            type: "POST",
            data: JSON.stringify(message),
            contentType: "application/json; charset=utf-8",
            success: function () {
                updateMessages();
                $('.local-message').remove();
            }
        });
    });

    $(document).on('click', '.conference', function () {
        var numId = this.id.substring(3);
        if (numId != activeConferenceId) {
            setActiveConference(numId);
        }
    });

    getConferences(function () {
            if (activeConferenceId) {
                setActiveConference(activeConferenceId);
            }
            updateMessages;
        }
    );
});

var createNewMessage = function () {
    var rawText = $('#messageText').val();
    var message = {
        UserId: getUserId(),
        Text: rawText,
        ConferenceId: activeConferenceId
    };
    var now = new Date(); //TODO! local
    var tag = "<div class=\"message local-message\">" + "<div class=\"message-header\">"
    "<div class=\"message-author\">Me</div>" + "<div class=\"message-time\">" + getTime(now) + "</div></div>" +
    "<div class=\"simple-message lighty\">" + '<img class="avatar" border="0" src="/Content/Img/no-picture.png">' +
      '<span class="message-text">' + Parser.parse(message.Text) + '</span></div>';
    $(".message-feed").append(tag);
    getColorScheme();
    $('#messageText').val("");
    return message;
};

var updateMessages = function () {
    if (activeConferenceId) {
        if (lastMessageId) {
            var messagesUrl = "/api/conferences/" + activeConferenceId + "/messages/" + lastMessageId;
        } else {
            var messagesUrl = "/api/conferences/" + activeConferenceId + "/messages";
        }
        return $.ajax({
            dataType: "json",
            url: messagesUrl,
            success: function (messages) {
                for (var i = 0; i < messages.length; i++) {
                    var message = messages[i];
                    if (users.indexOf(message.UserId) == -1) {
                        users.push(message.UserId);
                    }
                    var messageTag = "<div class=\"message" + (message.UserId == getUserId() ? " own-message" : " incoming-message") + "\" id=\"MSG" + message.Id +
                        "\">" + "<div class=\"message-header\"> " + " <div class=\"message-author\">User:" + message.UserId +
                        "</div> " + "<div class=\"message-time\">" + getTime(new Date(message.Date)) + "</div></div>" +
                        "<div class=\"simple-message" + (message.UserId == getUserId() ? " lighty" : " peachy") + "\">" +
                        '<img class="avatar" border="0" src="/Content/Img/no-picture.png">' + '<span class="message-text">' +
                        Parser.parse(message.Text) + "</span></div>";
                    $(".message-feed").append(messageTag);
                    $("#MSG" + message.Id).each(function () {
                        var messageId = message.Id;
                        var messageUserId = message.UserId;
                        $.ajax({
                            dataType: "json",
                            url: '/api/users/' + messageUserId,
                            success: function (user) {
                                $('#MSG' + messageId + ' .message-author').html(user.FirstName + ' ' + user.LastName);
                                if (user.Avatar) {
                                    $('#MSG' + messageId + ' .avatar').attr('src', user.Avatar);
                                }
                            },
                        });
                    });
                    var messageId = message.Id;
                    var imgLink = $("#MSG" + messageId + " a");
                    if (imgLink) {
                        var imgUrl = imgLink.attr("href");
                        if (imgUrl) {
                            $.ajax({
                                method: "HEAD",
                                url: imgUrl,
                                success: function (data, textStatus, request) {
                                    if (request.getResponseHeader('content-type').startsWith("image/")) {
                                        $(imgLink).html('<img border="0" src="' + imgUrl + '" width="100" height="100">');
                                    }
                                }
                            });
                        }
                    }
                    lastMessageId = message.Id;
                }
                getColorScheme();
            },
        });
    }
};

var getUserId = function () {
    var userId = $.cookie('userId');
    if (userId) {
        return userId;
    } else {
        window.location.replace('/Main/Index');
    }
}

var setActiveConference = function (id) {
    $('#CNF' + id + ' .simple-conference').removeClass("lighty").addClass("darky");
    $('#CNF' + id + ' .simple-conference').css({ 'cursor': 'default' });
    $('.conference[id!="CNF' + id + '"] .simple-conference').removeClass("darky").addClass("lighty");
    $('.conference[id!="CNF' + id + '"] .simple-conference').css({ 'cursor': 'pointer' });
    getColorScheme();
    activeConferenceId = id;
    $('.message').remove();
    lastMessageId = null;
    updateMessages();
}

var getConferences = function (callback) {
    updating = true;
    var conferenceUrl = "/api/users/" + getUserId() + "/conferences";
    if (lastConferenceId) {
        conferenceUrl = "/api/users/" + getUserId() + "/conferences/" + lastConferenceId;
    }
    return $.ajax({
        dataType: "json",
        url: conferenceUrl,
        success: function (conferences) {
            for (var i = 0; i < conferences.length; i++) {
                var conference = conferences[i];
                var conferenceTag = '<div class="conference" id="CNF' + conference.Id +
                    '"> <div class="simple-conference">' +
                    '<img class="conference-picture" src="' + (conference.Photo ? conference.Photo : "/Content/Img/dialog.png") + '">' +
                    '<span class="conference-title">' + conference.Title + '</span></div> </div>';
                $(".conference-container").append(conferenceTag);
                if (!lastConferenceId || conference.Id > lastConferenceId) {
                    lastConferenceId = conference.Id;
                }
                var otherUser;
                if (conference.Members.length == 2) {
                    if (conference.Members[0].Id == getUserId()) {
                        otherUser = conference.Members[1];
                    } else {
                        otherUser = conference.Members[0];
                    }
                } else if (conference.Members.length > 2) {
                    $('.conference[id="CNF' + conference.Id + '"] .conference-title').html('Conference (' + conference.Members.length + ' participants)');
                }
                if (otherUser) {
                    $('.conference[id="CNF' + conference.Id + '"] .conference-title').html('Chat with ' + otherUser.FirstName + ' ' + otherUser.LastName);
                }
                if (otherUser.Avatar) {
                    $('.conference[id="CNF' + conference.Id + '"] .conference-picture').attr('src', otherUser.Avatar);
                }
            }
            if (conferences.length > 0) {
                activeConferenceId = conferences[0].Id;
            }
            callback();
        },
        complete: function () {
            updating = false;
        }
    });
};

var getTime = function (time) {
    var now = new Date();
    if (now.getFullYear() != time.getFullYear()) {
        return "year " + time.getFullYear();
    }
    if (now.getMonth() != time.getMonth()) {
        return monthsShort[time.getMonth()];
    }
    if (now.getDate() != time.getDate()) {
        return monthsShort[time.getMonth()] + time.getDate();
    }
    return stringPad(time.getHours()) + ":" + stringPad(time.getMinutes()) + ":" + stringPad(time.getSeconds());
}

var stringPad = function (n) {
    if (n < 10) {
        return "0" + n;
    } else {
        return n;
    }
}

setInterval(function () {
    if (!updating) {
        getConferences(updateMessages);
    }
}, 10000);