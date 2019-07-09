using System;
using RoundRobinGroupLibrary;
using StarCraft2League.Models;
using StarCraft2League.Models.Seasons.Rounds;
using StarCraft2League.Services.Interfaces;

namespace StarCraft2League.Services
{
    public class GroupRoundService : IGroupRoundService
    {
        private readonly LeagueContext _leagueContext;
        private readonly IMatchService _matchService;

        public GroupRoundService(LeagueContext leagueContext, IMatchService matchService)
        {
            _leagueContext = leagueContext;
            _matchService = matchService;
        }

        public void Create(int groupId, IRound<int, Matchup<int>> groupRound, DateTime date)
        {
            GroupRound dbGroupRound = new GroupRound
            {
                GroupId = groupId,
                EventDate = date
            };
            _leagueContext.GroupRounds.Add(dbGroupRound);
            _leagueContext.SaveChanges();

            foreach (Matchup<int> matchup in groupRound)
                _matchService.CreateMatch(dbGroupRound.Id, matchup.FirstPlayer, matchup.SecondPlayer);
        }
    }
}