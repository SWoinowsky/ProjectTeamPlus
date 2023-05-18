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
        return GetAll().Where(m => m.RecipientId == userId).ToList<InboxMessage>();
    }

    public List<InboxMessage> GetMessagesBetween(int fromId, int toId)
    {
        var lQuery = GetAll().Where(x => x.SenderId == fromId).Where(y => y.RecipientId == toId);
        var rQuery = GetAll().Where(x => x.SenderId == toId).Where(y => y.RecipientId == fromId);
        var jQuery = lQuery.Concat(rQuery);
        jQuery = jQuery.OrderBy(i => i.TimeStamp);
        return jQuery.ToList();
    }
}