using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarCraft2League.Extensions;
using StarCraft2League.Models;
using StarCraft2League.Models.Users;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using StarCraft2League.Models.Seasons;
using StarCraft2League.Services.Interfaces;
using StarCraft2League.Constants.Users;
using StarCraft2League.Models.Seasons.Rounds;
using System.Threading.Tasks;

namespace StarCraft2League.Controllers
{
    public partial class SeasonController : Controller
    {
        private LeagueContext _leagueContext;
        private readonly ILeagueService _leagueService;
        private readonly ISeasonService _seasonService;

        public SeasonController(LeagueContext leagueContext, ILeagueService leagueService, ISeasonService seasonService)
        {
            _leagueContext = leagueContext;
            _leagueService = leagueService;
            _seasonService = seasonService;
        }

        [Authorize]
        public virtual IActionResult Register()
        {
            if (!_leagueService.IsStarted)
                throw new InvalidOperationException("League isn't started.");

            _seasonService.Register(User.GetId());

            return RedirectToAction(MVC.Season.Participants());
        }

        [Authorize(Roles = RoleConstants.Admin)]
        public virtual IActionResult RegisterAll()
        {
            if (!_leagueService.IsStarted)
                throw new InvalidOperationException("League isn't started.");

            foreach (User user in _leagueContext.Users)
            {
                _seasonService.Register(user.Id);
                if (_seasonService.ParticipantsWithProfiles.Count() >= 8)
                    break;
            }

            return RedirectToAction(MVC.Season.Participants());
        }

        public virtual IActionResult Details(int id)
        {
            return View(id);
        }

        public virtual IActionResult Participants()
        {
            IEnumerable<User> participants = new User[0];
            if (_leagueService.IsStarted)
                participants = _seasonService.ParticipantsWithProfiles;
            return View(participants);
        }

        public virtual IActionResult Groups(int seasonId)
        {
            IList<Group> groups = new Group[0];
            bool isGroupInSeason(Group g) => g.SeasonId == seasonId;
            if (_leagueService.IsStarted &&
                _leagueContext.Groups.Any(isGroupInSeason))
                groups = _leagueContext.Groups
                    .Include(g => g.UserGroups)
                        .ThenInclude(ug => ug.User)
                            .ThenInclude(u => u.Profile)
                                .ThenInclude(p => p.League)
                    .Where(isGroupInSeason).ToList();
            return View(groups);
        }

        public virtual IActionResult Schedule()
        {
            IList<Group> groupSchedule = new Group[0];            
            if (_leagueService.IsStarted)
            {
                Season currentSeason = _seasonService.Current;
                _leagueContext.Entry(_seasonService.Current).Collection(s => s.Groups).Query()
                    .Include(g => g.Rounds)
                        .ThenInclude(r => r.Matches)
                        .ThenInclude(m => m.FirstPlayer)
                            .ThenInclude(fp => fp.Profile)
                                .ThenInclude(p => p.League)
                    .Include(g => g.Rounds)
                        .ThenInclude(r => r.Matches)
                        .ThenInclude(m => m.SecondPlayer)
                            .ThenInclude(sp => sp.Profile)
                                .ThenInclude(p => p.League).Load();                            
                groupSchedule = currentSeason.Groups.ToList();
            }
            return View(groupSchedule);
        }

        public async virtual Task<IActionResult> Playoffs(int seasonId)
        {
            var roundsList = new List<PlayoffsRound>();
            if (await _leagueContext.PlayoffsRounds.AnyAsync())
            {
                var playoffsRounds = _leagueContext.PlayoffsRounds
                    .Where(pr => pr.SeasonId == seasonId)
                    .Include(p => p.Season)
                    .Include(p => p.Matches)
                        .ThenInclude(m => m.FirstPlayer)
                            .ThenInclude(fp => fp.Profile)
                                .ThenInclude(p => p.League)
                    .Include(p => p.Matches)
                        .ThenInclude(m => m.SecondPlayer)
                            .ThenInclude(sp => sp.Profile)
                                .ThenInclude(p => p.League);
                roundsList = await playoffsRounds.ToListAsync();
            }
            return View(roundsList);
        }
    }
}