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

    // [HttpGet("messagesTo")]
    // public ActionResult MessagesTo(int messengerId)
    // {
    //     List<InboxMessage> userMessages = _inboxRepository.GetMessagesFor(messengerId);
    //     return Ok(userMessages);
    // }

    [HttpGet("messagesBetween")]
    public ActionResult MessagesTo(int fromId, int messengerId)
    {
        List<InboxMessage> userMessages = _inboxRepository.GetMessagesBetween(fromId, messengerId);
        return Ok(userMessages);
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
        foreach (var friend in friends)
        {
            if (steamIds.Contains(friend.SteamId)) {
                var obj = new JObject
                {
                    {"id", friend.Id},
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