using System.IO;
using System.Linq;
using SteamProject.Models;

namespace SteamProject.Data
{
    public static class SeedGameData
    {
        public static void Seed(SteamInfoDbContext dbContext)
        {
            string filePath = "Data/Results.csv"; // Specify the path to your .csv file
            try{
                using (var reader = new StreamReader(filePath))
                {
                    reader.ReadLine();
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');

                        // Access the values using the index or column names and perform data manipulation as needed

                        // Concatenate values from index 8 until the end into a single string for the genre string
                        var genres = string.Join(",", values.Skip(8));

                        // Example: Insert data into the table
                        var item = new Game
                        {
                            AppId = Int32.Parse(values[1]),
                            Name = values[2],
                            IconUrl = values[6],
                            Genres = genres
                            // Set other properties as needed
                        };
                        dbContext.Games.Add(item);
                    }
                }

                dbContext.SaveChanges(); // Save changes to the database
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
            }
        }
    }
}