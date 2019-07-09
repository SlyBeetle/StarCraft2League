using StarCraft2League.Models.Seasons.Rounds;
using StarCraft2League.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StarCraft2League.Models.Seasons
{
    public class Season
    {
        public int Id { get; set; }
        public long IntervalBetweenRoundsInTicks { get; set; }
        public bool IsRegistrationOpen { get; set; } = true;
        public bool IsEnded { get; set; } = false;
        public IList<Group> Groups { get; set; } = new List<Group>();
        public IList<PlayoffsRound> PlayoffsRounds { get; set; } = new List<PlayoffsRound>();

        public IList<UserSeason> UserSeasons { get; set; } = new List<UserSeason>();

        public int ParticipantsCount => UserSeasons.Count;

        [NotMapped]
        public TimeSpan IntervalBetweenRounds
        {
            get
            {
                return TimeSpan.FromTicks(IntervalBetweenRoundsInTicks);
            }
            set
            {
                IntervalBetweenRoundsInTicks = value.Ticks;
            }
        }
    }
}