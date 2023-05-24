using Newtonsoft.Json;
using SteamProject.DAL.Abstract;
using SteamProject.Models;

namespace SteamProject.DAL.Concrete;

public class TeamRepository : Repository<Team>, ITeamRepository
{
    private readonly SteamInfoDbContext _ctx;
    public TeamRepository(SteamInfoDbContext ctx) : base(ctx)
    {   
        _ctx = ctx;
    }

    public bool IsUserCaptain(int userId)
    {
        return GetAll().Any(s => s.Captain == userId);
    }

    public bool IsUserMember(int userId)
    {
        return _ctx.Users.Where(s => s.Id == userId).First().TeamMember != null;
    }

    public void AddUserToTeam(int teamId, int userId)
    {
        Team teamToAddUserTo = GetAll().Where(s => s.Id == teamId).First();
        User userToAdd = _ctx.Users.Where(s => s.Id == userId).First();
        userToAdd.TeamMember = teamToAddUserTo.Id;
        _ctx.SaveChanges();
    }

    public void CreateTeam(int captain, string name, string motto, string imageUrl)
    {
        Team newTeam = new();
        newTeam.Captain = captain;
        newTeam.Name = name;
        newTeam.Motto = motto;
        newTeam.ImageUrl = imageUrl;
        AddOrUpdate(newTeam);
        AddUserToTeam(newTeam.Id, newTeam.Captain);
    }

    public List<User> GetTeamMembers(int teamId)
    {
        return _ctx.Users.Where(s => s.TeamMember == teamId).Select(u => u).ToList();
    }

    public void DeleteTeam(int teamId)
    {
        Team teamToDelete = GetAll().Where(s => s.Id == teamId).First();
        var teamMembers = _ctx.Users.Where(s => s.TeamMember == teamId);
        foreach (var member in teamMembers)
        {
            member.TeamMember = null;
        }
        Delete(teamToDelete);
    }

    public Team GetTeamById(int teamId)
    {
        return GetAll().Where(s => s.Id == teamId).First();
    }

    public void RemoveUserFromTeam(int userId)
    {
        User userToRemove = _ctx.Users.First(u => u.Id == userId);
        userToRemove.TeamMember = null;
        _ctx.SaveChanges();
    }
}