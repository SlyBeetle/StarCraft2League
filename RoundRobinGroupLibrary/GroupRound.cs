using RoundRobinGroupLibrary.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RoundRobinGroupLibrary
{
    class GroupRound<T, U> : IRound<T, U>
        where U : Matchup<T>, new()
        where T : IEquatable<T>
    {
        private readonly T BYE = default(T);
        private LinkedList<T> _firstPlayers;
        private LinkedList<T> _secondPlayers;

        public GroupRound(IReadOnlyCollection<T> players)
        {
            Count = players.Count / 2;
            _firstPlayers = new LinkedList<T>(players.Take(Count));
            _secondPlayers = new LinkedList<T>(players.Skip(Count));
            if (players.Count.IsOdd())
                _firstPlayers.AddLast(BYE);
        }

        private GroupRound(GroupRound<T, U> groupRound)
        {
            Count = groupRound.Count;
            _firstPlayers = new LinkedList<T>(groupRound._firstPlayers);
            _secondPlayers = new LinkedList<T>(groupRound._secondPlayers);
        }

        public int Count { get; }

        public IEnumerator<U> GetEnumerator()
        {
            if (Count < 1)
                yield break;

            U match;
            LinkedListNode<T> firstNode = _firstPlayers.First;
            LinkedListNode<T> secondNode = _secondPlayers.First;
            while (firstNode != _firstPlayers.Last)
            {
                if (TryCreateMatch(firstNode, secondNode, out match))
                    yield return match;

                firstNode = firstNode.Next;
                secondNode = secondNode.Next;
            }
            if (TryCreateMatch(firstNode, secondNode, out match))
                yield return match;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IRound<T, U> Next()
        {
            GroupRound<T, U> nextRound = new GroupRound<T, U>(this);
            T one = nextRound._firstPlayers.First.Value;
            nextRound._firstPlayers.RemoveFirst();
            nextRound.TurnClockwise();
            nextRound._firstPlayers.AddFirst(one);
            return nextRound;
        }

        private void TurnClockwise()
        {
            _firstPlayers.AddLast(_secondPlayers.Last.Value);
            _secondPlayers.RemoveLast();

            _secondPlayers.AddFirst(_firstPlayers.First.Value);
            _firstPlayers.RemoveFirst();
        }

        private bool TryCreateMatch(
            LinkedListNode<T> firstNode,
            LinkedListNode<T> secondNode,
            out U match
            )
        {
            match = default(U);
            if (firstNode.Value.Equals(BYE) || secondNode.Value.Equals(BYE))
                return false;
            match = new U
            {
                FirstPlayer = firstNode.Value,
                SecondPlayer = secondNode.Value
            };
            return true;
        }
    }
}