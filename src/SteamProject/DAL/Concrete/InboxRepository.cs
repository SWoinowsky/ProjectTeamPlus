using Newtonsoft.Json;
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
        // return GetAll().Where(x => x.RecipientId == userId ).ToList<InboxMessage>();
        return GetAll().ToList<InboxMessage>();
    }

    // public List<InboxMessage> GetMessagesFor(int userId)
    // {
    //     // return GetAll().Where(x => x.RecipientId == userId ).ToList<InboxMessage>();
    //     var query = GetAll().Where(m => int32(m.Sender) == )
    // }

    public List<InboxMessage> GetMessagesBetween(int fromId, int toId)
    {
        var lQuery = GetAll().Where(x => x.SenderId == fromId);
        var rQuery = GetAll().Where(x => x.RecipientId == toId);
        var jQuery = lQuery.Concat(rQuery).OrderBy(s => s.TimeStamp);
        return jQuery.ToList();
    }
}