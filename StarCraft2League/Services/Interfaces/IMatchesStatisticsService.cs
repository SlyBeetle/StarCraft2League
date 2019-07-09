using StarCraft2League.Models.Seasons;
using System.Collections.Generic;

namespace StarCraft2League.Services.Interfaces
{
    public interface IMatchesStatisticsService
    {
        int GetWins(IEnumerable<Match> matches, int playerId);
        int GetLosses(IEnumerable<Match> matches, int playerId);
        double GetWinrate(int wins, int losses);
    }
}