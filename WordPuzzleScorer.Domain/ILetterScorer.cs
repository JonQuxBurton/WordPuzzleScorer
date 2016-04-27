using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordPuzzleScorer.Domain
{
    public interface ILetterScorer
    {
        int GetScore(char letter);
    }
}
