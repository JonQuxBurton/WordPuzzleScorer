using Moq;
using Ploeh.AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordPuzzleScorer.Domain;
using Xunit;
using FluentAssertions;

namespace WordPuzzleScorer.Tests.Acceptance
{
    public class DefaultScorerShould
    {
        [Theory, AutoMoqData]
        public void NotScoreAOneLetterWord(Answer answer, [Frozen] Mock<IWordChecker> wordCheckerStub, DefaultWordScorer wordScorer, DefaultLineParser parser)
        {
            string inputWord = "A";
            answer.Lines.Add(new Line(inputWord, Enumerable.Empty<int>()));

            wordCheckerStub.Setup(x => x.IsValid(inputWord)).Returns(true);
            var scorer = new DefaultScorer(parser, wordScorer);

            var score = scorer.Score(answer);

            score.WordScores.Should().BeEmpty();
        }

        [Theory]
        [InlineAutoMoqDataAttribute("AXE", 3)]
        [InlineAutoMoqDataAttribute("GOOSE", 5)]
        public void ScoreAOneWordAnswerLine(string inputWord, int expectedScore,
                Answer answer, [Frozen] Mock<IWordChecker> wordCheckerStub, DefaultWordScorer wordScorer, DefaultLineParser parser)
        {
            answer.Lines.Add(new Line(inputWord, Enumerable.Empty<int>()));
            wordCheckerStub.Setup(x => x.IsValid(inputWord)).Returns(true);
            var scorer = new DefaultScorer(parser, wordScorer);

            var score = scorer.Score(answer);

            score.WordScores.Should().ContainSingle()
                            .Which.ShouldBeEquivalentTo(new {
                                                            Word = inputWord,
                                                            IsValid = true,
                                                            TotalScore = expectedScore,
                                                            
                            }, options => options.ExcludingMissingMembers());
        }

        [Theory]
        [InlineAutoMoqDataAttribute("AXZ")]
        [InlineAutoMoqDataAttribute("GOZSE")]
        public void ScoreZeroForInvalidWordAnswerLine(string inputWord,
            Answer answer, [Frozen] Mock<IWordChecker> wordCheckerStub, DefaultWordScorer wordScorer, DefaultLineParser parser)
        {
            answer.Lines.Add(new Line(inputWord, Enumerable.Empty<int>()));
            wordCheckerStub.Setup(x => x.IsValid(It.IsAny<string>())).Returns(false);
            var scorer = new DefaultScorer(parser, wordScorer);

            var score = scorer.Score(answer);

            score.WordScores.Should().ContainSingle().Which.ShouldBeEquivalentTo(new
            {
                Word = inputWord,
                IsValid = false,
                TotalScore = 0,
            }, options => options.ExcludingMissingMembers());

        }

        [Theory]
        [InlineAutoMoqDataAttribute("AXE DUCK", 3, 4)]
        public void ScoreATwoWordAnswerLine(string inputLine, int expectedScore1, int expectedScore2,
                        Answer answer, [Frozen] Mock<IWordChecker> wordCheckerStub, DefaultWordScorer wordScorer, DefaultLineParser parser)
        {
            answer.Lines.Add(new Line(inputLine, Enumerable.Empty<int>()));
            wordCheckerStub.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);
            var scorer = new DefaultScorer(parser, wordScorer);

            var score = scorer.Score(answer);

            score.WordScores.Should().HaveCount(2);
            score.WordScores.First().ShouldBeEquivalentTo(new {
                Word = inputLine.Split(' ')[0],
                IsValid = true,
                TotalScore = expectedScore1,
                BaseScore = expectedScore1,
                LetterBonusIndexes = Enumerable.Empty<int>()
            });
            score.WordScores.ElementAt(1).ShouldBeEquivalentTo(new
            {
                Word = inputLine.Split(' ')[1],
                IsValid = true,
                TotalScore = expectedScore2,
                BaseScore = expectedScore2,
                LetterBonusIndexes = Enumerable.Empty<int>()
            });
            score.TotalScore.Should().Be(expectedScore1 + expectedScore2);
        }

        [Theory]
        [InlineAutoMoqDataAttribute("SWAN EAGLE GOOSE", 4, 5, 5)]
        public void ScoreAMultiWordAnswerLine(string inputLine, int expectedScore1, int expectedScore2, int expectedScore3,
                                    Answer answer, [Frozen] Mock<IWordChecker> wordCheckerStub, DefaultWordScorer wordScorer, DefaultLineParser parser)
        {
            answer.Lines.Add(new Line(inputLine, Enumerable.Empty<int>()));
            wordCheckerStub.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);
            var scorer = new DefaultScorer(parser, wordScorer);

            var score = scorer.Score(answer);

