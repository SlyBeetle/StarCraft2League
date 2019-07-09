using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarCraft2League.Constants.Users;
using StarCraft2League.Models;
using StarCraft2League.Services.Interfaces;
using StarCraft2League.ViewModels;

namespace StarCraft2League.Controllers
{
    public partial class LeagueController : Controller
    {
        private ILeagueService _leagueService;
        private LeagueContext _leagueContext;

        public LeagueController(ILeagueService leagueService, LeagueContext leagueContext)
        {
            _leagueService = leagueService;
            _leagueContext = leagueContext;
        }

        [Authorize(Roles = RoleConstants.Admin)]
        [HttpPost]
        public virtual IActionResult Launch([FromForm]SeasonOptions seasonOptions)
        {
            _leagueService.LaunchNewSeason(seasonOptions);
            return RedirectToAction(MVC.Home.Index());
        }

        public virtual IActionResult Seasons() => View(_leagueContext.Seasons);
    }
}