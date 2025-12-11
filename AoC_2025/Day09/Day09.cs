using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Xunit;

namespace AoC_2025
{
    public static class Day09
    {
        public class Day09_Input : List<Point> //Define input type
        {
        }
        public static void Day09_Main()
        {
            var input = Day09_ReadInput();
            var results = Day09_Part12(input);
            Console.WriteLine($"Day09 Part1: {results.Item1}");
            Console.WriteLine($"Day09 Part2: {results.Item2}");
        }

        public static Day09_Input Day09_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day09\\Day09_input.txt").ReadToEnd();
            }

            var result = new Day09_Input();

            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                var split = line.Split(',');
                result.Add(new Point(int.Parse(split[0]), int.Parse(split[1])));
            }

            return result;
        }


        public static long Day09_Part1(Day09_Input input)
        {
            long maxArea = 0;
            for(var i = 0; i < input.Count-1; i++)
            {
                for(var j = i+1; j < input.Count; j++)
                {
                    long area = CalculateRectangleArea(input[i], input[j]);
                    maxArea = Math.Max(maxArea, area);
                }
            }
            return maxArea;
        }

        private static long CalculateRectangleArea(Point p1, Point p2 )
        {
            return ((long)Math.Abs((long)p1.X - (long)p2.X) + 1) * ((long)Math.Abs((long)p1.Y - (long)p2.Y) + 1);
        }

        public static (long, long) Day09_Part12(Day09_Input input)
        {
            var maxAreas = new PriorityQueue<(Point start, Point end), long>();
            long maxArea = 0;

            for (var i = 0; i < input.Count-1; i++)
            {
                for (var j =i+1; j < input.Count; j++)
                {
                    long area = CalculateRectangleArea(input[i], input[j]);
                    maxAreas.Enqueue((input[i], input[j]), -area);
                    maxArea = Math.Max(maxArea, area);
                }
            }


#if DEBUG //this part is not needed for the solution, but it is required to generally solve any input correctly
            for (var i = 0; i < input.Count - 1; i++)
            {
                if (input[i + 1].X - input[i].X == 1 || input[i + 1].Y - input[i].Y == 1)
                {
                    //it is not handled if a section goes into the area, and immediately turn back. Theoretically this is allowed
                    throw new Exception();
                }
            }

            var polygonArea = CalculateArea(input);
            var dir = Math.Sign(polygonArea);
            polygonArea = Math.Abs(polygonArea);

#endif

            while (maxAreas.Count > 0)
            {
                var (start, end) = maxAreas.Dequeue();

#if DEBUG //this does not help on the given input, but generally it could speed up the process
                long area = CalculateRectangleArea(start, end);
                if (area > polygonArea) continue; // if the area is larger than the polygon area, it cannot be the answer
#endif

                // Check if the rectangle defined by start and end fully inside the polygon
                if (Day09_RectangleContainsPolygon(start, end, input)) continue;
                if (RectangleInterSectedByThePolygon(start, end, input)) continue; // TODO: section that goes through the area from edge to edge are not detected (e.g. test cases)

#if DEBUG //this is not needed for the solution, but it is required to generally solve any input correctly
                //Check if the rectangle is inside of the polygon
                var testInput = new Day09_Input();
                for (var i = input.IndexOf(start); i <= input.IndexOf(end); i++)
                {
                    testInput.Add(input[i]);
                }
                var testArea = CalculateArea(testInput);
                if (dir != Math.Sign(testArea)) continue;
#endif

                long rectangleArea = CalculateRectangleArea(start, end);
                return (maxArea, rectangleArea);
            }

            return (maxArea, 0);
        }

        private static bool RectangleInterSectedByThePolygon(Point start, Point end, Day09_Input polygon)
        {
            int minX = Math.Min(start.X, end.X);
            int maxX = Math.Max(start.X, end.X);
            int minY = Math.Min(start.Y, end.Y);
            int maxY = Math.Max(start.Y, end.Y);
            for (var i = 0; i < polygon.Count; i++)
            {
                var pos = polygon[i];
                var nextpos = polygon[(i + 1) % polygon.Count];
                //if(pos==start && nextpos==end) continue;
                // Check if the edge intersects with any of the rectangle's edges
                if (Day09_SectionsIntersects((pos, nextpos), (new Point(minX, minY), new Point(maxX, minY))) ||
                    Day09_SectionsIntersects((pos, nextpos), (new Point(maxX, minY), new Point(maxX, maxY))) ||
                    Day09_SectionsIntersects((pos, nextpos), (new Point(maxX, maxY), new Point(minX, maxY))) ||
                    Day09_SectionsIntersects((pos, nextpos), (new Point(minX, maxY), new Point(minX, minY))))
                {
                    return true; // The rectangle is intersected by the polygon
                }
            }
            return false; // No intersection found
        }

        public static bool Day09_SectionsIntersects((Point start, Point end) section1, (Point start, Point end) section2)
        {
            if (section1.start.X == section1.end.X && section2.start.X == section2.end.X)
            {
                // Both vertical
                return false;
            }
            else if(section1.start.Y == section1.end.Y && section2.start.Y == section2.end.Y)
            {
                // Both horizontal
                return false;
            }
            else if (section1.start.X == section1.end.X)
            {
                // section1 is vertical, section2 is horizontal
                return (Math.Min(section2.start.X, section2.end.X) < section1.start.X && section1.start.X < Math.Max(section2.start.X, section2.end.X)) &&
                       (Math.Min(section1.start.Y, section1.end.Y) < section2.start.Y && section2.start.Y < Math.Max(section1.start.Y, section1.end.Y));
            }
            else
            {
                // section1 is horizontal, section2 is vertical
                return (Math.Min(section1.start.X, section1.end.X) < section2.start.X && section2.start.X < Math.Max(section1.start.X, section1.end.X)) &&
                       (Math.Min(section2.start.Y, section2.end.Y) < section1.start.Y && section1.start.Y < Math.Max(section2.start.Y, section2.end.Y));
            }

            //return false;
        }

        public static bool Day09_RectangleContainsPolygon(Point start, Point end, Day09_Input polygon)
        {
            int minX = Math.Min(start.X, end.X);
            int maxX = Math.Max(start.X, end.X);
            int minY = Math.Min(start.Y, end.Y);
            int maxY = Math.Max(start.Y, end.Y);
            foreach (var point in polygon)
            {
                if (point.X > minX && point.X < maxX && point.Y > minY && point.Y < maxY)
                {
                    return true; // A vertex of the polygon is inside the rectangle
                }
            }
            return false; // All vertices of the polygon are outside the rectangle
        }

     
        private static long CalculateArea(Day09_Input input)
        {

            long doublearea = 0;
            long numOfPoints = 0;
            for (var i = 0; i < input.Count; i++)
            {
                var pos = input[i];
                var nextpos = input[(i + 1) % input.Count];
                doublearea += (long)pos.X * nextpos.Y;
                doublearea -= (long)nextpos.X * pos.Y;
                numOfPoints += (Math.Abs(nextpos.X - pos.X) + Math.Abs(nextpos.Y - pos.Y));
            }
            //Debug.Assert(doublearea % 2 == 0); only valid when there is no diagonal
            return (long)(doublearea / 2) + numOfPoints / 2 + 1;
        }
    }
    public class Day09_Test
    {
        [Theory]
        [InlineData("7,1\r\n11,1\r\n11,7\r\n9,7\r\n9,5\r\n2,5\r\n2,3\r\n7,3", 50)]
        public static void Day09Part1Test(string rawinput, long expectedValue)
        {
            Assert.Equal(expectedValue, Day09.Day09_Part1(Day09.Day09_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("7,1\r\n11,1\r\n11,7\r\n9,7\r\n9,5\r\n2,5\r\n2,3\r\n7,3", 24)]
        [InlineData("1,1\r\n10,1\r\n10,3\r\n10,10\r\n7,10\r\n7,3\r\n3,3\r\n3,10\r\n1,10\r\n1,3", 24)]
        [InlineData("1,1\r\n1,3\r\n1,10\r\n3,10\r\n3,3\r\n7,3\r\n7,10\r\n10,10\r\n10,3\r\n10,1", 24)]
        public static void Day09Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day09.Day09_Part12(Day09.Day09_ReadInput(rawinput)).Item2);
        }

        [Theory]
        [InlineData(1, 1, 10, 10, 2, 2, true)]
        [InlineData(1, 1, 10, 10, 9, 9, true)]
        [InlineData(1, 1, 10, 10, -1, 2, false)]
        [InlineData(1, 1, 10, 10, 2, -1, false)]
        [InlineData(1, 1, 10, 10, -1, -1, false)]
        [InlineData(1, 1, 10, 10, 1, 1, false)]
        [InlineData(1, 1, 10, 10, 1, 10, false)]
        [InlineData(1, 1, 10, 10, 10, 1, false)]
        [InlineData(1, 1, 10, 10, 10, 10, false)]
        public static void Day09_RectangleContainsPolygonTest(int RectStartX, int RectStartY, int RectEndX, int RectEndY, int PointToTestX, int PointToTestY, bool expectedResult)
        {
            Assert.Equal(expectedResult, Day09.Day09_RectangleContainsPolygon(new Point(RectStartX, RectStartY), new Point(RectEndX, RectEndY), new Day09.Day09_Input { new Point(PointToTestX, PointToTestY) }));
        }

        [Theory]
        [InlineData(1, 1, 1, 10, 2, 1, 2, 10, false)]
        [InlineData(1, 1, 10, 1, 1, 2, 10, 2, false)]
        [InlineData(1, 1, 1, 10, 0, 5, 2, 5, true)]
        [InlineData(1, 1, 10, 1, 5, 0, 5, 2, true)]
        [InlineData(1, 1, 1, 10, 1, 5, 2, 5, false)]
        [InlineData(1, 1, 10, 1, 5, 1, 5, 2, false)]
        [InlineData(1, 1, 1, 10, 0, 5, 1, 5, false)]
        [InlineData(1, 1, 10, 1, 5, 0, 5, 1, false)]
        [InlineData(1, 1, 10, 1, 2, 1, 3, 1, false)]
        [InlineData(1, 1, 10, 1, 1, 1, 3, 1, false)]
        [InlineData(1, 1, 10, 1, 0, 1, 3, 1, false)]
        [InlineData(1, 1, 10, 1, 2, 1, 10, 1, false)]
        [InlineData(1, 1, 10, 1, 2, 1, 11, 1, false)]
        [InlineData(1, 1, 10, 1, 0, 1, 11, 1, false)]
        [InlineData(1, 1, 1, 10, 1, 2, 1, 3, false)]
        [InlineData(1, 1, 1, 10, 1, 1, 1, 3, false)]
        [InlineData(1, 1, 1, 10, 1, 0, 1, 3, false)]
        [InlineData(1, 1, 1, 10, 1, 2, 1, 10, false)]
        [InlineData(1, 1, 1, 10, 1, 2, 1, 11, false)]
        [InlineData(1, 1, 1, 10, 1, 0, 1, 11, false)]
        public static void Day09_SectionsIntersects(int Section1SX, int Section1SY, int Section1EX, int Section1EY, int Section2SX, int Section2SY, int Section2EX, int Section2EY, bool expectedResult)
        {
            Assert.Equal(expectedResult, Day09.Day09_SectionsIntersects((new Point(Section1SX, Section1SY), new Point(Section1EX, Section1EY)), (new Point(Section2SX, Section2SY), new Point(Section2EX, Section2EY))));
        }
    }
}