            score.WordScores.Should().HaveCount(3);
            score.WordScores.First().ShouldBeEquivalentTo(new
            {
                Word = inputLine.Split(' ')[0],
                IsValid = true,
                TotalScore = expectedScore1,
                BaseScore = expectedScore1,
                LetterBonusIndexes = Enumerable.Empty<int>()
            });
            score.WordScores.ElementAt(1).ShouldBeEquivalentTo(new
            {
                Word = inputLine.Split(' ')[1],
                IsValid = true,
                TotalScore = expectedScore2,
                BaseScore = expectedScore2,
                LetterBonusIndexes = Enumerable.Empty<int>()
            });
            score.WordScores.ElementAt(2).ShouldBeEquivalentTo(new
            {
                Word = inputLine.Split(' ')[2],
                IsValid = true,
                TotalScore = expectedScore3,
                BaseScore = expectedScore3,
                LetterBonusIndexes = Enumerable.Empty<int>()
            });
            score.TotalScore.Should().Be(expectedScore1 + expectedScore2 + expectedScore3);
        }

        [Theory]
        [InlineAutoMoqDataAttribute("AXE", 2, 4)]
        public void ScoreADoubleLetterBonus(string inputLine, int doubleLetterBonuesTileIndex, int expectedScore,
                                Answer answer, [Frozen] Mock<IWordChecker> wordCheckerStub, DefaultWordScorer wordScorer, DefaultLineParser parser)

        {
            answer.Lines.Add(new Line(inputLine, new List<int>() { doubleLetterBonuesTileIndex }));
            wordCheckerStub.Setup(x => x.IsValid(inputLine)).Returns(true);
            var scorer = new DefaultScorer(parser, wordScorer);

            var score = scorer.Score(answer);

            score.WordScores.First().LetterBonusIndexes.Should().ContainSingle(x => x == doubleLetterBonuesTileIndex);
            score.WordScores.First().TotalScore.Should().Be(expectedScore);
            score.TotalScore.Should().Be(expectedScore);
        }

        [Theory]
        [InlineAutoMoqDataAttribute("AXE DUCK", 1, 6, 4, 5)]
        public void ScoreDoubleLetterBonuses(string inputLine, int bonuesTileIndex1, int bonuesTileIndex2, int expectedScore1, int expectedScore2,
                            Answer answer, [Frozen] Mock<IWordChecker> wordCheckerStub, DefaultWordScorer wordScorer, DefaultLineParser parser)

        {
            answer.Lines.Add(new Line(inputLine, new List<int>() { bonuesTileIndex1, bonuesTileIndex2 }));
            wordCheckerStub.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);
            var scorer = new DefaultScorer(parser, wordScorer);

            var score = scorer.Score(answer);

            score.WordScores.First().TotalScore.Should().Be(expectedScore1);
            score.WordScores.ElementAt(1).TotalScore.Should().Be(expectedScore2);
            score.TotalScore.Should().Be(expectedScore1 + expectedScore2);
        }

        [Theory]
        [InlineAutoMoqDataAttribute("AXE", 2, 0)]
        public void NotScoreADoubleLetterBonusIfWordIsInvalid(string inputLine, int doubleLetterBonuesTileIndex, int expectedScore,
                        Answer answer, [Frozen] Mock<IWordChecker> wordCheckerStub, DefaultWordScorer wordScorer, DefaultLineParser parser)

        {
            answer.Lines.Add(new Line(inputLine, new List<int>() { doubleLetterBonuesTileIndex }));
            wordCheckerStub.Setup(x => x.IsValid(inputLine)).Returns(false);
            var scorer = new DefaultScorer(parser, wordScorer);

            var score = scorer.Score(answer);

            score.WordScores.First().LetterBonusIndexes.Should().ContainSingle(x => x == doubleLetterBonuesTileIndex);
            score.WordScores.First().TotalScore.Should().Be(expectedScore);
            score.TotalScore.Should().Be(expectedScore);
        }

        [Theory, InlineAutoMoqDataAttribute]
        public void ScoreMultipleLines(Answer answer, [Frozen] Mock<IWordChecker> wordCheckerStub, DefaultWordScorer wordScorer, DefaultLineParser parser)
        {
            answer.Lines.Add(new Line("AXE", Enumerable.Empty<int>()));
            answer.Lines.Add(new Line("DUCK", Enumerable.Empty<int>()));
            wordCheckerStub.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);
            var scorer = new DefaultScorer(parser, wordScorer);

            var score = scorer.Score(answer);

            score.WordScores.Should().HaveCount(2);
            score.WordScores.First().ShouldBeEquivalentTo(new { 
                Word = "AXE", 
                IsValid = true, 
                TotalScore = 3 ,
                BaseScore = 3,
                LetterBonusIndexes = Enumerable.Empty<int>()
            });
            score.WordScores.ElementAt(1).ShouldBeEquivalentTo(new { 
                Word = "DUCK", 
                IsValid = true, 
                TotalScore = 4,
                BaseScore = 4 ,
                LetterBonusIndexes = Enumerable.Empty<int>()
            });
        }

        [Theory, InlineAutoMoqDataAttribute]
        public void ScoreComplexAnswer(Answer answer, [Frozen] Mock<IWordChecker> wordCheckerStub, DefaultWordScorer wordScorer, DefaultLineParser parser)
        {
            answer.Lines.Add(new Line(" DUCK GOOSE  ", new int[] { 0, 2 }));
            answer.Lines.Add(new Line("EAGLE GULL  ", new int[] { 7 }));
            answer.Lines.Add(new Line("  MALLARD LZW", new int[] { 10 }));

            wordCheckerStub.Setup(x => x.IsValid("DUCK")).Returns(true);
            wordCheckerStub.Setup(x => x.IsValid("GOOSE")).Returns(true);
            wordCheckerStub.Setup(x => x.IsValid("EAGLE")).Returns(true);
            wordCheckerStub.Setup(x => x.IsValid("GULL")).Returns(true);
            wordCheckerStub.Setup(x => x.IsValid("MALLARD")).Returns(true);

            var scorer = new DefaultScorer(parser, wordScorer);

            var score = scorer.Score(answer);


            score.WordScores.Should().HaveCount(6);
            score.TotalScore.Should().Be(27);
        }
    }
}