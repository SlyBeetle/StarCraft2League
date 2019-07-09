using Microsoft.EntityFrameworkCore;
using StarCraft2League.Models;
using StarCraft2League.Models.Seasons;
using StarCraft2League.Models.Users;
using StarCraft2League.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StarCraft2League.Services
{
    public class SeasonService : ISeasonService
    {
        private readonly LeagueContext _leagueContext;
        private readonly IGroupsService _groupsService;
        private readonly IPlayoffsService _playoffsService;

        public SeasonService(LeagueContext leagueContext, IGroupsService groupsSevice, IPlayoffsService playoffsService)
        {
            _leagueContext = leagueContext;
            _groupsService = groupsSevice;
            _playoffsService = playoffsService;
        }

        public bool CanRegister(int userId) => Current.IsRegistrationOpen && !IsRegistered(userId);

        public void Create(TimeSpan intervalBetweenRounds)
        {
            _leagueContext.Seasons.Add(new Season()
            {
                IntervalBetweenRounds = intervalBetweenRounds
            });
            _leagueContext.SaveChanges();
        }

        public void CreateFirstPlayoffsRound(DateTime date) => _playoffsService.CreateFirstRound(Current, date);

        public bool TryCreateNextPlayoffsRound(DateTime date) => _playoffsService.TryCreateNextRound(Current, date);

        public void CreateGroupStage(DateTime startDate)
        {
            Current.IsRegistrationOpen = false;
            _groupsService.CreateStage(Current, startDate);
        }

        public void Register(int userId)
        {
            if (!CanRegister(userId))
                throw new InvalidOperationException(string.Format("User with id {0} can't register.", userId));
            UserSeason userSeason = new UserSeason
            {
                UserId = userId,
                SeasonId = Current.Id
            };
            _leagueContext.UserSeasons.Add(userSeason);
            _leagueContext.SaveChanges();
        }

        public Season Current => _leagueContext.Seasons.Any() ? _leagueContext.Seasons.Last() : null;

        public int GroupRoundsCount => _groupsService.RoundsCount;

        public IEnumerable<User> ParticipantsWithProfiles =>
            _leagueContext.UserSeasons
            .Where(us => us.SeasonId == Current.Id)
            .Select(us => us.User)
            .Include(u => u.Profile)
            .ThenInclude(p => p.League);

        private bool IsRegistered(int userId) => Current.UserSeasons.Any(us => us.UserId == userId);
    }
}