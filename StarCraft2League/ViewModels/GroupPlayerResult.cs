using StarCraft2League.Models.Users;
using System;

namespace StarCraft2League.ViewModels
{
    public class GroupPlayerResult : IEquatable<GroupPlayerResult>
    {
        public User Player { get; set; }
        public byte WinMatchesCount { get; set; }
        public byte LoseMatchesCount { get; set; }
        public byte WinGamesCount { get; set; }
        public byte LoseGameCount { get; set; }

        public bool Equals(GroupPlayerResult other) =>
            (WinMatchesCount - LoseMatchesCount).Equals(other.WinMatchesCount - other.LoseMatchesCount) &&
                (WinGamesCount - LoseGameCount).Equals(other.WinGamesCount - other.LoseGameCount);
    }
}