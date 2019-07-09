using RoundRobinGroupLibrary;
using StarCraft2League.Models;
using StarCraft2League.Models.Seasons;
using StarCraft2League.Models.Seasons.Rounds;
using StarCraft2League.Models.Users;
using StarCraft2League.Services.Interfaces;
using StarCraft2League.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StarCraft2League.Services
{
    public class GroupsService : IGroupsService
    {
        private const int MAX_GROUP_PLAYER_COUNT_FOR_TWO_QUALIFIERS = 6;
        private readonly LeagueContext _leagueContext;
        private readonly IGroupRoundService _groupRoundService;
        private readonly IMatchService _matchService;
        private DateTime _groupStageStartDate;
        private Season _currentSeason;

        public GroupsService(LeagueContext leagueContext, IGroupRoundService groupRoundService, IMatchService matchService)
        {
            _leagueContext = leagueContext;
            _groupRoundService = groupRoundService;
            _matchService = matchService;
        }

        public delegate bool TryGetFunc<TKey, TResult>(TKey key, out TResult result);

        public void CreateStage(Season currentSeason, DateTime startDate)
        {
            _leagueContext.Entry(currentSeason).Collection(s => s.UserSeasons).Load();
            _currentSeason = currentSeason;
            CreateGroupsWithParticipants();
            CreateSchedule(startDate);
            _leagueContext.SaveChanges();
        }

        public int GetCount(int playerCount)
        {
            if (playerCount < 8)
                throw new InvalidOperationException("There are too few participants to start the season.");
            return (int)Math.Pow(2, (int)Math.Log((playerCount - 1) / 10.0, 2) + 1);
        }

        public User[,] GetQualifersByGroups()
        {
            User[,] qualifersByGroups = new User[Count, QualifiersCount];
            int i = 0;
            IEnumerable<Group> currentSeasonGroups = _leagueContext.Groups.Where(g => g.SeasonId == CurrentSeason.Id);
            foreach (Group group in currentSeasonGroups)
            {
                IEnumerable<GroupPlayerResult> qualifiersResults = GetSortedGroupResults(group).Take(QualifiersCount);
                int j = 0;
                foreach (GroupPlayerResult qualifierResult in qualifiersResults)
                {
                    qualifersByGroups[i, j] = qualifierResult.Player;
                    j++;
                }
                i++;
            }
            return qualifersByGroups;
        }

        public IReadOnlyCollection<GroupPlayerResult> GetSortedGroupResults(Group group)
        {
            Load(group);
            User[] groupParticipants = group.UserGroups.Select(ug => ug.User).ToArray();
            return GetSortedPlayersResults(groupParticipants, group.Rounds.SelectMany(r => r.Matches).ToArray());
        }

        public int Count => GetCount(ParticipantsCount);

        public int QualifiersCount => ParticipantsCount <= Count * MAX_GROUP_PLAYER_COUNT_FOR_TWO_QUALIFIERS ? 2 : 4;

        private GroupPlayerResult[] GetSortedPlayersResults(IList<User> players, ICollection<Match> relatedMatches)
        {
            GroupPlayerResult[] sortedPlayersResults = GetPlayersResults(players, relatedMatches)
                .OrderByDescending(pr => pr.WinMatchesCount - pr.LoseMatchesCount)
                .ThenByDescending(pr => pr.WinGamesCount - pr.LoseGameCount).ToArray();
            for (int i = 0; i < sortedPlayersResults.Length; i++)
            {
                int j = i;
                while (j + 1 < sortedPlayersResults.Length && sortedPlayersResults[i].Equals(sortedPlayersResults[j + 1]))
                    j++;
                if (j - i + 1 == sortedPlayersResults.Length)
                    return GetRandomSortedPlayersResults(sortedPlayersResults);
                if (j > i)
                {
                    GroupPlayerResult[] sortedSubArray = GetSortedSubArray(sortedPlayersResults, i, j, relatedMatches);
                    Array.Copy(sortedSubArray, 0, sortedPlayersResults, i, sortedSubArray.Length);
                }
                i = j;
            }
            return sortedPlayersResults;
        }

        private GroupPlayerResult[] GetSortedSubArray(
            GroupPlayerResult[] results,
            int startIndex,
            int endIndex,
            ICollection<Match> matches
            )
        {
            GroupPlayerResult[] controversialPlayerResults = new GroupPlayerResult[endIndex - startIndex + 1];
            Array.Copy(
                results,
                startIndex,
                controversialPlayerResults,
                0,
                controversialPlayerResults.Length
                );
            User[] playersWithControversialResults = controversialPlayerResults.Select(r => r.Player).ToArray();
            IEnumerable<int> playersWithControversialResultsIds = playersWithControversialResults.Select(p => p.Id);
            Match[] relatedMatches = matches.Where(
                m => playersWithControversialResultsIds.Contains(m.FirstPlayerId) &&
                playersWithControversialResultsIds.Contains(m.SecondPlayerId)
                ).ToArray();
            GroupPlayerResult[] sortedSubArray = GetSortedPlayersResults(playersWithControversialResults, relatedMatches);
            for (int i = 0; i < sortedSubArray.Length; i++)
            {
                GroupPlayerResult trueResult = controversialPlayerResults.Single(r => r.Player.Id == sortedSubArray[i].Player.Id);
                sortedSubArray[i].LoseGameCount = trueResult.LoseGameCount;
                sortedSubArray[i].LoseMatchesCount = trueResult.LoseMatchesCount;
                sortedSubArray[i].WinGamesCount = trueResult.WinGamesCount;
                sortedSubArray[i].WinMatchesCount = trueResult.WinMatchesCount;
            }
            return sortedSubArray;
        }

        private GroupPlayerResult[] GetRandomSortedPlayersResults(GroupPlayerResult[] playersResults)
        {
            Random random = new Random();
            List<GroupPlayerResult> results = new List<GroupPlayerResult>(playersResults);
            for (int i = 0; i < playersResults.Length; i++)
            {
                int j = random.Next(results.Count);
                playersResults[i] = results[j];
                results.RemoveAt(j);
            }
            return playersResults;
        }

        private GroupPlayerResult[] GetPlayersResults(IList<User> players, ICollection<Match> matches)
        {
            GroupPlayerResult[] playerResults = new GroupPlayerResult[players.Count];
            for (int i = 0; i < players.Count; i++)
                playerResults[i] = new GroupPlayerResult
                {
                    Player = players[i],
                    WinMatchesCount = GetWinMatchesCount(matches, players[i].Id),
                    LoseMatchesCount = GetLoseMatchesCount(matches, players[i].Id),
                    WinGamesCount = GetWinGamesCount(matches, players[i].Id),
                    LoseGameCount = GetLoseGamesCount(matches, players[i].Id)
                };
            return playerResults;
        }

        private void Load(Group group)
        {
            _leagueContext.Entry(group).Collection(g => g.UserGroups).Load();
            _leagueContext.Entry(group).Collection(g => g.Rounds).Load();
            foreach (UserGroup userGroup in group.UserGroups)
                _leagueContext.Entry(userGroup).Reference(ug => ug.User).Load();
            foreach (GroupRound groupRound in group.Rounds)
                _leagueContext.Entry(groupRound).Collection(gr => gr.Matches).Load();
        }

        private byte GetLoseGamesCount(ICollection<Match> groupMatches, int participantId) =>
            GetGamesResultCount(_matchService.GetPlayerLosses, groupMatches, participantId);

        private byte GetGamesResultCount(
            Func<Match, int, byte> getPlayerResult,
            ICollection<Match> groupMatches,
            int participantId
            )
        {
            byte gamesResultCount = 0;
            foreach (Match match in groupMatches)
                gamesResultCount += getPlayerResult(match, participantId);
            return gamesResultCount;
        }

        private byte GetWinGamesCount(ICollection<Match> groupMatches, int participantId) =>
            GetGamesResultCount(_matchService.GetPlayerWins, groupMatches, participantId);

        private byte GetLoseMatchesCount(ICollection<Match> groupMatches, int groupParticipantId) =>
            GetMatchesResultCount(_matchService.TryGetLoser, groupMatches, groupParticipantId);

        private byte GetMatchesResultCount(
            TryGetFunc<Match, int> tryGetResult,
            ICollection<Match> groupMatches,
            int groupParticipantId
            )
        {
            byte resultMatchesCount = 0;
            foreach (Match match in groupMatches)
                if (tryGetResult(match, out int id))
                    if (id == groupParticipantId)
                        resultMatchesCount++;
            return resultMatchesCount;
        }

        private byte GetWinMatchesCount(ICollection<Match> groupMatches, int groupParticipantId) =>
            GetMatchesResultCount(_matchService.TryGetWinner, groupMatches, groupParticipantId);

        private void CreateGroups()
        {
            int groupCount = Count;
            for (int i = 0; i < groupCount; i++)
            {
                Group group = new Group();
                _currentSeason.Groups.Add(group);
            }
            _leagueContext.SaveChanges();
        }

        private void CreateGroupsWithParticipants()
        {
            CreateGroups();
            SeedParticipants();
        }

        private void CreateSchedule(DateTime startDate)
        {
            _groupStageStartDate = startDate;
            int groupsCount = Count;
            IList<Group> groups = _currentSeason.Groups;
            for (int i = 0; i < groupsCount; i++)
            {
                CreateSchedule(groups[i].Id, groups[i].UserGroups.Select(ug => ug.UserId).ToArray());
            }
        }

        private void CreateSchedule(int groupId, IReadOnlyCollection<int> userIds)
        {
            DateTime date = _groupStageStartDate.Add(_currentSeason.IntervalBetweenRounds);
            RoundRobinGroup<int, Matchup<int>> roundRobinGroup =
                new RoundRobinGroup<int, Matchup<int>>(userIds);
            foreach (IRound<int, Matchup<int>> groupRound in roundRobinGroup)
            {
                _groupRoundService.Create(groupId, groupRound, date);
                date = date.Add(_currentSeason.IntervalBetweenRounds);
            }
            _roundsCount = roundRobinGroup.Count;
        }

        private void SeedParticipants()
        {
            List<int> participantIds = _currentSeason.UserSeasons.Select(us => us.UserId).ToList();
            Random random = new Random();
            while (participantIds.Any())
                for (int i = 0; i < Count; i++)
                {
                    int index = random.Next(participantIds.Count);
                    UserGroup userGroup = new UserGroup
                    {
                        GroupId = _currentSeason.Groups[i].Id,
                        UserId = participantIds[index]
                    };
                    _leagueContext.UserGroups.Add(userGroup);
                    participantIds.RemoveAt(index);
                    if (!participantIds.Any())
                        break;
                }
            _leagueContext.SaveChanges();
        }

        private Season CurrentSeason => _leagueContext.Seasons.Last();

        private int ParticipantsCount => CurrentSeason.ParticipantsCount;

        private static int _roundsCount;
        public int RoundsCount => _roundsCount;
    }
}