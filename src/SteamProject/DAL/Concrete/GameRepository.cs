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
            return GetAll(g => g.AppId == appId).FirstOrDefault();
        }


        public HashSet<Game> GetGamesListByUserInfo(List<UserGameInfo> userInfo)
        {
            HashSet<Game> uniqueGames = new HashSet<Game>();
            foreach (var game in userInfo)
            {
                Game tempGame = this.GetGameById(game.GameId);
                if (tempGame != null)
                {
                    uniqueGames.Add(tempGame);
                }
            }
            return uniqueGames;
        }

        public Game GetGameById( int id )
        {
            return GetAll().Where( g => g.Id == id ).FirstOrDefault();
        }
    }
}