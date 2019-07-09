using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace StarCraft2League.Models.Users
{
    [DataContract]
    public class Profile
    {
        [DataMember(Name = "profileId")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        [Display(Name="Profile name")]
        public string Name { get; set; }

        [NotMapped]
        public string Url => "https://www.starcraft2.com/profile/" + RegionId + "/" + RealmId + "/" + Id;

        [DataMember(Name = "avatarUrl")]
        public string AvatarUrl { get; set; }

        [DataMember(Name = "regionId")]
        public byte RegionId { get; set; }

        [DataMember(Name = "realmId")]
        public byte RealmId { get; set; }

        public User User { get; set; }
        public string ClanTag { get; set; }
        public string Race { get; set; }

        public byte LeagueId { get; set; }
        public League League { get; set; }

        public byte Tier { get; set; }
    }
}