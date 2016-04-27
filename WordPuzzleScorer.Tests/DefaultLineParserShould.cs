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
        [Fact]
        public void ParseLine()
        {
            var line = "AXE";

            var parser = new DefaultLineParser();

            var words = parser.Parse(line, Enumerable.Empty<int>());

            Assert.Equal(1, words.Count());
            Assert.Equal("AXE", words.First().Value);
        }

        [Fact]
        public void ParseTwoWordLine()
        {
            var line = "AXE BEE";

            var parser = new DefaultLineParser();

            var words = parser.Parse(line, Enumerable.Empty<int>());

            Assert.Equal(2, words.Count());
            Assert.Equal("AXE", words.First().Value);
            Assert.Equal("BEE", words.Skip(1).First().Value);
        }

        [Fact]
        public void ParseLineWithBonusTile()
        {
            var line = "AXE";

            var parser = new DefaultLineParser();

            var words = parser.Parse(line, new int[] { 2 });

            Assert.Equal(1, words.First().BonusTiles.Count());
            Assert.Equal(2, words.First().BonusTiles.First());
        }

        [Theory]
        [InlineData(" AXE")]
        [InlineData("AXE ")]
        [InlineData(" AXE ")]
        public void ParseLinesWithSpaces(string inputLine)
        {
            var parser = new DefaultLineParser();

            var words = parser.Parse(inputLine, Enumerable.Empty<int>());

            Assert.Equal(1, words.Count());
            Assert.Equal("AXE", words.First().Value);
        }

        [Fact]
        public void ParseLineWithMulitpleSpaces()
        {
            var inputLine = "AXE  DUCK";
            var parser = new DefaultLineParser();

            var words = parser.Parse(inputLine, Enumerable.Empty<int>());

            Assert.Equal(2, words.Count());
            Assert.Equal("AXE", words.First().Value);
            Assert.Equal("DUCK", words.Skip(1).First().Value);
        }

        [Fact]
        public void ParseTwoWordLineWithBonusTiles()
        {
            var line = "AXE BEE";

            var parser = new DefaultLineParser();

            var words = parser.Parse(line, new int[] { 2, 6 });

            Assert.Equal(1, words.First().BonusTiles.Count());
            Assert.Equal(2, words.First().BonusTiles.First());

            Assert.Equal(1, words.Skip(1).First().BonusTiles.Count());
            Assert.Equal(2, words.Skip(1).First().BonusTiles.First());
        }

        [Fact]
        public void PreserveBonusTilesIndexesWhenRemovingSpaces()
        {
            var line = " AXE  DUCK";

            var parser = new DefaultLineParser();

            var words = parser.Parse(line, new int[] { 6 });

            Assert.Equal(1, words.Skip(1).First().BonusTiles.Count());
            Assert.Equal(0, words.Skip(1).First().BonusTiles.First());
        }

        [Fact]
        public void ReturnsNoWordsWhenLineIsAllSpaces()
        {
            var line = "   ";

            var parser = new DefaultLineParser();

            var words = parser.Parse(line, new int[] { 6 });

            Assert.Equal(0, words.Count());
        }
    }
}
