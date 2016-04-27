using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordPuzzleScorer.Domain
{
    public class Word
    {
        public Word(string value, IEnumerable<int> bonusTiles)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            if (bonusTiles == null)
                throw new ArgumentNullException("bonusTiles");

            this.Value = value;
            this.BonusTiles = bonusTiles;
        }

        public string Value { get; private set; }
        public IEnumerable<int> BonusTiles { get; private set; }
    }
}
