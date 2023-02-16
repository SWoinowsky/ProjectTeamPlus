using System;
using System.Collections.Generic;
using System.Linq;
using SteamProject.Models;
using SteamProject.DAL.Abstract;
using SteamProject.Data;

namespace SteamProject.DAL.Concrete
{
    public class GameRepository : Repository<Game>, IGameRepository
    {
        public GameRepository(SteamInfoDbContext ctx) : base(ctx)
        {
        }
    }
}