using Newtonsoft.Json;
namespace SteamProject.Models.DTO;

public class MyArray
{
    public int id { get; set; }
    public List<GenreCategory> genreCategory { get; set; }
}

public class GenreCategory
{
    public int id { get; set; }
    public string name { get; set; }
}

public class GenrePOCO
{
    public List<MyArray> MyArray { get; set; }
}
