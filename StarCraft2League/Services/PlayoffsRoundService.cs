using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using StarCraft2League.Models;
using StarCraft2League.Models.Seasons;
using StarCraft2League.Models.Seasons.Rounds;
using StarCraft2League.Models.Users;
using StarCraft2League.Services.Interfaces;

namespace StarCraft2League.Services
{
    public class PlayoffsRoundService : IPlayoffsRoundService
    {
        private readonly LeagueContext _leagueContext;
        private readonly IGroupsService _groupsService;
        private readonly IMatchService _matchService;

        private int _roundId;

        public PlayoffsRoundService(
            LeagueContext leagueContext,
            IGroupsService groupsService,
            IMatchService matchService
            )
        {
            _leagueContext = leagueContext;
            _groupsService = groupsService;
            _matchService = matchService;
        }

        public void CreateFirst(Season currentSeason, DateTime date)
        {
            CreateRound(currentSeason, date);
            User[,] qualifersByGroups = _groupsService.GetQualifersByGroups();
            if (_groupsService.QualifiersCount == 2)
                SeedFirstWhenTwoQualifiers(qualifersByGroups);
            else
                SeedFirstWhenFourQualifiers(qualifersByGroups);
        }

        public bool TryCreateNext(Season currentSeason, DateTime date)
        {
            if (!_leagueContext.PlayoffsRounds.Any())
                return false;

            PlayoffsRound currentRound = Current;
            int nextRoundPlayersCount = currentRound.Matches.Count;
            if (nextRoundPlayersCount <= 1)
                return false;

            List<int> winnersIds = new List<int>(nextRoundPlayersCount);
            foreach (Match match in currentRound.Matches)
            {
                int winnerId;
                if (!_matchService.TryGetWinner(match, out winnerId))
                    if (new Randomizer().Bool())
                        winnerId = match.FirstPlayerId;
                    else
                        winnerId = match.SecondPlayerId;
                winnersIds.Add(winnerId);
            }

            CreateRound(currentSeason, date);
            for (int i = 0; i < nextRoundPlayersCount; i += 2)
            {
                User firstPlayer = _leagueContext.Users.Find(winnersIds[i]);
                User secondPlayer = _leagueContext.Users.Find(winnersIds[i + 1]);
                CreateMatch(firstPlayer, secondPlayer);
            }
            return true;
        }

        private void CreateMatch(User firstPlayer, User secondPlayer)
        {
            _matchService.CreateMatch(_roundId, firstPlayer.Id, secondPlayer.Id);
        }

        private void CreateRound(Season currentSeason, DateTime date)
        {
            PlayoffsRound firstPlayoffsRound = new PlayoffsRound
            {
                EventDate = date
            };
            currentSeason.PlayoffsRounds.Add(firstPlayoffsRound);
            _leagueContext.SaveChanges();
            _roundId = firstPlayoffsRound.Id;
        }

        private void SeedFirstWhenFourQualifiers(User[,] qualifersByGroups)
        {
            if (_groupsService.Count == 2)
                for (int i = 0; i < _groupsService.QualifiersCount; i++)
                    CreateMatch(qualifersByGroups[0, i], qualifersByGroups[1, _groupsService.QualifiersCount - 1 - i]);
            else
                for (int i = 0; i < _groupsService.Count; i += 4)
                {
                    // A1 vs B4, C2 vs D3, D2 vs C3, B1 vs A4, C1 vs D4, A2 vs B3, B2 vs A3, D1 vs C4 or
                    // A0 vs B3, C1 vs D2, D1 vs C2, B0 vs A3, C0 vs D3, A1 vs B2, B1 vs A2, D0 vs C3.
                    int A = i;
                    int B = i + 1;
                    int C = i + 2;
                    int D = i + 3;
                    // A0 vs B3, C1 vs D2, D1 vs C2, B0 vs A3
                    CreateMatch(qualifersByGroups[A, 0], qualifersByGroups[B, 3]);
                    CreateMatch(qualifersByGroups[C, 1], qualifersByGroups[D, 2]);
                    CreateMatch(qualifersByGroups[D, 1], qualifersByGroups[C, 2]);
                    CreateMatch(qualifersByGroups[B, 0], qualifersByGroups[A, 3]);
                    // C0 vs D3, A1 vs B2, B1 vs A2, D0 vs C3
                    CreateMatch(qualifersByGroups[C, 0], qualifersByGroups[D, 3]);
                    CreateMatch(qualifersByGroups[A, 1], qualifersByGroups[B, 2]);
                    CreateMatch(qualifersByGroups[B, 1], qualifersByGroups[A, 2]);
                    CreateMatch(qualifersByGroups[D, 0], qualifersByGroups[C, 3]);
                }
        }

        private void SeedFirstWhenTwoQualifiers(User[,] qualifersByGroups)
        {
            for (int i = 0; i < _groupsService.Count; i += 2)
            {
                CreateMatch(qualifersByGroups[i, 0], qualifersByGroups[i + 1, 1]);
                CreateMatch(qualifersByGroups[i, 1], qualifersByGroups[i + 1, 0]);
            }
        }

        private PlayoffsRound Current => _leagueContext.PlayoffsRounds.Last();
    }
}
