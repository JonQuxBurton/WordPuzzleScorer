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

            actualScore.Word.Should().Be(inputWord);
            actualScore.IsValid.Should().BeTrue();
            actualScore.TotalScore.Should().Be(expectedTotalScore);
        }

        [Theory]
        [InlineAutoMoqData("ZX", 0)]
        [InlineAutoMoqData("ZXE", 0)]
        [InlineAutoMoqData("ZXES", 0)]
        public void ScoreNoPointsForInvalidWord(string inputWord, int expectedTotalScore, [Frozen] Mock<IWordChecker> wordCheckerStub, DefaultWordScorer defaultScorer)
        {
            wordCheckerStub.Setup(x => x.IsValid(inputWord)).Returns(false);

            var actualScore = defaultScorer.Score(new Word(inputWord, Enumerable.Empty<int>()));

            actualScore.TotalScore.Should().Be(expectedTotalScore);
        }

        [Theory, AutoMoqData]
        public void ScoreBonusPoint([Frozen] Mock<IWordChecker> wordChecker, DefaultWordScorer defaultScorer)
        {
            var inputWord = "AXE";
            wordChecker.Setup(x => x.IsValid(inputWord)).Returns(true);

            var actualScore = defaultScorer.Score(new Word(inputWord, new int[] { 2 }));

            actualScore.LetterBonusIndexes.Should().ContainSingle(x => x == 2);
            actualScore.BaseScore.Should().Be(3);
            actualScore.TotalScore.Should().Be(4);
        }
    }
}
