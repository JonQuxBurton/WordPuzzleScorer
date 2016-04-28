using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordPuzzleScorer.Domain;
using Xunit;

namespace WordPuzzleScorer.Tests
{
    public class DefaultLineParserShould
    {
        [Theory, AutoMoqData]
        public void ParseLine(DefaultLineParser parser)
        {
            var line = "AXE";

            var words = parser.Parse(line, Enumerable.Empty<int>());

            Assert.Equal(1, words.Count());
            Assert.Equal("AXE", words.First().Value);
        }

        [Theory, AutoMoqData]
        public void ParseTwoWordLine(DefaultLineParser parser)
        {
            var line = "AXE BEE";

            var words = parser.Parse(line, Enumerable.Empty<int>());

            Assert.Equal(2, words.Count());
            Assert.Equal("AXE", words.First().Value);
            Assert.Equal("BEE", words.Skip(1).First().Value);
        }

        [Theory, AutoMoqData]
        public void ParseLineWithBonusTile(DefaultLineParser parser)
        {
            var line = "AXE";

            var words = parser.Parse(line, new int[] { 2 });

            Assert.Equal(1, words.First().BonusTiles.Count());
            Assert.Equal(2, words.First().BonusTiles.First());
        }

        [Theory]
        [InlineAutoMoqData(" AXE")]
        [InlineAutoMoqData("AXE ")]
        [InlineAutoMoqData(" AXE ")]
        public void ParseLinesWithSpaces(string inputLine, DefaultLineParser parser)
        {
            var words = parser.Parse(inputLine, Enumerable.Empty<int>());

            Assert.Equal(1, words.Count());
            Assert.Equal("AXE", words.First().Value);
        }

        [Theory, AutoMoqData]
        public void ParseLineWithMulitpleSpaces(DefaultLineParser parser)
        {
            var inputLine = "AXE  DUCK";

            var words = parser.Parse(inputLine, Enumerable.Empty<int>());

            Assert.Equal(2, words.Count());
            Assert.Equal("AXE", words.First().Value);
            Assert.Equal("DUCK", words.Skip(1).First().Value);
        }

        [Theory, AutoMoqData]
        public void ParseTwoWordLineWithBonusTiles(DefaultLineParser parser)
        {
            var line = "AXE BEE";

            var words = parser.Parse(line, new int[] { 2, 6 });

            Assert.Equal(1, words.First().BonusTiles.Count());
            Assert.Equal(2, words.First().BonusTiles.First());

            Assert.Equal(1, words.Skip(1).First().BonusTiles.Count());
            Assert.Equal(2, words.Skip(1).First().BonusTiles.First());
        }

        [Theory, AutoMoqData]
        public void PreserveBonusTilesIndexesWhenRemovingSpaces(DefaultLineParser parser)
        {
            var line = " AXE  DUCK";

            var words = parser.Parse(line, new int[] { 6 });

            Assert.Equal(1, words.Skip(1).First().BonusTiles.Count());
            Assert.Equal(0, words.Skip(1).First().BonusTiles.First());
        }

        [Theory, AutoMoqData]
        public void ReturnsNoWordsWhenLineIsAllSpaces(DefaultLineParser parser)
        {
            var line = "   ";

            var words = parser.Parse(line, new int[] { 6 });

            Assert.Equal(0, words.Count());
        }
    }
}
