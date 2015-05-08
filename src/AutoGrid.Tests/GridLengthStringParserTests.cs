using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace AutoGrid.Tests
{
    [TestFixture]
    public class GridLengthStringParserTests
    {
        [Test]
        public void CanHandleSingleStar()
        {
            var g = GridLengthParser.FromString("*").ToArray();
            g.Count().Should().Be(1);
            g.Single().IsStar.Should().BeTrue();
            g.Single().Value.Should().BeInRange(0.99, 1.01);
        }

        [Test]
        public void CanHandleTenStar()
        {
            var g = GridLengthParser.FromString("10*").ToArray();
            g.Count().Should().Be(1);
            g.Single().IsStar.Should().BeTrue();
            g.Single().Value.Should().BeInRange(9.99, 10.01);
        }

        [Test]
        public void CanHandleAuto()
        {
            var g = GridLengthParser.FromString("auto").ToArray();
            g.Count().Should().Be(1);
            g.Single().IsAuto.Should().BeTrue();
        }

        [Test]
        public void CanHandlePixel()
        {
            var g = GridLengthParser.FromString("10").ToArray();
            g.Count().Should().Be(1);
            g.Single().IsAbsolute.Should().BeTrue();
            g.Single().Value.Should().BeInRange(9.99, 10.01);
        }

        [Test]
        public void CanHandleMultiple()
        {
            var g = GridLengthParser.FromString("10,5*,auto,*").ToArray();
            g.Count().Should().Be(4);
            g[0].IsAbsolute.Should().BeTrue("10");
            g[0].Value.Should().BeInRange(9.99, 10.01);

            g[1].IsStar.Should().BeTrue("5*");
            g[1].Value.Should().BeInRange(4.99, 5.01);

            g[2].IsAuto.Should().BeTrue("auto");

            g[3].IsStar.Should().BeTrue("*");
            g[3].Value.Should().BeInRange(0.99, 1.01);
        }

    }
}