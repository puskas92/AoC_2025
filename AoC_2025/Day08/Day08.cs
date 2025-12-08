using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Xunit;

namespace AoC_2025
{
    public static class Day08
    {
        public record Point3D(int X, int Y, int Z);
        public class Day08_Input : List<Point3D> //Define input type
        {
        }
        public static void Day08_Main()
        {
            var input = Day08_ReadInput();
            var results = Day08_Part12(input, 1000);
            Console.WriteLine($"Day08 Part1: {results.Item1}");
            Console.WriteLine($"Day08 Part2: {results.Item2}");
        }

        public static Day08_Input Day08_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day08\\Day08_input.txt").ReadToEnd();
            }

            var result = new Day08_Input();

            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                var splitted = line.Split(',').Select(s => int.Parse(s.Trim())).ToArray();
                result.Add(new Point3D(splitted[0], splitted[1], splitted[2]));
            }

            return result;
        }


        public static (int, long) Day08_Part12(Day08_Input input, int iteration)
        {
            var distanceTable = new PriorityQueue<(Point3D from, Point3D to), long>();

            for(var i = 0; i<input.Count-1; i++)
            {
                for (var j = i+1; j<input.Count; j++)
                {
                    var point1 = input[i];
                    var point2 = input[j];
                    long distance = ((long)point1.X - point2.X)*((long)point1.X - point2.X) + ((long)point1.Y - point2.Y)* ((long)point1.Y - point2.Y) + ((long)point1.Z - point2.Z)* ((long)point1.Z - point2.Z);
                    distanceTable.Enqueue((point1, point2), distance);
                }
            }
     

            var circuits = new List<HashSet<Point3D>>();
            foreach(var point in input)
            {
                circuits.Add(new HashSet<Point3D>() { point });
            }

            var count = 0;
            var part1Result = 0;
            var part2Result = 0;
            while (distanceTable.Count>0)
            {
                var (from, to) = distanceTable.Dequeue();
                count++;

                var circuitsThatContains = circuits.FindAll(f => f.Contains(from) || f.Contains(to));
                switch (circuitsThatContains.Count())
                {
                    case 2:
                        circuitsThatContains[0].UnionWith(circuitsThatContains[1]);
                        circuitsThatContains[0].Add(from);
                        circuitsThatContains[0].Add(to);
                        
                        circuits.Remove(circuitsThatContains[1]);

                        break;
                    case 1:
                        var circuit = circuits.First(f => f.Contains(from) || f.Contains(to));
                       
                        circuit.Add(from);
                        circuit.Add(to);
                   
                        break;
                    case 0:
                        circuits.Add(new HashSet<Point3D>() { from, to });
                        break;
                    default:
                        throw new Exception();
                }
                   

                if(count==iteration) part1Result = circuits.OrderByDescending(f => f.Count).Take(3).Aggregate(1, (acc, val) => acc * val.Count);
                if (circuits.Count == 1) {
                    part2Result = from.X * to.X;
                    break;
                }
            }

            return (part1Result,part2Result);
        }

        public static int Day08_Part2(Day08_Input input)
        {
            return 0;
        }


    }
    public class Day08_Test
    {
        [Theory]
        [InlineData("162,817,812\r\n57,618,57\r\n906,360,560\r\n592,479,940\r\n352,342,300\r\n466,668,158\r\n542,29,236\r\n431,825,988\r\n739,650,466\r\n52,470,668\r\n216,146,977\r\n819,987,18\r\n117,168,530\r\n805,96,715\r\n346,949,466\r\n970,615,88\r\n941,993,340\r\n862,61,35\r\n984,92,344\r\n425,690,689", 40)]
        public static void Day08Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day08.Day08_Part12(Day08.Day08_ReadInput(rawinput),10).Item1);
        }

        [Theory]
        [InlineData("162,817,812\r\n57,618,57\r\n906,360,560\r\n592,479,940\r\n352,342,300\r\n466,668,158\r\n542,29,236\r\n431,825,988\r\n739,650,466\r\n52,470,668\r\n216,146,977\r\n819,987,18\r\n117,168,530\r\n805,96,715\r\n346,949,466\r\n970,615,88\r\n941,993,340\r\n862,61,35\r\n984,92,344\r\n425,690,689", 25272)]
        public static void Day08Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day08.Day08_Part12(Day08.Day08_ReadInput(rawinput),10).Item2);
        }
    }
}
