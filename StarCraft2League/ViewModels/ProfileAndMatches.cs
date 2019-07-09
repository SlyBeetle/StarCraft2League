using StarCraft2League.Models.Seasons;
using StarCraft2League.Models.Users;
using System.Collections.Generic;

namespace StarCraft2League.ViewModels
{
    public class ProfileAndMatches
    {
        public Profile Profile { get; set; }
        public IList<Match> Matches { get; set; } = new List<Match>();
        public int Wins { get; set; }
        public int Losses { get; set; }
        public double Winrate { get; set; }
        public IList<Match> CurrentSeasonMatches { get; set; } = new List<Match>();
        public int CurrentSeasonWins { get; set; }
        public int CurrentSeasonLosses { get; set; }
        public double CurrentSeasonWinrate { get; set; }
    }
}