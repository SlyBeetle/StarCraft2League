using System;

namespace RoundRobinGroupLibrary
{
    public class Matchup<T> : IEquatable<Matchup<T>> where T : IEquatable<T>
    {
        public Matchup() { }

        public Matchup(T firstPlayer, T secondPlayer)
        {
            FirstPlayer = firstPlayer;
            SecondPlayer = secondPlayer;
        }

        public T FirstPlayer { get; set; }
        public T SecondPlayer { get; set; }

        public bool Equals(Matchup<T> other)
        {
            return FirstPlayer.Equals(other.FirstPlayer) && SecondPlayer.Equals(other.SecondPlayer) ||
                FirstPlayer.Equals(other.SecondPlayer) && SecondPlayer.Equals(other.FirstPlayer);
        }

        public override int GetHashCode()
        {
            return FirstPlayer.GetHashCode() + SecondPlayer.GetHashCode();
        }
    }
}