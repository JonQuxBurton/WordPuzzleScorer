using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
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
    public class ScoreShould
    {
        [Theory, AutoMoqData]
        public void ReturnTotalScoreZeroWhenNoWords(Score score)
        {
            Assert.Equal(0, score.TotalScore);
        }

        [Theory, AutoMoqData]
        public void ReturnTotalScoreForWord(Score score)
        {
            score.WordScores.Add(new WordScore("AAA", true, 0, 3));

            Assert.Equal(3, score.TotalScore);
        }

        [Theory, AutoMoqData]
        public void ReturnTotalScoreForWords(Score score)
        {
            score.WordScores.Add(new WordScore("AAA", true, 0, 3));
            score.WordScores.Add(new WordScore("AAA", true, 0, 3));

            Assert.Equal(6, score.TotalScore);
        }

        [Theory, AutoMoqData]
        public void ReturnTotalScoreForInvalidWords(Score score)
        {
            score.WordScores.Add(new WordScore("AAA", false, 0, 3));
            
            Assert.Equal(0, score.TotalScore);
        }
    }
}
