using StarCraft2League.Models.Seasons;
using StarCraft2League.Services.Interfaces;
using System.Linq;
using System.Collections.Generic;

namespace StarCraft2League.Services
{
    public class MatchesStatisticsService : IMatchesStatisticsService
    {
        private readonly IMatchService _matchService;

        public MatchesStatisticsService(IMatchService matchService)
        {
            _matchService = matchService;
        }

        public int GetWins(IEnumerable<Match> matches, int playerId) =>
            matches.Sum(m => _matchService.GetPlayerWins(m, playerId));

        public int GetLosses(IEnumerable<Match> matches, int playerId) =>
            matches.Sum(m => _matchService.GetPlayerLosses(m, playerId));

        public double GetWinrate(int wins, int losses) =>
            (double)wins / (wins + losses) * 100;
    }
}