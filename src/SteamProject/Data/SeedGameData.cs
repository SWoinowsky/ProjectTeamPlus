using System.IO;
using System.Linq;
using SteamProject.Models;

namespace SteamProject.Data
{
    public static class SeedGameData
    {
        public static void Seed(SteamInfoDbContext dbContext)
        {
            HashSet<string> genreList = new HashSet<string>();
            string filePath = "Data/Results.csv"; // Specify the path to your .csv file
            try
            {
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

                        // Example: Find existing game by AppId
                        var appId = Int32.Parse(values[1]);
                        var existingGame = dbContext.Games.FirstOrDefault(g => g.AppId == appId);

                        if (existingGame != null)
                        {
                            // Existing game found, update its properties
                            existingGame.Name = values[2];
                            existingGame.IconUrl = values[6];
                            existingGame.Genres = genres;
                            // Update other properties as needed
                        }
                        else
                        {
                            // No existing game found, add a new game
                            var newGame = new Game
                            {
                                AppId = appId,
                                Name = values[2],
                                IconUrl = values[6],
                                Genres = genres
                                // Set other properties as needed
                            };
                            foreach (var genre in genres.Split(","))
                            {
                                // Remove double quotation marks from the genre
                                var cleanedGenre = genre.Trim('"');

                                genreList.Add(cleanedGenre);
                            }
                            dbContext.Games.Add(newGame);
                        }
                    }
                }

                // Update IGDBGenres table
                foreach (var genre in genreList)
                {
                    // Check if the genre already exists in the IGDBGenres table
                    var existingGenre = dbContext.Igdbgenres.FirstOrDefault(g => g.Name == genre);

                    if (existingGenre == null)
                    {
                        // Genre does not exist, add it to the IGDBGenres table
                        var newGenre = new Igdbgenre
                        {
                            Name = genre
                            // Set other properties as needed
                        };
                        dbContext.Igdbgenres.Add(newGenre);
                    }
                }

                dbContext.SaveChanges(); // Save changes to the database
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
            }
        }
    }
}
