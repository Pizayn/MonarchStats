using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MonarchStatsConsoleApp
{
    public class Program
    {
        private static HttpClient httpClient = new HttpClient();

        public static async Task Main(string[] args)
        {
            try
            {
                var monarchs = await FetchMonarchsAsync();
                var totalMonarchs = GetTotalMonarchs(monarchs);
                Console.WriteLine($"Total Monarchs: {totalMonarchs}");
                var longestReigningMonarch = GetLongestReignMonarch(monarchs);
                Console.WriteLine($"Longest Reigning Monarch: {longestReigningMonarch.Name} ({longestReigningMonarch.ReignLength} years)");
                var longestRulingHouse = GetLongestRulingHouse(monarchs);
                Console.WriteLine($"Longest Ruling House: {longestRulingHouse.House} ({longestRulingHouse.TotalYears} years)");
                var mostCommonFirstName = GetMostCommonFirstName(monarchs);
                Console.WriteLine($"Most Common First Name: {mostCommonFirstName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

        }



        public static async Task<List<Monarch>> FetchMonarchsAsync()
        {
            var url = "https://gist.githubusercontent.com/christianpanton/10d65ccef9f29de3acd49d97ed423736/raw/b09563bc0c4b318132c7a738e679d4f984ef0048/kings";
            try
            {
                var response = await httpClient.GetStringAsync(url);
                var monarchs = JsonConvert.DeserializeObject<List<Monarch>>(response);
                return monarchs ?? new List<Monarch>(); 
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return new List<Monarch>();
            }
            catch (JsonException e)
            {
                Console.WriteLine($"Parsing error: {e.Message}");
                return new List<Monarch>();
            }
        }

        public static int GetTotalMonarchs(List<Monarch> monarchs)
        {
            return monarchs.Count();
        }

        public static (string Name, int ReignLength) GetLongestReignMonarch(List<Monarch> monarchs)
        {
            return monarchs.Select(m => new { m.Nm, m.ReignLength })
                           .OrderByDescending(m => m.ReignLength) // Finds the monarch with the longest reign.
                           .Select(m => (m.Nm, m.ReignLength))
                           .FirstOrDefault();
        }

        public static (string House, int TotalYears) GetLongestRulingHouse(List<Monarch> monarchs)
        {
            return monarchs.GroupBy(m => m.Hse) // Groups monarchs by house.
                           .Select(g => new { House = g.Key, TotalYears = g.Sum(m => m.ReignLength) }) // Calculates total years for each house.
                           .OrderByDescending(g => g.TotalYears)
                           .Select(g => (g.House, g.TotalYears))
                           .FirstOrDefault();
        }

        public static string GetMostCommonFirstName(List<Monarch> monarchs)
        {
            return monarchs.Select(m => m.Nm.Split(' ')[0])
                           .GroupBy(n => n) // Groups monarchs by first name.
                           .OrderByDescending(g => g.Count()) // Orders by the number of occurrences.
                           .Select(g => g.Key) // Selects the most common first name.
                           .FirstOrDefault();
        }
    }
}


public class Monarch
{
    // Unique identifier for the Monarch.
    public int Id { get; set; }

    // Name of the Monarch.
    public string Nm { get; set; }

    // Country the Monarch ruled.
    public string Cty { get; set; }

    // House or dynasty of the Monarch.
    public string Hse { get; set; }

    // Years of reign in a string format, expected to be in "startYear-endYear" format.
    public string Yrs { get; set; }

    // Calculated property to get the length of the reign in years.
    public int ReignLength
    {
        get
        {
            // Check if Yrs is null or empty, return 0 if it is.
            if (string.IsNullOrEmpty(Yrs))
            {
                return 0;
            }

            // Split the Yrs string into parts using '-' as separator.
            var parts = Yrs.Split('-');

            // Try to parse the first part as the start year.
            // If parsing fails, return 0.
            if (!int.TryParse(parts[0], out int startYear))
            {
                return 0;
            }

            int endYear;

            // Check if there are two parts and try to parse the second part as end year.
            // If parsing fails or there's only one part, use current year as end year.
            if (parts.Length > 1 && int.TryParse(parts[1], out endYear))
            {
            }
            else
            {
                endYear = (parts.Length == 1) ? startYear : DateTime.Now.Year;
            }

            // Calculate the length of the reign and return it.
            return endYear - startYear;
        }
    }
}
