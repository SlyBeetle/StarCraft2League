using System;
using System.Collections.Generic;

namespace StarCraft2League.Models.Seasons.Rounds
{
    public abstract class Round
    {
        public int Id { get; set; }
        public bool IsGroupRound { get; set; }
        public DateTime EventDate { get; set; }

        public IList<Match> Matches { get; set; } = new List<Match>();

        public string Name
        {
            get
            {
                if (Matches.Count == 1)
                    return "Final";
                else if (Matches.Count == 2)
                    return "Semifinals";
                return "Ro" + (Matches.Count * 2);
            }
        }
    }
}