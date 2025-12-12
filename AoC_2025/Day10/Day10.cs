using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Xunit;

namespace AoC_2025
{
    public static class Day10
    {
        public class Day10_Input : List<Day10_Machine> //Define input type
        {
        }

        public class Day10_Machine
        {
            public int lightDiagram;
            public List<List<int>> buttonConfigs;
            public List<int> joltageReq;

            public Day10_Machine(string raw)
            {
                var parts = raw.Split(new char[] { '[', ']', '(', ')', '{', '}' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).Where(s => s != "").ToList();
                var revertedCharArray = parts[0].Replace("#", "1").Replace(".", "0").ToCharArray().Reverse().ToArray();
                lightDiagram = Convert.ToInt32(new string(revertedCharArray), 2);
                buttonConfigs = new List<List<int>>();
                for (int i = 1; i < parts.Count - 1; i++)
                {
                    buttonConfigs.Add(parts[i].Split(',').Select(s => int.Parse(s)).ToList());
                }
                joltageReq = parts.Last().Split(',').Select(s => int.Parse(s)).ToList();
            }

            public long Part1()
            {
                var StatesToCheck = new PriorityQueue<(int state, long step), long>();
                StatesToCheck.Enqueue((0, 0), 0);
                var SeenStates = new HashSet<int>();
                while (StatesToCheck.Count > 0)
                {
                    var (state, step) = StatesToCheck.Dequeue();
                   
                    //if (SeenStates.Contains(new (state, step)))
                    //{
                    //    continue;
                    //}
                    SeenStates.Add(state);
                    foreach(var button in buttonConfigs)
                    {
     
                            int newState = state;
                            foreach (var lightIndex in button)
                            {
                                newState ^= (1 << lightIndex);
                            }
                            if (newState == lightDiagram)
                            {
                                return step+1;
                            }

                            if (!SeenStates.Contains(newState))
                            {
                                StatesToCheck.Enqueue((newState, step + 1), step + 1);
                            }
                       
                    }
                }
                return 0;
            }
        }
        public static void Day10_Main()
        {
            var input = Day10_ReadInput();
            Console.WriteLine($"Day10 Part1: {Day10_Part1(input)}");
            Console.WriteLine($"Day10 Part2: {Day10_Part2(input)}");
        }

        public static Day10_Input Day10_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day10\\Day10_input.txt").ReadToEnd();
            }

            var result = new Day10_Input();

            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                result.Add(new Day10_Machine(line));
            }

            return result;
        }


        public static long Day10_Part1(Day10_Input input)
        {
            var result = 0L;
            foreach (var machine in input)
            {
                result += machine.Part1();
            }
            return result;
        }

        public static long Day10_Part2(Day10_Input input)
        {
            return 0;
        }


    }
    public class Day10_Test
    {
        [Theory]
        [InlineData("[.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}\r\n[...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}\r\n[.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}", 7)]
        public static void Day10Part1Test(string rawinput, long expectedValue)
        {
            Assert.Equal(expectedValue, Day10.Day10_Part1(Day10.Day10_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("[.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}\r\n[...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}\r\n[.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}", 33)]
        public static void Day10Part2Test(string rawinput, long expectedValue)
        {
            Assert.Equal(expectedValue, Day10.Day10_Part2(Day10.Day10_ReadInput(rawinput)));
        }
    }
}
