var getMarsScheme = function () {
    $('.darky').css('background-color', '#930');
    $('.lighty').css('background-color', '#fb8');
    $('.peachy').css('background-color', 'peachpuff');
    $('.darky-text').css('color', '#930');
    $('.btn-primary').removeClass('btn-primary').addClass('btn-danger');
    $('.btn-default').removeClass('btn-default').addClass('btn-danger');
};

var getFrozenScheme = function () {
    $('.darky').css('background-color', '#33f');
    $('.lighty').css('background-color', '#68f');
    $('.peachy').css('background-color', '#bbf');
    $('.darky-text').css('color', '#33f');
    $('.btn-danger').removeClass('btn-danger').addClass('btn-primary');
    $('.btn-default').removeClass('btn-default').addClass('btn-primary');
};

var getColorScheme = function () {
    if ($.cookie('scheme') == 'frozen') {
        getFrozenScheme();
    } else if ($.cookie('scheme')) {
        getMarsScheme();
    } else {
        getMarsScheme();
        $.cookie('scheme', 'mars', {
            expires: 10
        });
    }
}

$(document).ready(function (e) {
    getColorScheme();
    $(document).on('click', '.log-out-link', function () {
        $.cookie('userId', null);
        window.location.replace('/');
    });
    $(document).on('click', '.settings-link', function () {
        $("#settingsModal").modal("show");
    });
    $(document).on('click', '#selectBtn', function () {
        var scheme = $('input[name=scheme]:checked').val();
        if (scheme == 'mars') {
            getMarsScheme();
        } else {
            getFrozenScheme();
        }
        $.cookie('scheme', scheme, {
            expires: 10
        });
        $("#settingsModal").modal("hide");
    });
});
