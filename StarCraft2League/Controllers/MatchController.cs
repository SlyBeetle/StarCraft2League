using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarCraft2League.Extensions;
using StarCraft2League.Models;
using StarCraft2League.Models.Seasons;

namespace StarCraft2League.Controllers
{
    public partial class MatchController : Controller
    {
        private readonly LeagueContext _leagueContext;

        public MatchController(LeagueContext leagueContext)
        {
            _leagueContext = leagueContext;
        }

        [Authorize]
        public virtual ActionResult Edit(int id)
        {
            Match match = LoadMatchWithPlayers(id);
            if (User.IsSimpleUser() &&
                match.FirstPlayerId != User.GetId() &&
                match.SecondPlayerId != User.GetId())
                return Unauthorized();
            return View(match);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit([FromForm]Match match)
        {
            if (!ModelState.IsValid)
            {
                match = LoadMatchWithPlayers(match.MatchId);
                return View(match);
            }

            _leagueContext.Attach(match);
            _leagueContext.Entry(match).Property(m => m.FirstPlayerWins).IsModified = true;
            _leagueContext.Entry(match).Property(m => m.SecondPlayerWins).IsModified = true;
            _leagueContext.SaveChanges();
            return RedirectToAction(MVC.Season.Schedule());
        }

        private Match LoadMatchWithPlayers(int id)
        {
            Match match = _leagueContext.Matches.Find(id);
            _leagueContext.Entry(match).Reference(m => m.FirstPlayer).Load();
            _leagueContext.Entry(match).Reference(m => m.SecondPlayer).Load();
            return match;
        }
    }
}