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
            var distanceTable = new List<(Point3D from, Point3D to, long distance)>();
            var visitedSet = new HashSet<Point3D>();
          
            foreach (var point in input)
            {
                visitedSet.Add(point);
                foreach (var otherPoint in input)
                {
                    if (visitedSet.Contains(otherPoint)) continue;
              
                    long distance = (long)Math.Pow(point.X - otherPoint.X, 2) + (long)Math.Pow(point.Y - otherPoint.Y, 2) + (long)Math.Pow(point.Z - otherPoint.Z, 2);
                    distanceTable.Add((point, otherPoint, distance));

                }
            }
            
     

            var circuits = new List<List<Point3D>>();
            foreach(var point in input)
            {
                circuits.Add(new List<Point3D>() { point });
            }
            var usedPairs = new HashSet<(Point3D, Point3D)>();

            var count = 0;
            var part1Result = 0;
            var part2Result = 0;
            foreach (var entry in distanceTable.OrderBy(t => t.distance))
            {
                if (usedPairs.Contains((entry.from, entry.to)) || usedPairs.Contains((entry.to, entry.from)))
                {
                    continue;
                }
                usedPairs.Add((entry.from, entry.to));
                count++;

                var circuitsThatContains = circuits.FindAll(f => f.Contains(entry.from) || f.Contains(entry.to));
                switch (circuitsThatContains.Count())
                {
                    case 2:
                        //if (circuitsThatContains[1] is null) throw new Exception;
                        foreach (var circuitelements in circuitsThatContains[1])
                        {
                            circuitsThatContains[0].Add(circuitelements);
                        }
                        if (!circuitsThatContains[0].Contains(entry.from))
                        {
                            circuitsThatContains[0].Add(entry.from);
                        }
                        if (!circuitsThatContains[0].Contains(entry.to))
                        {
                            circuitsThatContains[0].Add(entry.to);
                        }

                        circuits.Remove(circuitsThatContains[1]);

                        break;
                    case 1:
                        var circuit = circuits.First(f => f.Contains(entry.from) || f.Contains(entry.to));
                        if (!circuit.Contains(entry.from))
                        {
                            circuit.Add(entry.from);
                        }
                        if (!circuit.Contains(entry.to))
                        {
                            circuit.Add(entry.to);
                        }
                        break;
                    case 0:
                        circuits.Add(new List<Point3D>() { entry.from, entry.to });
                        break;
                    default:
                        throw new Exception();
                }
                   

                if(count==iteration) part1Result = circuits.OrderByDescending(f => f.Count).Take(3).Aggregate(1, (acc, val) => acc * val.Count);
                if (circuits.Count == 1) {
                    part2Result = entry.from.X * entry.to.X;
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
