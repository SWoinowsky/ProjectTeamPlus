using SteamProject.Models;

namespace SteamProject.DAL.Abstract;

public interface ITeamRepository : IRepository<Team>
{
    public void CreateTeam(int captain, string name, string motto, string imageUrl);
    public Team GetTeamById(int teamId);
    public List<User> GetTeamMembers(int teamId);
    public void AddUserToTeam(int teamId, int userId);
    public void RemoveUserFromTeam(int userId);
    public void DeleteTeam(int teamId);
    public bool IsUserCaptain(int userId);
    public bool IsUserMember(int userId);
}