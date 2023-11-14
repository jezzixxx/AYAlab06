using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public struct Weather
{
    private string _country, _name, _description;
    private double _temp;
    public Weather(string country, string name, double temp, string description)
    {
        _country = country;
        _name = name;
        _temp = temp;
        _description = description;
    }
    public string Country { get { return _country; } }
    public double Temp { get { return _temp; } }
    public string Name { get { return _name; } }
    public string Description { get { return _description; } }
    public override string ToString()
    {
        return $"{Country} {Name} {Temp} {Description}";
    }
}


internal class Task1
{
    private static async Task Main()
    {
        double par1, par2;
        Random rand = new Random();
        uint n = 0;
        List<Weather> list = new List<Weather>();
        while (n < 50)
        {
            par1 = rand.Next(180) - 90 + (rand.Next(100) * 0.01);
            par2 = rand.Next(360) - 180 + (rand.Next(100) * 0.01);
            string html = await GetHtmlContent(par1, par2);
            JObject jObject = JObject.Parse(html);
            Console.WriteLine(n);
            if (jObject["name"].ToString() != "" && jObject["sys"]["country"] != null)
            {
                ++n;
                list.Add(new Weather(jObject["sys"]["country"].ToString(), jObject["name"].ToString(), Convert.ToDouble(jObject["main"]["temp"]), jObject["weather"][0]["description"].ToString()));
            }
        }
        foreach (Weather weather in list) { Console.WriteLine(weather); }
        double max_temp = list.Max(p => p.Temp);
        double min_temp = list.Min(p => p.Temp);
        string max_country = list.First(p => p.Temp == max_temp).Country;
        string min_country = list.First(p => p.Temp == min_temp).Country;
        Console.WriteLine($"Country with min temp ({min_temp}): {min_country}");
        Console.WriteLine($"Country with max temp ({max_temp}): {max_country}");
        double average_temp = list.Average(p => p.Temp);
        Console.WriteLine($"Average temp = {average_temp}");
        int count_countries = list.GroupBy(p => p.Country).Count();
        Console.WriteLine($"Num of countries: {count_countries}");
        Console.WriteLine($"First clear sky: {list.FirstOrDefault(p => p.Description == "clear sky").Country} {list.FirstOrDefault(p => p.Description == "clear sky").Name}");
        Console.WriteLine($"First rain: {list.FirstOrDefault(p => p.Description == "rain").Country} {list.FirstOrDefault(p => p.Description == "rain").Name}");
        Console.WriteLine($"First few clouds: {list.FirstOrDefault(p => p.Description == "few clouds").Country} {list.FirstOrDefault(p => p.Description == "few clouds").Name}");
    }
    private static Task<string> GetHtmlContent(double par1, double par2)
    {
        HttpClient client = new HttpClient();
        const string API = "da5caa93fd2b4350a6eeab276cb3e9bd";
        client.BaseAddress = new Uri($"https://api.openweathermap.org/data/2.5/weather?lat={par1}&lon={par2}&appid={API}");
        return client.GetStringAsync(client.BaseAddress);
    }
}