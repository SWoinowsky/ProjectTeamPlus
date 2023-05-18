using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using SteamProject.DAL.Abstract;
using SteamProject.Models;
using SteamProject.Models.DTO;
using SteamProject.Services;
using Newtonsoft.Json.Linq;

namespace SteamProject.Controllers;

[Route("api/[controller]")]
[ApiController]

public class MessagesController : ControllerBase
{

    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly IInboxRepository _inboxRepository;
    private readonly IInboxService _inboxService;
    private readonly IFriendRepository _friendRepository;
    public MessagesController(UserManager<IdentityUser> userManager, IUserRepository userRepository, IInboxRepository inboxRepository, IInboxService inboxService, IFriendRepository friendRepository) 
    {
        _userManager = userManager;
        _userRepository = userRepository;
        _inboxRepository = inboxRepository;
        _inboxService = inboxService;
        _friendRepository = friendRepository;
    }

    [HttpGet("messageCount")]
    public ActionResult MessageCount(int messengerId)
    {
        return Ok(_inboxRepository.GetInboxMessages(messengerId).Count);
    }

    [HttpGet("messagesBetween")]
    public ActionResult MessagesBetween(int fromId, int toId)
    {
        List<InboxMessage> messagesBetween = _inboxRepository.GetMessagesBetween(fromId, toId);
        string jString = "{ \"messages\" : [ ";
        foreach (var message in messagesBetween)
        {
            var obj = new JObject
                {
                    {"from", message.SenderId},
                    {"to", message.RecipientId},
                    {"content", message.Content},
                    {"time", message.TimeStamp.Value.ToString("hh:mm tt")}
                };
                jString += obj.ToString()+",";
        }
        jString = jString.Remove(jString.Length - 1);
        jString += "] }";
        return Ok(jString.ToString());
    }

    [HttpPost("sendMessageTo")]
    public ActionResult SendMessageTo(int toId, int fromId, string message)
    {
        _inboxService.SendMessage(toId, fromId, message);
        return Ok();
    }

    [HttpGet("userContacts")]
    public ActionResult VerifiedContacts(int messengerId)
    {
        string jString = "{ \"contacts\" : [ ";
        List<Friend> verified = new();
        List<Friend> friends = _friendRepository.GetFriends(messengerId);
        var steamIds = _userRepository.GetAllUsers().Select(u => u.SteamId);
        var users = _userRepository.GetAllUsers();
        foreach (var friend in friends)
        {
            if (steamIds.Contains(friend.SteamId)) {
                var correctId = users.Where(s => s.SteamId == friend.SteamId).Select(s => s.Id).First();
                var obj = new JObject
                {
                    {"id", correctId},
                    {"avatar", friend.AvatarFullUrl},
                    {"name", friend.SteamName},
                    {"nickname", friend.Nickname}
                };
                jString += obj.ToString()+",";
            }
        }
        jString = jString.Remove(jString.Length - 1);
        jString += "] }";
        return Ok(jString.ToString());
    }

}