using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Xunit;

namespace AoC_2025
{
    public static class Day11
    {
        public class Day11_Input : Dictionary<string, HashSet<string>> //Define input type
        {

        }
        public static void Day11_Main()
        {
            var input = Day11_ReadInput();
            Console.WriteLine($"Day11 Part1: {Day11_Part1(input)}");
            Console.WriteLine($"Day11 Part2: {Day11_Part2(input)}");
        }

        public static Day11_Input Day11_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day11\\Day11_input.txt").ReadToEnd();
            }

            var result = new Day11_Input();

            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                var splitted1 = line.Split(':');
                var destinations = splitted1[1].Trim().Split(' ').Select(s => s.Trim());
                result.Add(splitted1[0].Trim(), new HashSet<string>(destinations));
            }

            return result;
        }


        public static int Day11_Part1(Day11_Input input)
        {

            return Day11_RecursiveGetRoutesToOut(input, "you")["out"];
        }

        public static Dictionary<string,int> Day11_RecursiveGetRoutesToOut(Day11_Input input, string startPoint, string exitPoint="out", string skipElement ="")
        {
            if(startPoint == skipElement) return new Dictionary<string, int>();

            if (!input.ContainsKey(startPoint)) return new Dictionary<string, int>();

            var start = input[startPoint];
            var result = new Dictionary<string,int>();
            foreach(var dest in start)
            {
                if (dest == exitPoint)
                {
                    result.Add(dest, 1);
                    continue;
                }
                var subResult = Day11_RecursiveGetRoutesToOut(input, dest, exitPoint, skipElement);
                foreach(var sub in subResult)
                {
                    if(!result.ContainsKey(sub.Key)) result.Add(sub.Key, sub.Value);
                    else result[sub.Key] += sub.Value;
                }
   
            }

            return result;
        }

        public static long Day11_Part2(Day11_Input input)
        {
            int svr_fft = 0;
            int svr_dac = 0;
            int fft_dac = 0;
            int dac_fft = 0;
            int fft_out = 0;
            int dac_out = 0;
           Day11_RecursiveGetRoutesToOut(input, "svr","fft", "dac").TryGetValue("fft", out svr_fft);
             Day11_RecursiveGetRoutesToOut(input, "svr", "dac", "fft").TryGetValue("dac", out svr_dac);
            Day11_RecursiveGetRoutesToOut(input, "fft", "dac", "out").TryGetValue("dac", out fft_dac);
             Day11_RecursiveGetRoutesToOut(input, "dac", "fft", "out").TryGetValue("fft", out dac_fft);
             Day11_RecursiveGetRoutesToOut(input, "fft", "out", "dac").TryGetValue("out", out fft_out);
            Day11_RecursiveGetRoutesToOut(input, "dac", "out", "fft").TryGetValue("out", out dac_out);

            return (long)svr_fft * fft_dac * dac_out + (long)svr_dac * dac_fft * fft_out;
        }


    }
    public class Day11_Test
    {
        [Theory]
        [InlineData("aaa: you hhh\r\nyou: bbb ccc\r\nbbb: ddd eee\r\nccc: ddd eee fff\r\nddd: ggg\r\neee: out\r\nfff: out\r\nggg: out\r\nhhh: ccc fff iii\r\niii: out", 5)]
        public static void Day11Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day11.Day11_Part1(Day11.Day11_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("svr: aaa bbb\r\naaa: fft\r\nfft: ccc\r\nbbb: tty\r\ntty: ccc\r\nccc: ddd eee\r\nddd: hub\r\nhub: fff\r\neee: dac\r\ndac: fff\r\nfff: ggg hhh\r\nggg: out\r\nhhh: out", 2)]
        public static void Day11Part2Test(string rawinput, long expectedValue)
        {
            Assert.Equal(expectedValue, Day11.Day11_Part2(Day11.Day11_ReadInput(rawinput)));
        }
    }
}
