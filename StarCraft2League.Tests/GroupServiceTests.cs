using Microsoft.EntityFrameworkCore;
using Moq;
using StarCraft2League.Models;
using StarCraft2League.Services;
using StarCraft2League.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Tests.Common;
using Xunit;

namespace StarCraft2League.Tests
{
    public class GroupServiceTests
    {
        private readonly IGroupsService _groupService;

        public GroupServiceTests()
        {
            var optionsBuilder = new DbContextOptionsBuilder<LeagueContext>();
            optionsBuilder.UseInMemoryDatabase("StarCraft2League");
            LeagueContext dbContext = new LeagueContext(optionsBuilder.Options);            
            Mock<IGroupRoundService> groupRoundServiceMock = new Mock<IGroupRoundService>();
            Mock<IMatchService> matchServiceMock = new Mock<IMatchService>();
            _groupService = new GroupsService(dbContext, groupRoundServiceMock.Object, matchServiceMock.Object);
        }

        public static IEnumerable<object[]> GetPlayerCountRangeWithExpectedResult(
            int expectedResult,
            int lowBound,
            int highBound
            ) =>
            Enumerable.Range(lowBound, ++highBound - lowBound)
            .Select(pc => new object[] { expectedResult, pc });

        [Theory]
        [MemberData(
            nameof(TestDataGenerator.GetPlayerCountRange),
            -1,
            7,
            MemberType = typeof(TestDataGenerator)
            )]
        public void Should_ThrowInvalidOperationException_When(int playerCount)
        {
            Assert.Throws<InvalidOperationException>(() => _groupService.GetCount(playerCount));
        }

        [Theory]
        [MemberData(nameof(GetPlayerCountRangeWithExpectedResult), 2, 8, 20)]
        [MemberData(nameof(GetPlayerCountRangeWithExpectedResult), 4, 21, 40)]
        [MemberData(nameof(GetPlayerCountRangeWithExpectedResult), 8, 41, 80)]
        [MemberData(nameof(GetPlayerCountRangeWithExpectedResult), 16, 81, 160)]
        [MemberData(nameof(GetPlayerCountRangeWithExpectedResult), 32, 161, 161)]
        public void TestGetCount(int expectedResult, int playerCount)
        {
            Assert.Equal(expectedResult, _groupService.GetCount(playerCount));
        }
    }
}