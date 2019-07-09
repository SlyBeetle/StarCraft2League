using StarCraft2League.Models.Seasons;
using StarCraft2League.Services.Interfaces;
using System;

namespace StarCraft2League.Services
{
    public class PlayoffsService : IPlayoffsService
    {
        private readonly IPlayoffsRoundService _playoffsRoundService;

        public PlayoffsService(

            IPlayoffsRoundService playoffsRoundService
            )
        {
            _playoffsRoundService = playoffsRoundService;
        }

        public void CreateFirstRound(Season currentSeason, DateTime date) =>
            _playoffsRoundService.CreateFirst(currentSeason, date);

        public bool TryCreateNextRound(Season currentSeason, DateTime date) =>
            _playoffsRoundService.TryCreateNext(currentSeason, date);
    }
}