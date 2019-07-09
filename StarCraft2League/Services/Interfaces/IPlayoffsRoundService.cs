using StarCraft2League.Models.Seasons;
using System;

namespace StarCraft2League.Services.Interfaces
{
    public interface IPlayoffsRoundService
    {
        void CreateFirst(Season currentSeason, DateTime date);
        bool TryCreateNext(Season currentSeason, DateTime date);
    }
}
