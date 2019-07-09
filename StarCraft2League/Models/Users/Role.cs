using System.Collections.Generic;

namespace StarCraft2League.Models.Users
{
    public class Role
    {
        public byte Id { get; set; }
        public string Name { get; set; }

        public IList<User> Users { get; set; } = new List<User>();
    }
}