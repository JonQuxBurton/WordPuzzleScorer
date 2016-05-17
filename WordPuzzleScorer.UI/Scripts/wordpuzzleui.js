var WordPuzzleUi = function (puzzleViewModel) {

    this.puzzleViewModel = puzzleViewModel;

    var sourceDiv;
    var sourceLetter;

    var that = this;

    this.enableDragAndDrop = function () {


        $(".draggable").draggable({
            revert: true,
            revertDuration: 100,
            start: function (event, ui) {

                sourceDiv = $(this);
                sourceLetter = ko.contextFor($(this).find("span").get(0)).$data;

            },
            stop: function (event, ui) {
                sourceDiv = null;
                sourceLetter = null;
            }
        });

        $(".droppable").droppable({
            drop: function (event, ui) {

                sourceDiv.addClass("done");
                sourceDiv.draggable("destroy")

                var slot = $(this).attr('id');
                var id = parseInt(slot.split("_")[1]);

                that.puzzleViewModel.answerBacking[id].letter = sourceLetter;
                that.puzzleViewModel.answerBacking[id].isDone = true;

                that.puzzleViewModel.refreshAnswer();
                that.enableDragAndDrop();
            }
        });
    }

    $('#submitButton').click(function () {

        var answer = that.puzzleViewModel.getAnswerLines();

        _.forEach(answer, function (value, index) {
            $('form').append('<input type="hidden" name="AnswerLines[' + index + ']" value="' + value + '" />');
        });
    });
}