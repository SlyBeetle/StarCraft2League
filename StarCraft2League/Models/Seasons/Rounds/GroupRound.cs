namespace StarCraft2League.Models.Seasons.Rounds
{
    public class GroupRound : Round
    {
        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}