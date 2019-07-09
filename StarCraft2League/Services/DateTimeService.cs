using StarCraft2League.Services.Interfaces;
using System;

namespace StarCraft2League.Services
{
    public class DateTimeService : IDateTimeService
    {
        private readonly ISeasonService _seasonService;

        public DateTimeService(ISeasonService seasonService)
        {
            _seasonService = seasonService;
        }

        public DateTime GetDateTimeOfStartOfPlayoffsOfCurrentSeason(DateTime startMoment) =>
            startMoment.Add(_seasonService.Current.IntervalBetweenRounds * (_seasonService.GroupRoundsCount + 1));

        public DateTime GetNextRoundStartMoment(DateTime currentRoundStartMoment) =>
            currentRoundStartMoment.Add(_seasonService.Current.IntervalBetweenRounds);
    }
}