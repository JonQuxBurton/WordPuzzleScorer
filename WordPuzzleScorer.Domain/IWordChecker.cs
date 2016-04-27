using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordPuzzleScorer.Domain
{
    public interface IWordChecker
    {
        bool IsValid(string word);
    }
}
