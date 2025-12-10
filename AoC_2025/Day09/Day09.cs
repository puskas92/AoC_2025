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
                    long area = (long)Math.Abs((long)input[i].X - input[j].X +1) * (long)Math.Abs((long)input[i].Y - input[j].Y +1);
                    maxArea = Math.Max(maxArea, area);
                }
            }
            return maxArea;
        }

        public static (long, long) Day09_Part12(Day09_Input input)
        {
            var maxAreas = new PriorityQueue<(Point start, Point end), long>();
            long maxArea = 0;

            for (var i = 0; i < input.Count - 1; i++)
            {
                for (var j = i + 1; j < input.Count; j++)
                {
                   long area = (long)Math.Abs((long)input[i].X - input[j].X + 1) * (long)Math.Abs((long)input[i].Y - input[j].Y + 1);
                   maxAreas.Enqueue((input[i], input[j]), -area);
                    maxArea = Math.Max(maxArea, area);
                }
            }
            for (var i = 0; i < input.Count - 1; i++)
            {
                if (input[i + 1].X - input[i].X == 1 || input[i+1].Y - input[i].Y == 1)
                {
                    //it is not handled if a section goes into the area, and immediately turn back. Theoretically this is allowed
                    throw new Exception();
                }
            }

                var polygonArea = CalculateArea(input);
            var dir = Math.Sign(polygonArea);
            polygonArea = Math.Abs(polygonArea);

            while (maxAreas.Count > 0)
            {
                var (start, end) = maxAreas.Dequeue();

                //this does not help
                //long area = (long)Math.Abs((long)start.X - end.X + 1) * (long)Math.Abs((long)start.Y - end.Y + 1);
                //if (area > polygonArea) continue; // if the area is larger than the polygon area, it cannot be the answer

                // Check if the rectangle defined by start and end fully inside the polygon
                if (RectangleContainsPolygon(start, end, input)) continue;
                if (RectangleInterSectedByThePolygon(start, end, input)) continue;

                //Check if the rectangle is inside of the polygon
                //var testInput = new Day09_Input();
                //for(var i = input.IndexOf(start); i <= input.IndexOf(end); i++)
                //{
                //    testInput.Add(input[i]);
                //}
                //testInput.Add(new Point(end.X, start.Y));
                //var testArea = CalculateArea(testInput);
                //if (dir == Math.Sign(testArea)) continue;

                long rectangleArea = (long)Math.Abs((long)start.X - end.X + 1) * (long)Math.Abs((long)start.Y - end.Y + 1);
                return (maxArea, rectangleArea); //1550726012 too low
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
                if(pos==start && nextpos==end) continue;
                // Check if the edge intersects with any of the rectangle's edges
                if (SectionsIntersects((pos, nextpos), (new Point(minX, minY), new Point(maxX, minY))) ||
                    SectionsIntersects((pos, nextpos), (new Point(maxX, minY), new Point(maxX, maxY))) ||
                    SectionsIntersects((pos, nextpos), (new Point(maxX, maxY), new Point(minX, maxY))) ||
                    SectionsIntersects((pos, nextpos), (new Point(minX, maxY), new Point(minX, minY))))
                {
                    return true; // The rectangle is intersected by the polygon
                }
            }
            return false; // No intersection found
        }

        private static bool SectionsIntersects((Point start, Point end) section1, (Point start, Point end) section2)
        {
            // Helper function to check orientation
            int Orientation(Point p, Point q, Point r)
            {
                long val = ((long)q.Y - p.Y) * ((long)r.X - q.X) - ((long)q.X - p.X) * ((long)r.Y - q.Y);
                if (val == 0) return 0; // colinear
                return (val > 0) ? 1 : 2; // clock or counterclock wise
            }

            // Helper function to check if point q lies on segment pr
            bool OnSegment(Point p, Point q, Point r)
            {
                return q.X < Math.Max(p.X, r.X) && q.X > Math.Min(p.X, r.X) &&
                       q.Y < Math.Max(p.Y, r.Y) && q.Y > Math.Min(p.Y, r.Y);
            }

            Point p1 = section1.start, q1 = section1.end;
            Point p2 = section2.start, q2 = section2.end;

            int o1 = Orientation(p1, q1, p2);
            int o2 = Orientation(p1, q1, q2);
            int o3 = Orientation(p2, q2, p1);
            int o4 = Orientation(p2, q2, q1);

            // General case
            if(o1!=0 && o2 !=0 && o3 !=0 && o4 !=0)
            {
                if (o1 != o2 && o3 != o4)
                    return true;
            }
           

            // Special Cases
            // p1, q1 and p2 are colinear and p2 lies on segment p1q1
            if (o1 == 0 && OnSegment(p1, p2, q1)) return true;

            // p1, q1 and q2 are colinear and q2 lies on segment p1q1
            if (o2 == 0 && OnSegment(p1, q2, q1)) return true;

            // p2, q2 and p1 are colinear and p1 lies on segment p2q2
            if (o3 == 0 && OnSegment(p2, p1, q2)) return true;

            // p2, q2 and q1 are colinear and q1 lies on segment p2q2
            if (o4 == 0 && OnSegment(p2, q1, q2)) return true;

            return false;
        }

        private static bool RectangleContainsPolygon(Point start, Point end, Day09_Input polygon)
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
                numOfPoints += (Math.Abs(nextpos.X - pos.X) + Math.Abs(nextpos.Y - pos.Y) );
            }
            Debug.Assert(doublearea % 2 == 0);
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
        [InlineData("1,1\r\n10,1\r\n10,2\r\n10,10\r\n8,10\r\n8,2\r\n3,2\r\n3,10\r\n1,10\r\n1,2", 0)]
        [InlineData("1,1\r\n1,2\r\n1,10\r\n3,10\r\n3,2\r\n8,2\r\n8,10\r\n10,10\r\n10,2\r\n10,1", 0)]
        public static void Day09Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day09.Day09_Part12(Day09.Day09_ReadInput(rawinput)).Item2);
        }
    }
}
