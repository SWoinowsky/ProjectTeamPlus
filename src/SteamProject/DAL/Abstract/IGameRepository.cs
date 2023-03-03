using SteamProject.DAL.Abstract;
using SteamProject.DAL.Concrete;
using SteamProject.Models;
using SteamProject.ViewModels;

namespace SteamProject.DAL.Abstract
{

    public interface IGameRepository : IRepository<Game>
    {
        Game? GetGameByAppId(int appId);

        List<Game> GetGamesList(List<UserGameInfo> userInfo);

    }
}