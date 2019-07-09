namespace StarCraft2League.Models.Seasons.Rounds
{
    public class PlayoffsRound : Round
    {
        public int SeasonId { get; set; }
        public Season Season { get; set; }
    }
}