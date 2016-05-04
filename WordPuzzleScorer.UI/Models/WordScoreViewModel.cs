using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WordPuzzleScorer.UI.Models
{
    public class WordScoreViewModel
    {
        public string Word { get; private set; }
        public bool IsValid { get; private set; }
        public int BaseScore { get; private set; }
        public int TotalScore { get; private set; }
        public IEnumerable<int> LetterBonusIndexes { get; private set; }
    }
}