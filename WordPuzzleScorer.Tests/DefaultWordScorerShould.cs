using Moq;
using Ploeh.AutoFixture.Xunit2;
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
        [InlineAutoMoqData("AX", 2)]
        [InlineAutoMoqData("AXE", 3)]
        [InlineAutoMoqData("AXES", 4)]
        public void ScoreOnePointPerLetter(string inputWord, int expectedTotalScore, [Frozen] Mock<IWordChecker> wordCheckerStub, DefaultWordScorer defaultScorer)
        {
            wordCheckerStub.Setup(x => x.IsValid(inputWord)).Returns(true);

            var actualScore = defaultScorer.Score(new Word(inputWord, Enumerable.Empty<int>()));

            Assert.Equal(inputWord, actualScore.Word);
            Assert.Equal(true, actualScore.IsValid);
            Assert.Equal(expectedTotalScore, actualScore.TotalScore);
        }

        [Theory]
        [InlineAutoMoqData("ZX", 0)]
        [InlineAutoMoqData("ZXE", 0)]
        [InlineAutoMoqData("ZXES", 0)]
        public void ScoreNoPointsForInvalidWord(string inputWord, int expectedTotalScore, [Frozen] Mock<IWordChecker> wordCheckerStub, DefaultWordScorer defaultScorer)
        {
            wordCheckerStub.Setup(x => x.IsValid(inputWord)).Returns(false);

            var actualScore = defaultScorer.Score(new Word(inputWord, Enumerable.Empty<int>()));

            Assert.Equal(expectedTotalScore, actualScore.TotalScore);
        }

        [Theory, AutoMoqData]
        public void ScoreBonusPoint([Frozen] Mock<IWordChecker> wordChecker, DefaultWordScorer defaultScorer)
        {
            var inputWord = "AXE";
            wordChecker.Setup(x => x.IsValid(inputWord)).Returns(true);

            var actualScore = defaultScorer.Score(new Word(inputWord, new int[] { 2 }));

            Assert.Equal(1, actualScore.LetterBonusIndexes.Count());
            Assert.Equal(2, actualScore.LetterBonusIndexes.First());
            Assert.Equal(3, actualScore.BaseScore);
            Assert.Equal(4, actualScore.TotalScore);
        }
    }
}
