using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Puzzle10
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../input.txt");
            string[] lines = File.ReadAllLines(inputFilePath);

            List<Bot> AllBots = new List<Bot>();
            List<OutputBin> Outputs = new List<OutputBin>();
            // There can only be as many bots as there are lines, so let's generate that many bots and outputs
            for(int i=0;i<lines.Length;i++)
            {
                AllBots.Add(new Bot(i));
                Outputs.Add(new OutputBin(i));
            }

            // Initialize each bot's instructions
            foreach (string instruction in lines)
            {
                // Example instructions
                //bot 26 gives low to bot 131 and high to bot 149
                //bot 68 gives low to output 7 and high to bot 87
                //value 61 goes to bot 119

                var give = Regex.Match(instruction, @"^bot ([0-9]+) gives low to (bot|output) ([0-9]+) and high to (bot|output) ([0-9]+)");
                var begin = Regex.Match(instruction, @"^value ([0-9]+) goes to bot ([0-9]+)");
                if (give.Success)
                {
                    int botId = int.Parse(give.Groups[1].Value);
                    int lowId = int.Parse(give.Groups[3].Value);
                    bool isLowOutput = give.Groups[2].Value == "output";
                    int highId = int.Parse(give.Groups[5].Value);
                    bool isHighOutput = give.Groups[4].Value == "output";

                    AllBots[botId].LowerLocation = lowId;
                    AllBots[botId].IsLowerOutput = isLowOutput;
                    AllBots[botId].HigherLocation = highId;
                    AllBots[botId].IsHigherOutput = isHighOutput;
                }
                else if (begin.Success)
                {
                    int value = int.Parse(begin.Groups[1].Value);
                    int botId = int.Parse(begin.Groups[2].Value);

                    AllBots[botId].Inventory.Add(value);
                }
                else
                {
                    throw new Exception("Unknown instruction!");
                }
            }

            // Alright now we are ready to get the bots to begin moving.
            while(AllBots.Any(b=>b.Inventory.Count>=2))
            {
                foreach(var bot in AllBots)
                {
                    bot.Execute(AllBots, Outputs);
                }
            }

            foreach(var output in Outputs)
            {
                if(output.Value != null)
                {
                    Console.WriteLine("Output bin " + output.Id + " contains chips of value " + output.Value + ".");
                }
            }

            Console.ReadLine();
        }
    }

    public class Bot
    {
        public int Id { get; set; }

        public int HigherLocation { get; set; }
        public bool IsHigherOutput { get; set; }

        public int LowerLocation { get; set; }
        public bool IsLowerOutput { get; set; }

        public List<int> Inventory { get; set; }

        public Bot(int id)
        {
            this.Id = id;
            this.Inventory = new List<int>();
        }

        public void Execute(List<Bot> AllBots, List<OutputBin> Outputs)
        {
            if(Inventory.Count == 2)
            {
                // Only activate if this bot has 2 chips in inventory
                int lower = Inventory.Min();
                int higher = Inventory.Max();

                if(lower == 17 && higher == 61)
                {
                    Console.WriteLine("Bot " + Id + " is responsible for comparing value 61 and value 17 chips.");
                }

                if(IsHigherOutput)
                {
                    Outputs.Single(o=>o.Id == HigherLocation).Value = higher;
                }
                else
                {
                    AllBots.Single(b => b.Id == HigherLocation).Inventory.Add(higher);
                }
                if(IsLowerOutput)
                {
                    Outputs.Single(o=>o.Id == LowerLocation).Value = lower;
                }
                else
                {
                    AllBots.Single(b => b.Id == LowerLocation).Inventory.Add(lower);
                }

                Inventory.Clear();
            }
            if(Inventory.Count>=3)
            {
                throw new Exception("More inventory than the bot can handle!");
            }
        }
    }

    public class OutputBin
    {
        public int Id { get; set; }
        public int? Value { get; set; }

        public OutputBin(int id)
        {
            this.Id = id;
        }
    }
}
