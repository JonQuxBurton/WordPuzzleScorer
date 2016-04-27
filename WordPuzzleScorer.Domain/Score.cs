using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordPuzzleScorer.Domain
{
    public class Score
    {
        public Score()
        {
            this.WordScores = new List<WordScore>();
        }

        public List<WordScore> WordScores { get; private set; }

        public int TotalScore
        {
            get { return this.WordScores.Where(x => x.IsValid).Sum(x => x.TotalScore);  }
        }
    }
}
