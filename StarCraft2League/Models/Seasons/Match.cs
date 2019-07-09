using StarCraft2League.Models.Seasons.Rounds;
using StarCraft2League.Models.Users;
using System.ComponentModel.DataAnnotations;

namespace StarCraft2League.Models.Seasons
{
    public class Match
    {
        public int MatchId { get; set; }

        public int RoundId { get; set; }
        public Round Round { get; set; }

        public int FirstPlayerId { get; set; }
        public User FirstPlayer { get; set; }
        public int SecondPlayerId { get; set; }
        public User SecondPlayer { get; set; }

        [Range(byte.MinValue, byte.MaxValue)]
        public byte FirstPlayerWins { get; set; }
        [Range(byte.MinValue, byte.MaxValue)]
        public byte SecondPlayerWins { get; set; }
    }
}