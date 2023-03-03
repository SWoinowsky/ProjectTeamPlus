using System;
using System.Collections.Generic;
using System.Linq;
using SteamProject.Models;
using SteamProject.DAL.Abstract;
using SteamProject.Data;
using SteamProject.ViewModels;

namespace SteamProject.DAL.Concrete
{
    public class GameRepository : Repository<Game>, IGameRepository
    {
        public GameRepository(SteamInfoDbContext ctx) : base(ctx)
        {
        }

        public Game? GetGameByAppId(int appId)
        {
            return this.GetAll(g => g.AppId == appId).SingleOrDefault();
        }

        public List<Game> GetGamesList(List<UserGameInfo> userInfo)
        {
            List<Game> returnList = new List<Game>();

            foreach (var game in userInfo)
            {
                Game tempGame = this.FindById(game.GameId);
                returnList.Add(tempGame);
            }
            return returnList;
        }
    }
}