using System.Collections.Generic;
using System.Linq;

namespace Tests.Common
{
    public static class TestDataGenerator
    {
        public static IEnumerable<object[]> GetPlayerCountRange(int lowBound, int highBound) =>
            Enumerable.Range(lowBound, ++highBound - lowBound).Select(pc => new object[] { pc });
    }
}