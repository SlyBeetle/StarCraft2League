using Xunit;

namespace RoundRobinGroupLibrary.Tests
{
    public class MatchupTests
    {
        [Theory]
        [InlineData(true, 1, 2, 1, 2)]
        [InlineData(true, 1, 2, 2, 1)]
        [InlineData(false, 1, 2, 1, 3)]
        [InlineData(false, 1, 2, 3, 1)]
        public void TestEquality(
            bool expectedResult,
            int player1OfMatch1,
            int player2OfMatch1,
            int player1OfMatch2,
            int player2OfMatch2
            )
        {
            Matchup<int> firstMatchup = new Matchup<int>(player1OfMatch1, player2OfMatch1);
            Matchup<int> secondMatchup = new Matchup<int>(player1OfMatch2, player2OfMatch2);

            Assert.Equal(expectedResult, firstMatchup.Equals(secondMatchup));
            Assert.Equal(expectedResult, secondMatchup.Equals(firstMatchup));
        }

        [Theory]
        [InlineData(1, 2, 1, 2)]
        [InlineData(1, 2, 2, 1)]
        public void TestHashCode(
            int player1OfMatch1,
            int player2OfMatch1,
            int player1OfMatch2,
            int player2OfMatch2
            )
        {
            Matchup<int> firstMatchup = new Matchup<int>(player1OfMatch1, player2OfMatch1);
            Matchup<int> secondMatchup = new Matchup<int>(player1OfMatch2, player2OfMatch2);

            Assert.Equal(firstMatchup.GetHashCode(), secondMatchup.GetHashCode());
        }
    }
}