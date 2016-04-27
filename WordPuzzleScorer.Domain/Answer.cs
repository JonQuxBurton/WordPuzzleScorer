using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordPuzzleScorer.Domain
{
    public class Answer
    {
        public Answer()
        {
            this.Lines = new List<Line>();
        }

        public List<Line> Lines { get; private set; }
    }
}
