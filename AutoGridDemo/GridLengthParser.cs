using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;

namespace AutoGridDemo
{
    public static class GridLengthParser
    {
        private static readonly Regex Star = new Regex(@"(\d*)(\*)");
        private static readonly Regex Pixel = new Regex(@"(\d*)");

        public static IEnumerable<GridLength> FromString(string lengths)
        {
            var l = lengths.Split(',');
            var gl = new List<GridLength>();
            foreach (var ss in l)
            {
                var s = ss.Trim().ToLower();
                if (s == "auto")
                {
                    gl.Add(new GridLength(1, GridUnitType.Auto));
                    continue;
                }
                var star = Star.Match(s);
                if (star.Success)
                {
                    var n = star.Groups[1].Value;
                    gl.Add(String.IsNullOrWhiteSpace(n)
                        ? new GridLength(1, GridUnitType.Star)
                        : new GridLength(Convert.ToDouble(n), GridUnitType.Star));
                    continue;
                }
                var pixel = Pixel.Match(s);
                if (pixel.Success)
                {
                    gl.Add(new GridLength(Convert.ToDouble(s), GridUnitType.Pixel));
                    continue;
                }
                throw new ArgumentException("Invalid grid length in autogrid xaml");
            }
            return gl;
        }
    }
}