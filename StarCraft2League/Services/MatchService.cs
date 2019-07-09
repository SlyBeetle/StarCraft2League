using StarCraft2League.Models;
using StarCraft2League.Models.Seasons;
using StarCraft2League.Services.Interfaces;

namespace StarCraft2League.Services
{
    public class MatchService : IMatchService
    {
        private readonly LeagueContext _leagueContext;

        public MatchService(LeagueContext leagueContext)
        {
            _leagueContext = leagueContext;
        }

        public void CreateMatch(int roundId, int firstPlayerId, int secondPlayerId)
        {
            _leagueContext.Matches.Add(
                new Match
                {
                    RoundId = roundId,
                    FirstPlayerId = firstPlayerId,
                    SecondPlayerId = secondPlayerId
                });
            _leagueContext.SaveChanges();
        }

        public byte GetPlayerLosses(Match match, int playerId)
        {
            if (match.FirstPlayerId == playerId)
                return match.SecondPlayerWins;
            if (match.SecondPlayerId == playerId)
                return match.FirstPlayerWins;
            return 0;
        }

        public byte GetPlayerWins(Match match, int playerId)
        {
            if (match.FirstPlayerId == playerId)
                return match.FirstPlayerWins;
            if (match.SecondPlayerId == playerId)
                return match.SecondPlayerWins;
            return 0;
        }

        public bool TryGetLoser(Match match, out int loserId)
        {
            loserId = 0;
            if (match.FirstPlayerWins == match.SecondPlayerWins)
                return false;
            if (match.FirstPlayerWins < match.SecondPlayerWins)
                loserId = match.FirstPlayerId;
            else
                loserId = match.SecondPlayerId;
            return true;
        }

        public bool TryGetWinner(Match match, out int winnerId)
        {
            winnerId = 0;
            if (match.FirstPlayerWins == match.SecondPlayerWins)
                return false;
            if (match.FirstPlayerWins > match.SecondPlayerWins)
                winnerId = match.FirstPlayerId;
            else
                winnerId = match.SecondPlayerId;
            return true;
        }
    }
}