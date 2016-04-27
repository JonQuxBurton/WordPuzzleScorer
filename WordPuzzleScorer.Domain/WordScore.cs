using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordPuzzleScorer.Domain
{
    public class WordScore
    {
        public WordScore(string word, bool isValid, int baseScore, int totalScore) : this(word, isValid, baseScore, totalScore, new int[]{})
        {

        }

        public WordScore(string word, bool isValid, int baseScore, int totalScore, IEnumerable<int> letterBonusTiles)
        {
            if (word == null)
                throw new ArgumentNullException("word");

            if (letterBonusTiles == null)
                throw new ArgumentNullException("letterBonusTiles");

            this.Word = word;
            this.IsValid = isValid;
            this.BaseScore = baseScore;
            this.TotalScore = totalScore;

            this.LetterBonusIndexes = letterBonusTiles;
        }

        public string Word { get; private set; }
        public bool IsValid{ get; private set; }
        public int BaseScore { get; private set; }
        public int TotalScore { get; private set; }
        public IEnumerable<int> LetterBonusIndexes { get; private set; }
    }
}
