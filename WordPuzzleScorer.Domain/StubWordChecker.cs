using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordPuzzleScorer.Domain
{
    public class StubWordChecker : IWordChecker
    {
        public bool IsValid(string word)
        {
            return true;
        }
    }
}
