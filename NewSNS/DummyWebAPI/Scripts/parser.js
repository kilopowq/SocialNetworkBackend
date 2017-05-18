var Parser = {

    parse: function (t) {
        return this.replaceUrl(this.replaceEmoticons(t));
    },

    replaceUrl: function (t) {
        var httpExpression = /(^|\s)(https?:\/\/[^\s.]+[^\s]+)/gi;
        var wwwExpression = /(^|\s)(www\.[^\s]+)/gi;
        t = t.replace(httpExpression, '$1<a href="$2" class="matched">$2</a>');
        return t.replace(wwwExpression, '$1<a href="http://$2" class="matched">$2</a>');
    },

    replaceEmoticons: function (t) {
        var width = Math.floor(EmoticonHandler.width / EmoticonHandler.columns);
        var height = Math.floor(EmoticonHandler.height / EmoticonHandler.rows);

        for (var i = 0; i < EmoticonHandler.emoticons.length; i++) {
            var hPos = i % EmoticonHandler.columns;
            var vPos = Math.floor(i / EmoticonHandler.columns) % EmoticonHandler.rows;
            var hOffset = -width * hPos;
            var vOffset = -height * vPos;
            t = t.replace(new RegExp(EmoticonHandler.emoticons[i].replace("(", "\\(").replace(")", "\\)"), 'g'),
              "<i style=\"background: url('/Content/Img/emo_sm.gif') no-repeat; display: inline-block; height: " +
              height + "px; width: " + width + "px; background-position: " + hOffset + "px " + vOffset + "px" + ";\"></i>");
        };
        return t;
    }
};

var EmoticonHandler = {

    emoticons: ["(smile)", "(laugh)", "(cheeky)", "(blushing)", "(crying)",
                    "(cwl)", "(wink)", "(afraid)", "(inlove)", "(relieved)",
                    "(wow)", "(mmm)", "(kiss)", "(sad)", "(ouch)"
    ],

    width: 150,
    height: 94,
    columns: 5,
    rows: 3,

    getIndex: function (coords) {
        var index = Math.floor(coords.x * EmoticonHandler.columns / EmoticonHandler.width) +
         EmoticonHandler.columns * Math.floor(coords.y * EmoticonHandler.rows / EmoticonHandler.height);
        return index;
    },

    send: function () {

    }
};