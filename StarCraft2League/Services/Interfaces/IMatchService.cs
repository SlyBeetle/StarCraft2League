using StarCraft2League.Models.Seasons;

namespace StarCraft2League.Services.Interfaces
{
    public interface IMatchService
    {
        void CreateMatch(int roundId, int firstPlayerId, int secondPlayerId);
        byte GetPlayerLosses(Match match, int playerId);
        byte GetPlayerWins(Match match, int playerId);
        bool TryGetLoser(Match match, out int winnerId);
        bool TryGetWinner(Match match, out int winnerId);
    }
}