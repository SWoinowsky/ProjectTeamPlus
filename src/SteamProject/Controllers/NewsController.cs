using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SteamProject.DAL.Abstract;
using SteamProject.Models;
using SteamProject.Services;
using SteamProject.ViewModels;

namespace SteamProject.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NewsController : Controller
{


    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly IGameRepository _gameRepository;
    private readonly ISteamService _steamService;
    private readonly IOpenAiApiService _openAiApiService;

    public NewsController
    (
        UserManager<IdentityUser> userManager,
        IUserRepository userRepository, 
        IGameRepository gameRepository,
        ISteamService steamService, 
        IOpenAiApiService openAiApiService

    )
    {
        _userManager = userManager;
        _userRepository = userRepository;
        _gameRepository = gameRepository;
        _steamService = steamService;
        _openAiApiService = openAiApiService;
    }

    [HttpGet("GetRecentGameNews")]
    public async Task<IActionResult> GetRecentGameNews(int appId)
    {
        string? id = _userManager.GetUserId(User);

        if (id is null)
        {
            return Json(new { success = false, message = "User not found" });
        }
        else
        {
            User user = _userRepository.GetUser(id);

            if (user.SteamId != null)
            {
                Game game = _gameRepository.GetGameByAppId(appId);
                if (game == null)
                {
                    return BadRequest(new { success = false, message = "Game not found" });
                }

                GameNewsVM currentGame = _steamService.GetGameNews(game, 1);
                string summarizedNews = "";

                if (currentGame._poco.appnews.newsitems.Any())
                {
                    try
                    {
                        summarizedNews = await _openAiApiService.SummarizeNewsShortAsync(currentGame._poco
                            .appnews
                            .newsitems.SingleOrDefault().contents);
                    }
                    catch (NullReferenceException)
                    {
                        summarizedNews = "There was no valid news found";
                    }
                }
                else
                {
                    summarizedNews = "There was no recent news found";
                }

                return Ok(new { appId, summarizedNews });
            }
        }
        return BadRequest(new { success = false, message = "Unable to retrieve game news" });
    }

    [HttpGet("GetSpecificGameNews")]
    public async Task<IActionResult> GetSpecificGameNews(int appId, int newsIndex)
    {
        string? id = _userManager.GetUserId(User);

        if (id is null)
        {
            return Json(new { success = false, message = "User not found" });
        }
        else
        {
            User user = _userRepository.GetUser(id);

            if (user.SteamId != null)
            {
                Game game = _gameRepository.GetGameByAppId(appId);
                if (game == null)
                {
                    return BadRequest(new { success = false, message = "Game not found" });
                }

                GameNewsVM newsVM = _steamService.GetGameNews(game, newsIndex + 1);
                string summarizedNews = "";

                if (newsVM._poco.appnews.newsitems.Count > newsIndex)
                {
                    try
                    {
                        summarizedNews = await _openAiApiService.SummarizeNewsLongAsync(newsVM._poco
                            .appnews
                            .newsitems[newsIndex].contents);
                    }
                    catch (Exception)
                    {
                        summarizedNews = "There was no valid news found";
                    }
                }
                else
                {
                    summarizedNews = "There was no news found for the provided game";
                }

                return Ok(new { appId, summarizedNews });
            }
        }
        return BadRequest(new { success = false, message = "Unable to retrieve game news" });
    }



}