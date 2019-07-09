using Microsoft.Extensions.DependencyInjection;
using StarCraft2League.Models;
using StarCraft2League.Services.Interfaces;
using StarCraft2League.ViewModels;
using System;
using System.Linq;
using System.Timers;

namespace StarCraft2League.Services
{
    public class LeagueService : ILeagueService
    {
        private readonly LeagueContext _leagueContext;
        private readonly IServiceScope _serviceScope;
        private readonly IAlarmClock _alarmClock;
        private readonly IDateTimeService _dateTimeService;
        private readonly ISeasonService _seasonService;

        private DateTime _currentRoundStartMoment;
        private DateTime _startMoment;

        public LeagueService(IServiceScopeFactory scopeFactory, IAlarmClock alarmClock)
        {
            _serviceScope = scopeFactory.CreateScope();
            _alarmClock = alarmClock;
            _dateTimeService = _serviceScope.ServiceProvider.GetRequiredService<IDateTimeService>();
            _seasonService = _serviceScope.ServiceProvider.GetRequiredService<ISeasonService>();
            _leagueContext = _serviceScope.ServiceProvider.GetRequiredService<LeagueContext>();
        }

        public bool IsStarted => _leagueContext.Seasons.Any();

        public void LaunchNewSeason(SeasonOptions seasonOptions)
        {
            _startMoment = seasonOptions.StartDate;
            _startMoment = _startMoment.Add(seasonOptions.StartMomentTime);
            TimeSpan intervalBetweenRounds = new TimeSpan(
                seasonOptions.DaysBetweenRounds,
                seasonOptions.TimeBetweenRounds.Hours,
                seasonOptions.TimeBetweenRounds.Minutes,
                seasonOptions.TimeBetweenRounds.Seconds
                );

            if (_startMoment < DateTime.Now)
                throw new InvalidOperationException("Bad start time or date.");

            _seasonService.Create(intervalBetweenRounds);
            _alarmClock.Rang += StartNewSeason;
            _alarmClock.Set(_startMoment);
        }

        private void StartNewSeason(object source, ElapsedEventArgs e)
        {
            _seasonService.CreateGroupStage(_startMoment);

            _alarmClock.Rang -= StartNewSeason;
            _alarmClock.Rang += StartPlayoffs;
            _currentRoundStartMoment = _dateTimeService.GetDateTimeOfStartOfPlayoffsOfCurrentSeason(_startMoment);
            _alarmClock.Set(_currentRoundStartMoment);
        }

        private void StartPlayoffs(object source, ElapsedEventArgs e)
        {
            _seasonService.CreateFirstPlayoffsRound(_currentRoundStartMoment);

            _alarmClock.Rang -= StartPlayoffs;
            _alarmClock.Rang += StartNextPlayoffsRound;
            _currentRoundStartMoment = _dateTimeService.GetNextRoundStartMoment(_currentRoundStartMoment);
            _alarmClock.Set(_currentRoundStartMoment);
        }

        private void StartNextPlayoffsRound(object source, ElapsedEventArgs e)
        {
            if (_seasonService.TryCreateNextPlayoffsRound(_currentRoundStartMoment))
            {
                _currentRoundStartMoment = _currentRoundStartMoment.Add(_seasonService.Current.IntervalBetweenRounds);
                _alarmClock.Set(_currentRoundStartMoment);
                return;
            }
            _seasonService.Current.IsEnded = true;
        }

        public void Dispose()
        {
            _serviceScope.Dispose();
        }
    }
}