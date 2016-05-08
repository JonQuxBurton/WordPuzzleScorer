using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordPuzzleScorer.Domain
{
    public class DefaultWordScorer : IWordScorer
    {
        private IWordChecker wordChecker;

        public DefaultWordScorer(IWordChecker wordChecker)
        {
            if (wordChecker == null)
                throw new ArgumentNullException("wordChecker");

            this.wordChecker = wordChecker;
        }

        public WordScore Score(Word word)
        {
            var baseScore = 0;
            var bonusScore = 0;

            bool isValid = false;

            if (word.Value.Length > 1)
            {
                if (this.wordChecker.IsValid(word.Value))
                {
                    isValid = true;
                    baseScore += word.Value.Length;
                }
            }

            if (isValid && word.BonusTiles != null && word.BonusTiles.Any())
            {
                bonusScore += word.BonusTiles.Count(x => x < word.Value.Length);
            }

            return new WordScore(word.Value, isValid, baseScore, baseScore + bonusScore, word.BonusTiles.Where(x => x < word.Value.Length));
        }
    }
}
