using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SteamProject.DAL.Abstract;
using SteamProject.Models;
using SteamProject.Helpers;
using SteamProject.Models.DTO;
using SteamProject.Services;
using SteamProject.ViewModels;
using SteamProject.DAL.Concrete;
using Newtonsoft.Json.Linq;


namespace SteamProject.Controllers;

public class TeamsController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly IInboxService _inboxService;
    private readonly ITeamRepository _teamRepository;


    public TeamsController(UserManager<IdentityUser> userManager, IUserRepository userRepository, ITeamRepository teamRepository, IInboxService inboxService)
    {
        _userManager = userManager;
        _userRepository =  userRepository;
        _teamRepository = teamRepository;
        _inboxService = inboxService;
    }

    [Authorize]
    public IActionResult Index()
    {
        string? id = _userManager.GetUserId(User);
        if (id is null)
        {
            return View();
        }
        else {
            User user = _userRepository.GetUser(id);
            List<Team> currentTeams = _teamRepository.GetAll().ToList();
            bool isCaptain = _teamRepository.IsUserCaptain(user.Id);
            bool isMember = _teamRepository.IsUserMember(user.Id);
            TeamsVM teamsVm = new(currentTeams, user.Id, isCaptain, isMember);
            return View(teamsVm);
        }
    }

    // [HttpGet("sendRequest")]
    // public ActionResult SendJoinRequest(int captain, int userId)
    // {
    //     string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
    //     var name = _userRepository.FindById(userId).SteamName;
    //     var teamId = _userRepository.FindById(captain).TeamMember.Value;
    //     var teamName = _teamRepository.GetTeamById(teamId).Name;
    //     _inboxService.SendMessage(captain, 69420, $"{name} wants to join {teamName}! Click <a href='{baseUrl}/addToTeam?teamId={teamId}&userId={userId}'>here</a> to accept their request!");
    //     return Ok();
    // }

    [HttpPatch("addToTeam")]
    public ActionResult AddUserToTeam(int teamId, int userId)
    {
        var count = _teamRepository.GetTeamMembers(teamId).Count();
        if (count < 5) {
            _teamRepository.AddUserToTeam(teamId, userId);
        }
        return Ok();
    }

    [HttpPost("createTeam")]
    public ActionResult CreateTeam(int captain, string name, string motto, string imageUrl)
    {
        _teamRepository.CreateTeam(captain, name, motto, imageUrl);
        return Ok();
    }

    [HttpGet("getMembers")]
    public ActionResult GetTeamMembers(int teamId)
    {
        string jString = "{ \"members\" : [ ";
        List<User> memberList = _teamRepository.GetTeamMembers(teamId);
        foreach (User member in memberList)
        {
        var obj = new JObject
            {
                {"id", member.Id},
                {"name", member.SteamName},
                {"avatar", member.AvatarUrl},
                {"level", member.PlayerLevel},
            };
            jString += obj.ToString()+",";
        }
            
        jString = jString.Remove(jString.Length - 1);
        jString += "] }";
        return Ok(jString.ToString());
    }

    [HttpPatch("deleteTeam")]
    public ActionResult DeleteTeam(int teamId)
    {
        _teamRepository.DeleteTeam(teamId);
        return Ok();
    }

    [HttpPatch("removeFromTeam")]
    public ActionResult RemoveUserFromTeam(int userId)
    {
        _teamRepository.RemoveUserFromTeam(userId);
        return Ok();
    }
}
