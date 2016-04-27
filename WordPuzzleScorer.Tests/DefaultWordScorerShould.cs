using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordPuzzleScorer.Domain;
using Xunit;

namespace WordPuzzleScorer.Tests
{
    public class DefaultWordScorerShould
    {
        [Theory]
        [InlineData("AX", 2)]
        [InlineData("AXE", 3)]
        [InlineData("AXES", 4)]
        public void ScoreOnePointPerLetter(string inputWord, int expectedTotalScore)
        {
            var wordCheckerStub = new Mock<IWordChecker>();
            wordCheckerStub.Setup(x => x.IsValid(inputWord)).Returns(true);

            var defaultScorer = new DefaultWordScorer(wordCheckerStub.Object);
            var actualScore = defaultScorer.Score(new Word(inputWord, Enumerable.Empty<int>()));

            Assert.Equal(inputWord, actualScore.Word);
            Assert.Equal(true, actualScore.IsValid);
            Assert.Equal(expectedTotalScore, actualScore.TotalScore);
        }

        [Theory]
        [InlineData("ZX", 0)]
        [InlineData("ZXE", 0)]
        [InlineData("ZXES", 0)]        
        public void ScoreNoPointsForInvalidWord(string inputWord, int expectedTotalScore)
        {
            var wordChecker = new Mock<IWordChecker>();
            wordChecker.Setup(x => x.IsValid(inputWord)).Returns(false);

            var defaultScorer = new DefaultWordScorer(wordChecker.Object);
            var actualScore = defaultScorer.Score(new Word(inputWord, Enumerable.Empty<int>()));

            Assert.Equal(expectedTotalScore, actualScore.TotalScore);
        }

        [Fact]
        public void ScoreBonusPoint()
        {
            var inputWord = "AXE";
            var wordChecker = new Mock<IWordChecker>();
            wordChecker.Setup(x => x.IsValid(inputWord)).Returns(true);

            var defaultScorer = new DefaultWordScorer(wordChecker.Object);
            var actualScore = defaultScorer.Score(new Word(inputWord, new int[] { 2 }));

            Assert.Equal(1, actualScore.LetterBonusIndexes.Count());
            Assert.Equal(2, actualScore.LetterBonusIndexes.First());
            Assert.Equal(3, actualScore.BaseScore);
            Assert.Equal(4, actualScore.TotalScore);
        }
    }
}
