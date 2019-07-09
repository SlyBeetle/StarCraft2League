using RoundRobinGroupLibrary.Constants;
using RoundRobinGroupLibrary.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RoundRobinGroupLibrary
{
    public class RoundRobinGroup<T, U> : IReadOnlyCollection<IRound<T, U>>
        where U : Matchup<T>, new()
        where T : IEquatable<T>
    {
        private IRound<T, U> _groupRound;

        public RoundRobinGroup(IReadOnlyCollection<T> players)
        {
            if (players == null)
                throw new ArgumentNullException("Argument players can not be null.");
            if (players.Count < GroupConstants.MIN_PLAYER_COUNT)
                throw new ArgumentOutOfRangeException(
                    string.Format(
                        "Player count can't be less than {0}.",
                        GroupConstants.MIN_PLAYER_COUNT
                        )
                    );
            if (players.Contains(default(T)))
                throw new ArgumentException(string.Format("Player cannot be equal {0}.", default(T)));

            Count = players.Count.IsOdd() ? players.Count : players.Count - 1;
            _groupRound = new GroupRound<T, U>(players);
        }

        public int Count { get; }

        public IEnumerator<IRound<T, U>> GetEnumerator()
        {
            yield return _groupRound;
            for (int i = 1; i < Count; i++)
            {                
                _groupRound = _groupRound.Next();
                yield return _groupRound;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}