using SteamProject.DAL.Abstract;
using SteamProject.Models;

namespace SteamProject.DAL.Concrete;

public class InboxRepository : Repository<InboxMessage>, IInboxRepository
{
    public InboxRepository(SteamInfoDbContext ctx) : base(ctx)
    {

    }
    
    public List<InboxMessage> GetInboxMessages(int userId)
    {
        return GetAll().Where(x => x.RecipientId == userId ).ToList<InboxMessage>();
    }

}