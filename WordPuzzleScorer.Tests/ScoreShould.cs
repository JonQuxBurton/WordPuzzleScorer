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
        [Fact]
        public void ReturnTotalScoreZeroWhenNoWords()
        {
            var score = new Score();

            Assert.Equal(0, score.TotalScore);
        }

        [Fact]
        public void ReturnTotalScoreForWord()
        {
            var score = new Score();
            score.WordScores.Add(new WordScore("AAA", true, 0, 3));

            Assert.Equal(3, score.TotalScore);
        }

        [Fact]
        public void ReturnTotalScoreForWords()
        {
            var score = new Score();
            score.WordScores.Add(new WordScore("AAA", true, 0, 3));
            score.WordScores.Add(new WordScore("AAA", true, 0, 3));

            Assert.Equal(6, score.TotalScore);
        }

        [Fact]
        public void ReturnTotalScoreForInvalidWords()
        {
            var score = new Score();
            score.WordScores.Add(new WordScore("AAA", false, 0, 3));
            
            Assert.Equal(0, score.TotalScore);
        }
    }
}
