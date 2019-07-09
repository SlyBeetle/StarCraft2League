using System.Collections.Generic;

namespace StarCraft2League.Models.Users
{
    public class League
    {
        public byte Id { get; set; }
        public string Name { get; set; }

        public IList<Profile> Profiles { get; set; } = new List<Profile>();
    }
}