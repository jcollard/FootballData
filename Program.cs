using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace FootballData
{
    class Program
    {
        static void Main(string[] args)
        {
            NFLStats stats = new NFLStats();
            
            Console.WriteLine("Enter a year between 1971 and 2021:");
            int year = int.Parse(Console.ReadLine());

            List<string> teams = stats.GetTeamNames(year);

            Console.Clear();
            Console.WriteLine($"Showing Teams from {year}");
            string header = "Team".PadRight(30);
            
            Console.WriteLine($"{header}\tRecord\tRank");
            foreach (string team in teams)
            {
                // Console.WriteLine($"Trying {team}");
                string record = stats.GetRank(year, team);
                string rank = stats.GetRank(year, team);
                string games = stats.GetGames(year, team);
                string tPad = team.PadRight(30);
                Console.WriteLine($"{tPad}\t{record}\t{rank}\t{games}");
            }

            Console.WriteLine("\nEnter a Team Name:");
            string selectedTeam = Console.ReadLine();

            List<string> statNames = stats.GetOffenseStatNames(year);
            
            Console.WriteLine("Select a stat: ");
            Console.WriteLine(String.Join(", ", statNames));
            string selectedStat = Console.ReadLine();
            string stat = stats.GetOffenseStat(year, selectedTeam, selectedStat);
            Console.WriteLine($"In {year} the {selectedTeam}: {stat}!");
        }

    }
}
