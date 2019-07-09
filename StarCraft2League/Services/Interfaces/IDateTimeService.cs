using System;

namespace StarCraft2League.Services.Interfaces
{
    public interface IDateTimeService
    {
        DateTime GetDateTimeOfStartOfPlayoffsOfCurrentSeason(DateTime startMoment);
        DateTime GetNextRoundStartMoment(DateTime currentRoundStartMoment);
    }
}