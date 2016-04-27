using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordPuzzleScorer.Domain
{
    public class DefaultScorer : IScorer
    {
        private ILineParser lineParser;
        private IWordScorer wordScorer;

        public DefaultScorer(ILineParser lineParser, IWordScorer wordScorer)
        {
            if (lineParser == null)
                throw new ArgumentNullException("lineParser");

            if (wordScorer == null)
                throw new ArgumentNullException("wordScorer");

            this.lineParser = lineParser;
            this.wordScorer = wordScorer;
        }

        public Score Score(Answer answer)
        {
            var score = new Score();

            foreach (var line in answer.Lines)
            {
                var lineWords = this.lineParser.Parse(line.Value, line.BonusTiles);

                foreach (var word in lineWords)
                {
                    score.WordScores.Add(this.wordScorer.Score(word));
                }
            }

            return score;
        }
    }
}
