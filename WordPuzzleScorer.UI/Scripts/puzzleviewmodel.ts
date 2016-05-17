
class PuzzleViewModel {

    private letters;

    public lettersSplit: KnockoutComputed<string>;
    public answer: KnockoutObservableArray<Tile>;
    public answerBacking: Array<Tile>;
    public puzzleLines: any;

    constructor(letters: string) {
        this.letters = ko.observable(letters);

        this.lettersSplit = ko.computed(() => {
            return this.letters().split("");
        });

        this.answer = ko.observableArray([]);
    }

    public init(puzzleLines: any) {

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
                } else {
                    y += i;
                }

                var doubleTile = _.find(that.answerBacking, function (o) { return o.x == x && o.y == y; });

                if (_.isUndefined(doubleTile)) {
                    that.answerBacking.push(new Tile("", false, x, y, isBonusTile));
                }
            }
        });

        this.refreshAnswer();
    }

    public refreshAnswer() {

        this.answer.removeAll();

        for (var i = 0; i < this.answerBacking.length; i++) {
            this.answer.push(this.answerBacking[i]);
        }
    }

    public getAnswerLines(): string[] {

        var answer = [];

        var that = this;

        _(this.puzzleLines).forEach(function (line) {

            var answerLine = [];

            for (var i = 0; i < line.length; i++) {
                var x = line.origin.x;
                var y = line.origin.y;

                if (line.direction == Direction.Horizontal) {
                    x += i;
                } else {
                    y += i;
                }

                var tile = _.find(that.answerBacking, function (o) { return o.x == x && o.y == y; });

                answerLine.push(tile.letter);
            }

            answer.push(answerLine.join(""));
        });

        return answer;
    }
}

class Tile {
    constructor(public letter: string, public isDone: boolean, public x: number, public y: number, public isBonusTile: boolean) {
    }
}

class PuzzleLine {
    constructor(public origin: Point, public length: number, public direction: Direction, public bonusTiles: Array<number>) {
    }
}
     
class Point {
    constructor(public x: number, public y: number) {
    }
}

enum Direction {
    Verical,
    Horizontal
}