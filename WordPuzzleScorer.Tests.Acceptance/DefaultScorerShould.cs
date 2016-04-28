using Moq;
using Ploeh.AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordPuzzleScorer.Domain;
using Xunit;

namespace WordPuzzleScorer.Tests.Acceptance
{
    public class DefaultScorerShould
    {
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

            Assert.Equal(1, score.WordScores.Count);
            Assert.Equal(inputWord, score.WordScores.First().Word);
            Assert.True(score.WordScores.First().IsValid);
            Assert.Equal(expectedScore, score.WordScores.First().TotalScore);
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

            Assert.Equal(1, score.WordScores.Count);
            Assert.Equal(inputWord, score.WordScores.First().Word);
            Assert.False(score.WordScores.First().IsValid);
            Assert.Equal(0, score.WordScores.First().TotalScore);

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

            Assert.Equal(2, score.WordScores.Count);
            Assert.Equal(inputLine.Split(' ')[0], score.WordScores.First().Word);
            Assert.True(score.WordScores.First().IsValid);
            Assert.Equal(expectedScore1, score.WordScores.First().TotalScore);
            
            Assert.Equal(inputLine.Split(' ')[1], score.WordScores.Skip(1).First().Word);
            Assert.True(score.WordScores.Skip(1).First().IsValid);
            Assert.Equal(expectedScore2, score.WordScores.Skip(1).First().TotalScore);

            Assert.Equal(expectedScore1 + expectedScore2, score.TotalScore);
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

            Assert.Equal(3, score.WordScores.Count);
            Assert.Equal(inputLine.Split(' ')[0], score.WordScores.First().Word);
            Assert.True(score.WordScores.First().IsValid);
            Assert.Equal(expectedScore1, score.WordScores.First().TotalScore);

            Assert.Equal(inputLine.Split(' ')[1], score.WordScores.Skip(1).First().Word);
            Assert.True(score.WordScores.Skip(1).First().IsValid);
            Assert.Equal(expectedScore2, score.WordScores.Skip(1).First().TotalScore);

            Assert.Equal(inputLine.Split(' ')[2], score.WordScores.Skip(2).First().Word);
            Assert.True(score.WordScores.Skip(2).First().IsValid);
            Assert.Equal(expectedScore2, score.WordScores.Skip(2).First().TotalScore);

            Assert.Equal(expectedScore1 + expectedScore2 + expectedScore3, score.TotalScore);
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

            Assert.Equal(1, score.WordScores.First().LetterBonusIndexes.Count());
            Assert.Equal(doubleLetterBonuesTileIndex, score.WordScores.First().LetterBonusIndexes.First());
            Assert.Equal(expectedScore, score.WordScores.First().TotalScore);
            Assert.Equal(expectedScore, score.TotalScore);
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

            Assert.Equal(expectedScore1, score.WordScores.First().TotalScore);
            Assert.Equal(expectedScore2, score.WordScores.Skip(1).First().TotalScore);
            Assert.Equal(expectedScore1 + expectedScore2, score.TotalScore);
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

            Assert.Equal(1, score.WordScores.First().LetterBonusIndexes.Count());
            Assert.Equal(expectedScore, score.WordScores.First().TotalScore);
            Assert.Equal(expectedScore, score.TotalScore);
        }

        [Theory, InlineAutoMoqDataAttribute]
        public void ScoreMultipleLines(Answer answer, [Frozen] Mock<IWordChecker> wordCheckerStub, DefaultWordScorer wordScorer, DefaultLineParser parser)
        {
            answer.Lines.Add(new Line("AXE", Enumerable.Empty<int>()));
            answer.Lines.Add(new Line("DUCK", Enumerable.Empty<int>()));
            wordCheckerStub.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);
            var scorer = new DefaultScorer(parser, wordScorer);

            var score = scorer.Score(answer);

            Assert.Equal(2, score.WordScores.Count);
            Assert.Equal("AXE", score.WordScores.First().Word);
            Assert.True(score.WordScores.First().IsValid);
            Assert.Equal(3, score.WordScores.First().TotalScore);
            Assert.Equal("DUCK", score.WordScores.Skip(1).First().Word);
            Assert.True(score.WordScores.Skip(1).First().IsValid);
            Assert.Equal(4, score.WordScores.Skip(1).First().TotalScore);
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

            Assert.Equal(6, score.WordScores.Count);
            Assert.Equal(27, score.TotalScore);
        }
    }
}