using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using static System.Text.Json.JsonElement;
using System.Linq;

public class NFLStats
{
    private readonly Dictionary<string, string> headers;
    private readonly Dictionary<int, string> cache;
    private readonly string URI = "https://nfl-team-stats1.p.rapidapi.com/v1/nfl/teamStats";

    public NFLStats()
    {
        cache = new Dictionary<int, string>();
        headers = new Dictionary<string, string>();
        headers["x-rapidapi-host"] = "nfl-team-stats1.p.rapidapi.com";
        headers["x-rapidapi-key"] = "62edfa1f41msha9e7c8696476fc3p1d3806jsna8e876d7f719";
    }

    public string GetDefenseStat(int year, string team, string stat)
    {
        JsonElement teamData = this.LookupTeam(year, team);
        if(!teamData.GetProperty("defense").TryGetProperty(stat, out JsonElement statData))
        {
            throw new ArgumentException($"No such stat {stat} for year {year}.");
        }
        return statData.GetProperty("value").GetString();
    }

    public string GetDefenseRank(int year, string team, string stat)
    {
        JsonElement teamData = this.LookupTeam(year, team);
        if(!teamData.GetProperty("defense").TryGetProperty(stat, out JsonElement statData))
        {
            throw new ArgumentException($"No such stat {stat} for year {year}.");
        }
        return statData.GetProperty("rank").GetString();
    }

    public string GetOffenseStat(int year, string team, string stat)
    {
        JsonElement teamData = this.LookupTeam(year, team);
        if(!teamData.GetProperty("offense").TryGetProperty(stat, out JsonElement statData))
        {
            throw new ArgumentException($"No such stat {stat} for year {year}.");
        }
        return statData.GetProperty("value").GetString();
    }

    public string GetOffenseRank(int year, string team, string stat)
    {
        JsonElement teamData = this.LookupTeam(year, team);
        if(!teamData.GetProperty("offense").TryGetProperty(stat, out JsonElement statData))
        {
            throw new ArgumentException($"No such stat {stat} for year {year}.");
        }
        return statData.GetProperty("rank").GetString();
    }

    public List<string> GetDefenseStatNames(int year)
    {
        string data = this.GetYear(year);
        JsonDocument doc = JsonDocument.Parse(data);
        JsonElement root = doc.RootElement;
        JsonElement stats = root.GetProperty("stats");
        HashSet<string> defenseStats = new HashSet<string>();
        foreach(JsonProperty prop in stats.EnumerateObject()) {
            if (!prop.Value.TryGetProperty("defense", out JsonElement defense)) continue;
            foreach(JsonProperty stat in defense.EnumerateObject()) {
                defenseStats.Add(stat.Name);
            }
        }
        return defenseStats.ToList<string>();
    }
    
    public List<string> GetOffenseStatNames(int year)
    {
        string data = this.GetYear(year);
        JsonDocument doc = JsonDocument.Parse(data);
        JsonElement root = doc.RootElement;
        JsonElement stats = root.GetProperty("stats");
        HashSet<string> defenseStats = new HashSet<string>();
        foreach(JsonProperty prop in stats.EnumerateObject()) {
            if (!prop.Value.TryGetProperty("offense", out JsonElement defense)) continue;
            foreach(JsonProperty stat in defense.EnumerateObject()) {
                defenseStats.Add(stat.Name);
            }
        }
        return defenseStats.ToList<string>();
    }
    public List<string> GetTeamNames(int year)
    {
        string data = this.GetYear(year);
        JsonDocument doc = JsonDocument.Parse(data);
        JsonElement root = doc.RootElement;
        JsonElement stats = root.GetProperty("stats");
        List<string> names = new List<string>();
        foreach(JsonProperty prop in stats.EnumerateObject()) {
            if (!prop.Value.TryGetProperty("record", out JsonElement temp)) continue;
            names.Add(prop.Name);
        }
        return names;
    }

    public string GetGames(int year, string team)
    {
        JsonElement teamData = this.LookupTeam(year, team);
        return teamData.GetProperty("games").GetProperty("g").GetProperty("value").GetString();
    }

    private JsonElement LookupTeam(int year, string team)
    {
        string data = this.GetYear(year);
        JsonDocument doc = JsonDocument.Parse(data);
        JsonElement root = doc.RootElement;
        JsonElement stats = root.GetProperty("stats");
        if (!stats.TryGetProperty(team, out JsonElement teamData))
        {
            throw new ArgumentException($"Invalid Team Name: {team}");
        }
        return teamData;
    }

    public string GetRecord(int year, string team)
    {
        JsonElement teamData = this.LookupTeam(year, team);
        return teamData.GetProperty("record").GetProperty("record").GetProperty("value").GetString();
    }

    public string GetRank(int year, string team)
    {
        JsonElement teamData = this.LookupTeam(year, team);
        return teamData.GetProperty("record").GetProperty("record").GetProperty("rank").GetString();
    }

    public string GetYear(int year)
    {
        if (this.cache.ContainsKey(year)) {
            return this.cache[year];
        }
        string filename = $"stats_db/{year}.json";
        if(File.Exists(filename))
        {
            // Console.WriteLine($"Loading from cache {filename}");
            this.cache[year] = File.ReadAllText(filename);
            return this.cache[year];
        }
        string uri = URI + $"?year={year}";
        // Console.WriteLine($"Calling {uri}");
        string results = this.Get(uri);
        File.WriteAllText(filename, results);
        this.cache[year] = results;
        return results;
    }

    private string Get(string uri)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
        request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
        foreach (string key in this.headers.Keys)
        {
            request.Headers.Add(key, this.headers[key]);
        }


        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        using (Stream stream = response.GetResponseStream())
        using (StreamReader reader = new StreamReader(stream))
        {
            return reader.ReadToEnd();
        }
    }

}