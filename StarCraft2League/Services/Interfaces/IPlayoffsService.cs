using StarCraft2League.Models.Seasons;
using System;

namespace StarCraft2League.Services.Interfaces
{
    public interface IPlayoffsService
    {
        void CreateFirstRound(Season currentSeason, DateTime startDate);
        bool TryCreateNextRound(Season currentSeason, DateTime date);
    }
}