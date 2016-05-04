using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WordPuzzleScorer.UI.Models
{
    public class ScoreViewModel
    {
        public List<WordScoreViewModel> WordScores { get; private set; }

        public int TotalScore { get; set; }
    }
}