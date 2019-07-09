using StarCraft2League.Models.Seasons;
using System.ComponentModel.DataAnnotations.Schema;

namespace StarCraft2League.Models.Users
{
    public class UserSeason
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }
        public User User { get; set; }

        public int SeasonId { get; set; }
        public Season Season { get; set; }
    }
}