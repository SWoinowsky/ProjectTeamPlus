using SteamProject.Models;

namespace SteamProject.ViewModels;

public class TeamsVM
{
    public List<Team> Teams { get; set; }
    public int Id { get; set; }
    public bool IsCaptain { get; set; }
    public bool IsMember { get; set; }
    public TeamsVM(List<Team> teams, int id, bool captain, bool member)
    {
        Teams = teams;
        Id = id;
        IsCaptain = captain;
        IsMember = member;
    }
}