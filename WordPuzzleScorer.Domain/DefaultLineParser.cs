using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordPuzzleScorer.Domain
{
    public class DefaultLineParser : ILineParser
    {
        private string remainingLine;
        private IEnumerable<int> letterBonusTiles;

        private class ExtractedWord
        {
            public string Word { get; set; }
            public int StartIndex { get; set; }
            public int EndIndex { get; set; }
        }

        private List<ExtractedWord> ExtractWords(string line)
        {
            var split = this.remainingLine.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            var extracted = new List<ExtractedWord>();

            var cursor = 0;

            foreach (var word in split)
            {
                var startIndex = line.IndexOf(word, cursor);
                var endIndex = startIndex + word.Length;

                extracted.Add(new ExtractedWord() { Word = word, StartIndex = startIndex, EndIndex = endIndex });

                cursor = endIndex;
            }

            return extracted;
        }

        public IEnumerable<Word> Parse(string line, IEnumerable<int> letterBonusTiles)
        {
            if (letterBonusTiles == null)
                throw new ArgumentNullException("letterBonusTiles");

            this.remainingLine = line;
            this.letterBonusTiles = letterBonusTiles;

            var extractedWords = this.ExtractWords(line);

            foreach (var extractedWord in extractedWords)
            {
                var bonusTiles = letterBonusTiles.Where(x => x >= extractedWord.StartIndex && x < extractedWord.EndIndex).Select(x => x - extractedWord.StartIndex);

                yield return new Word(extractedWord.Word, bonusTiles);
            }
        }
    }
}
