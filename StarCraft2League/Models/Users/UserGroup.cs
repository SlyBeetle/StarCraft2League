using StarCraft2League.Models.Seasons;
using System.ComponentModel.DataAnnotations.Schema;

namespace StarCraft2League.Models.Users
{
    public class UserGroup
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }
        public User User { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}
