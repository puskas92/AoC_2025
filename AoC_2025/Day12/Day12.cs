using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Xunit;

namespace AoC_2025
{
    public static class Day12
    {
        public class Day12_Input
        {
            public List<int> Shapes;
            public List<Day12_Regions> Regions;

            public Day12_Input()
            {
                Shapes = new List<int>();
                Regions = new List<Day12_Regions>();
            }
        }

        public class Day12_Regions
        {
            public int width;
            public int height;
            public List<int> Values;
            public Day12_Regions(string raw)
            {
               var split1 = raw.Split(':');
                width = int.Parse(split1[0].Split('x')[0]);
                height = int.Parse(split1[0].Split('x')[1]);
                Values = new List<int>(split1[1].Trim().Split(' ').Select(f=> int.Parse(f)));
            }

            public override string ToString()
            {
                return $"{width}x{height}: {string.Join(" ", Values)}";
            }
        }
        public static void Day12_Main()
        {
            var input = Day12_ReadInput();
            Console.WriteLine($"Day12 Part1: {Day12_Part1(input)}");
        }

        public static Day12_Input Day12_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day12\\Day12_input.txt").ReadToEnd();
            }

            var result = new Day12_Input();

            var blocks = rawinput.Split(new string[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.RemoveEmptyEntries);
            for(var i =0; i< blocks.Length -1; i++)
            {
                result.Shapes.Add(blocks[i].Count(f=> f == '#'));
            }
            foreach (var region in blocks[6].Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                result.Regions.Add(new Day12_Regions(region));
            }

            return result;
        }


        public static int Day12_Part1(Day12_Input input)
        {
            var result = 0;
            foreach (var region in input.Regions)
            {
                var regionArea = region.width * region.height;

                var minPixel = 0;
                for (var i = 0; i < region.Values.Count; i++)
                {
                    minPixel += region.Values[i] * input.Shapes[i];
                }         
                if (minPixel > regionArea) continue;

                var maxPixel = 0;
                for (var i = 0; i < region.Values.Count; i++)
                {
                    maxPixel += region.Values[i] * 9;
                }
                if (maxPixel <= regionArea)
                {
                    result += 1;
                    continue;
                }

                Console.WriteLine("To check manually: " + region.ToString() + " Area: " + regionArea + " pixel: " + minPixel + " maxPixel: " + maxPixel );

            }
            return result;
        }
    }
}
