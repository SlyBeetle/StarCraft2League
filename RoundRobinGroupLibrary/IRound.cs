using System;
using System.Collections.Generic;

namespace RoundRobinGroupLibrary
{
    public interface IRound<T, U> : IReadOnlyCollection<U>
        where U : Matchup<T>, new()
        where T : IEquatable<T>
    {
        IRound<T, U> Next();
    }
}