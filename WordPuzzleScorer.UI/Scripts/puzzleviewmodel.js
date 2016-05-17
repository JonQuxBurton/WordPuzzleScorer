var PuzzleViewModel = (function () {
    function PuzzleViewModel(letters) {
        var _this = this;
        this.letters = ko.observable(letters);
        this.lettersSplit = ko.computed(function () {
            return _this.letters().split("");
        });
        this.answer = ko.observableArray([]);
    }
    PuzzleViewModel.prototype.init = function (puzzleLines) {
        this.puzzleLines = puzzleLines;
        this.answerBacking = [];
        var that = this;
        _(puzzleLines).forEach(function (line) {
            for (var i = 0; i < line.length; i++) {
                var x = line.origin.x;
                var y = line.origin.y;
                var isBonusTile = false;
                var bonusTile = _.find(line.bonusTiles, function (item) { return item == i; });
                if (!_.isUndefined(bonusTile)) {
                    isBonusTile = true;
                }
                if (line.direction == Direction.Horizontal) {
                    x += i;
                }
                else {
                    y += i;
                }
                var doubleTile = _.find(that.answerBacking, function (o) { return o.x == x && o.y == y; });
                if (_.isUndefined(doubleTile)) {
                    that.answerBacking.push(new Tile("", false, x, y, isBonusTile));
                }
            }
        });
        this.refreshAnswer();
    };
    PuzzleViewModel.prototype.refreshAnswer = function () {
        this.answer.removeAll();
        for (var i = 0; i < this.answerBacking.length; i++) {
            this.answer.push(this.answerBacking[i]);
        }
    };
    PuzzleViewModel.prototype.getAnswerLines = function () {
        var answer = [];
        var that = this;
        _(this.puzzleLines).forEach(function (line) {
            var answerLine = [];
            for (var i = 0; i < line.length; i++) {
                var x = line.origin.x;
                var y = line.origin.y;
                if (line.direction == Direction.Horizontal) {
                    x += i;
                }
                else {
                    y += i;
                }
                var tile = _.find(that.answerBacking, function (o) { return o.x == x && o.y == y; });
                answerLine.push(tile.letter);
            }
            answer.push(answerLine.join(""));
        });
        return answer;
    };
    return PuzzleViewModel;
}());
var Tile = (function () {
    function Tile(letter, isDone, x, y, isBonusTile) {
        this.letter = letter;
        this.isDone = isDone;
        this.x = x;
        this.y = y;
        this.isBonusTile = isBonusTile;
    }
    return Tile;
}());
var PuzzleLine = (function () {
    function PuzzleLine(origin, length, direction, bonusTiles) {
        this.origin = origin;
        this.length = length;
        this.direction = direction;
        this.bonusTiles = bonusTiles;
    }
    return PuzzleLine;
}());
var Point = (function () {
    function Point(x, y) {
        this.x = x;
        this.y = y;
    }
    return Point;
}());
var Direction;
(function (Direction) {
    Direction[Direction["Verical"] = 0] = "Verical";
    Direction[Direction["Horizontal"] = 1] = "Horizontal";
})(Direction || (Direction = {}));
//# sourceMappingURL=puzzleviewmodel.js.map