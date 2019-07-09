using StarCraft2League.ViewModels;
using System;

namespace StarCraft2League.Services.Interfaces
{
    public interface ILeagueService : IDisposable
    {
        bool IsStarted { get; }
        void LaunchNewSeason(SeasonOptions seasonOptions);
    }
}