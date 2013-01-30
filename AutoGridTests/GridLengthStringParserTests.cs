using System.Linq;
using AutoGridDemo;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoGridTests
{
    [TestClass]
    public class GridLengthStringParserTests
    {
        [TestMethod]
        public void CanHandleSingleStar()
        {
            var g = GridLengthParser.FromString("*").ToArray();
            g.Count().Should().Be(1);
            g.Single().IsStar.Should().BeTrue();
            g.Single().Value.Should().BeInRange(0.99, 1.01);
        }

        [TestMethod]
        public void CanHandleTenStar()
        {
            var g = GridLengthParser.FromString("10*").ToArray();
            g.Count().Should().Be(1);
            g.Single().IsStar.Should().BeTrue();
            g.Single().Value.Should().BeInRange(9.99, 10.01);
        }

        [TestMethod]
        public void CanHandleAuto()
        {
            var g = GridLengthParser.FromString("auto").ToArray();
            g.Count().Should().Be(1);
            g.Single().IsAuto.Should().BeTrue();
        }

        [TestMethod]
        public void CanHandlePixel()
        {
            var g = GridLengthParser.FromString("10").ToArray();
            g.Count().Should().Be(1);
            g.Single().IsAbsolute.Should().BeTrue();
            g.Single().Value.Should().BeInRange(9.99, 10.01);
        }

        [TestMethod]
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