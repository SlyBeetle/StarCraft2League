using StarCraft2League.Models.Seasons;
using StarCraft2League.Models.Users;
using System;
using System.Collections.Generic;

namespace StarCraft2League.Services.Interfaces
{
    public interface ISeasonService
    {
        bool CanRegister(int userId);
        void Create(TimeSpan intervalBetweenRounds);
        void CreateFirstPlayoffsRound(DateTime date);
        void CreateGroupStage(DateTime startDate);
        void Register(int userId);
        bool TryCreateNextPlayoffsRound(DateTime date);
        Season Current { get; }
        int GroupRoundsCount { get; }
        IEnumerable<User> ParticipantsWithProfiles { get; }
    }
}