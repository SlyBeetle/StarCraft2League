using RoundRobinGroupLibrary;
using System;

namespace StarCraft2League.Services.Interfaces
{
    public interface IGroupRoundService
    {
        void Create(int groupId, IRound<int, Matchup<int>> groupRound, DateTime date);
    }
}