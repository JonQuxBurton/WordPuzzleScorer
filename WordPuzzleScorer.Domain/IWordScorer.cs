using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordPuzzleScorer.Domain
{
    public interface IWordScorer
    {
        WordScore Score(Word word);
    }
}
