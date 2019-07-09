using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StarCraft2League.Models.Users
{
    /// <summary>
    /// A user's account details.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the user's BattleTag.
        /// </summary>
        public string BattleTag { get; set; }

        [NotMapped]
        [Display(Name = "Profile name with clan tag")]
        public string DisplayedName => '[' + Profile.ClanTag + ']' + Profile.Name;

        /// <summary>
        /// Gets or sets the user's account ID.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? ProfileId { get; set; }
        public Profile Profile { get; set; }

        public byte RoleId { get; set; }
        public Role Role { get; set; }

        public IList<UserSeason> UserSeasons { get; set; } = new List<UserSeason>();
        public IList<UserGroup> UserGroups { get; set; } = new List<UserGroup>();
    }
}