using System;
using System.Collections.Generic;
using System.Linq;
using SteamProject.Models;
using SteamProject.DAL.Abstract;

namespace SteamProject.DAL.Concrete
{
    public class GameRepository : Repository<Game>, IGameRepository
    {
        public GameRepository(SteamInfoDbContext ctx) : base(ctx)
        {
        }

        public IEnumerable<Game> GetGames(string userId)
        {
            var gameList = new List<Game>();
            return null;
        }
    }
}