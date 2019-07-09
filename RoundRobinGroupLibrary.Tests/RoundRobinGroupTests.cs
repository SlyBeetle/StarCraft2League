using RoundRobinGroupLibrary.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Tests.Common;
using Xunit;

namespace RoundRobinGroupLibrary.Tests
{
    public class RoundRobinGroupTests
    {
        [Fact]
        public void Should_ThrowArgumentNullException_When_PlayersCollectionIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new RoundRobinGroup<int, Matchup<int>>(null));
        }

        [Fact]
        public void Should_ThrowArgumentOutOfRangeException_When_PlayersCountLessThenMIN_PLAYER_COUNT()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new RoundRobinGroup<int, Matchup<int>>(new int[] { 1 }));
        }

        [Fact]
        public void Should_ThrowArgumentException_When_PlayersCollectionHaveDefaultValue()
        {
            Assert.Throws<ArgumentException>(
                () => new RoundRobinGroup<int, Matchup<int>>(new int[] { 0, 1 }));
        }

        [Theory]
        [MemberData(
            nameof(TestDataGenerator.GetPlayerCountRange),
            2,
            10,
            MemberType = typeof(TestDataGenerator)
            )]
        public void Should_CreateRightRoundCount_When(int playerCount)
        {
            int expectedCount = playerCount;
            if (playerCount.IsEven())
                expectedCount--;

            RoundRobinGroup<int, Matchup<int>> roundRobinGroup =
                CreateRoundRobinGroup(playerCount);
            int actualCountPropertyValue = roundRobinGroup.Count;
            int actualCount = roundRobinGroup.Count();

            Assert.Equal(expectedCount, actualCountPropertyValue);
            Assert.Equal(expectedCount, actualCount);
        }

        [Theory]
        [MemberData(
            nameof(TestDataGenerator.GetPlayerCountRange),
            2,
            10,
            MemberType = typeof(TestDataGenerator)
            )]
        public void Should_CreateRightMatchups_When(int playerCount)
        {
            HashSet<Matchup<int>> expectedMatchups = CreateExpectedMatchups(playerCount);
            IEnumerable<Matchup<int>> actualMatchups = CreateActualMatchups(playerCount);
            Assert.True(expectedMatchups.SetEquals(actualMatchups));
        }

        private HashSet<Matchup<int>> CreateExpectedMatchups(int playerCount)
        {
            HashSet<Matchup<int>> matchups =
                new HashSet<Matchup<int>>(GetExpectedMatchupCount(playerCount));
            for (int i = 1; i <= playerCount; i++)
                for (int j = i + 1; j <= playerCount; j++)
                    matchups.Add(new Matchup<int>(i, j));
            return matchups;
        }

        private int GetExpectedMatchupCount(int playerCount) => (playerCount * playerCount - 1) / 2;

        private IEnumerable<Matchup<int>> CreateActualMatchups(int playerCount) =>
            CreateRoundRobinGroup(playerCount).SelectMany(r => r);

        private RoundRobinGroup<int, Matchup<int>> CreateRoundRobinGroup(int playerCount) =>
            new RoundRobinGroup<int, Matchup<int>>(
                    Enumerable.Range(1, playerCount).ToArray()
                    );
    }
}