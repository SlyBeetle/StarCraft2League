using StarCraft2League.Models.Seasons.Rounds;
using StarCraft2League.Models.Users;
using System.Collections.Generic;

namespace StarCraft2League.Models.Seasons
{
    public class Group
    {
        public int Id { get; set; }

        public int SeasonId { get; set; }
        public Season Season { get; set; }

        public IList<UserGroup> UserGroups { get; set; }

        public IList<GroupRound> Rounds { get; set; } = new List<GroupRound>();
    }
}