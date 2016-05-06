using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordPuzzleScorer.Domain;
using Xunit;
using FluentAssertions;

namespace WordPuzzleScorer.Tests
{
    public class DefaultLineParserShould
    {
        [Theory, AutoMoqData]
        public void ParseLine(DefaultLineParser parser)
        {
            var line = "AXE";

            var words = parser.Parse(line, Enumerable.Empty<int>());

            words.Should().HaveCount(1);
            words.First().Value.Should().Be(line);
        }

        [Theory, AutoMoqData]
        public void ParseTwoWordLine(DefaultLineParser parser)
        {
            var line = "AXE BEE";

            var words = parser.Parse(line, Enumerable.Empty<int>());

            words.Should().HaveCount(2);
            words.Should().Contain(x => x.Value == "AXE");
            words.Should().Contain(x => x.Value == "BEE");
        }

        [Theory, AutoMoqData]
        public void ParseLineWithBonusTile(DefaultLineParser parser)
        {
            var line = "AXE";

            var words = parser.Parse(line, new int[] { 2 });

            words.First().BonusTiles.Should().ContainSingle(x => x == 2);
        }

        [Theory]
        [InlineAutoMoqData(" AXE")]
        [InlineAutoMoqData("AXE ")]
        [InlineAutoMoqData(" AXE ")]
        public void ParseLinesWithSpaces(string inputLine, DefaultLineParser parser)
        {
            var words = parser.Parse(inputLine, Enumerable.Empty<int>());

            words.Should().ContainSingle(x => x.Value == "AXE");
        }

        [Theory, AutoMoqData]
        public void ParseLineWithMulitpleSpaces(DefaultLineParser parser)
        {
            var inputLine = "AXE  DUCK";

            var words = parser.Parse(inputLine, Enumerable.Empty<int>());

            words.Should().HaveCount(2);
            words.Should().Contain(x => x.Value == "AXE");
            words.Should().Contain(x => x.Value == "DUCK");
        }

        [Theory, AutoMoqData]
        public void ParseTwoWordLineWithBonusTiles(DefaultLineParser parser)
        {
            var line = "AXE BEE";

            var words = parser.Parse(line, new int[] { 2, 6 });

            words.Single(x => x.Value == "AXE").BonusTiles.Should().ContainSingle(x => x == 2);
            words.Single(x => x.Value == "BEE").BonusTiles.Should().ContainSingle(x => x == 2);
        }

        [Theory, AutoMoqData]
        public void PreserveBonusTilesIndexesWhenRemovingSpaces(DefaultLineParser parser)
        {
            var line = " AXE  DUCK";

            var words = parser.Parse(line, new int[] { 6 });

            words.Single(x => x.Value == "DUCK").BonusTiles.Should().ContainSingle(x => x == 0);
        }

        [Theory, AutoMoqData]
        public void ReturnsNoWordsWhenLineIsAllSpaces(DefaultLineParser parser)
        {
            var line = "   ";

            var words = parser.Parse(line, new int[] { 6 });

            words.Should().BeEmpty();
        }
    }
}
