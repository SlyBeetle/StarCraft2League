using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarCraft2League.Extensions;
using StarCraft2League.Models;
using StarCraft2League.Models.Seasons;
using StarCraft2League.Models.Seasons.Rounds;
using StarCraft2League.Models.Users;
using StarCraft2League.Services.Interfaces;
using StarCraft2League.ViewModels;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace StarCraft2League.Controllers
{
    public partial class HomeController : Controller
    {
        private readonly LeagueContext _leagueContext;
        private readonly ISeasonService _seasonService;
        private readonly IMatchesStatisticsService _matchesStatisticsService;

        public HomeController(
            LeagueContext leagueContext,
            ISeasonService seasonService,
            IMatchesStatisticsService matchesStatisticsService
            )
        {
            _leagueContext = leagueContext;
            _seasonService = seasonService;
            _matchesStatisticsService = matchesStatisticsService;
        }

        public virtual IActionResult Index()
        {
            return View(new SeasonOptions());
        }

        public virtual IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        public virtual IActionResult Profile()
        {
            int userId = User.GetId();
            User currentUser = _leagueContext.Users.Find(userId);
            ProfileAndMatches profileAndMatches = new ProfileAndMatches
            {
                Profile = _leagueContext.Profiles.Find(currentUser.ProfileId)
            };
            var currentUserMatches = _leagueContext.Matches
                .Include(m => m.FirstPlayer)
                    .ThenInclude(fp => fp.Profile)
                .Include(m => m.SecondPlayer)
                    .ThenInclude(sp => sp.Profile)
                .Include(m => m.Round)
                .Where(m => m.FirstPlayerId == userId || m.SecondPlayerId == userId);
            if (currentUserMatches.Any())
            {
                profileAndMatches.Matches = currentUserMatches.ToList();
                profileAndMatches.Wins = _matchesStatisticsService.GetWins(profileAndMatches.Matches, userId);
                profileAndMatches.Losses = _matchesStatisticsService.GetLosses(profileAndMatches.Matches, userId);
                profileAndMatches.Winrate =
                    _matchesStatisticsService.GetWinrate(profileAndMatches.Wins, profileAndMatches.Losses);
                foreach (Match match in profileAndMatches.Matches)
                {
                    if (match.Round.IsGroupRound)
                    {
                        GroupRound groupRound = (GroupRound)match.Round;
                        _leagueContext.Entry(groupRound).Reference(gr => gr.Group).Load();
                        if (groupRound.Group.SeasonId == _leagueContext.Seasons.Last().Id)
                            profileAndMatches.CurrentSeasonMatches.Add(match);
                    }
                    else
                    {
                        PlayoffsRound playoffsRound = (PlayoffsRound)match.Round;
                        if (playoffsRound.SeasonId == _leagueContext.Seasons.Last().Id)
                            profileAndMatches.CurrentSeasonMatches.Add(match);
                    }
                }
                profileAndMatches.CurrentSeasonWins =
                    _matchesStatisticsService.GetWins(profileAndMatches.CurrentSeasonMatches, userId);
                profileAndMatches.CurrentSeasonLosses =
                    _matchesStatisticsService.GetLosses(profileAndMatches.CurrentSeasonMatches, userId);
                profileAndMatches.CurrentSeasonWinrate =
                    _matchesStatisticsService.GetWinrate(
                        profileAndMatches.CurrentSeasonWins,
                        profileAndMatches.CurrentSeasonLosses
                        );
            }
            return View(profileAndMatches);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public virtual IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}