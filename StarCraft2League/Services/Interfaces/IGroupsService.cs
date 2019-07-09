using StarCraft2League.Models.Seasons;
using StarCraft2League.Models.Users;
using StarCraft2League.ViewModels;
using System;
using System.Collections.Generic;

namespace StarCraft2League.Services.Interfaces
{
    public interface IGroupsService
    {
        int GetCount(int playerCount);
        void CreateStage(Season currentSeason, DateTime startDate);
        User[,] GetQualifersByGroups();
        IReadOnlyCollection<GroupPlayerResult> GetSortedGroupResults(Group group);
        int Count { get; }
        int QualifiersCount { get; }
        int RoundsCount { get; }
    }
}