using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

public class NFLStats
{
    private readonly Dictionary<string, string> headers;
    private readonly string URI = "https://nfl-team-stats1.p.rapidapi.com/v1/nfl/teamStats";

    public NFLStats()
    {
        headers = new Dictionary<string, string>();
        headers["x-rapidapi-host"] = "nfl-team-stats1.p.rapidapi.com";
        headers["x-rapidapi-key"] = "62edfa1f41msha9e7c8696476fc3p1d3806jsna8e876d7f719";
    }

    public string GetYear(int year)
    {
        string filename = $"stats_db/{year}.json";
        if(File.Exists(filename))
        {
            Console.WriteLine($"Loading from cache {filename}");
            return File.ReadAllText(filename);
        }
        string uri = URI + $"?year={year}";
        Console.WriteLine($"Calling {uri}");
        string results = this.Get(uri);
        File.WriteAllText(filename, results);
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