using Bogus;
using Microsoft.EntityFrameworkCore;
using StarCraft2League.Constants;
using StarCraft2League.Constants.Users;
using StarCraft2League.Models.Users;
using System.Collections.Generic;
using System.Linq;

namespace StarCraft2League.Extensions.Seeding
{
    public static class ModelBuilderExtensions
    {
        private const int MAX_GENERATED_USERS_COUNT = 80;
        private const byte ADMIN_ROLE_ID = 1;
        private const byte USER_ROLE_ID = 3;

        public static void Seed(this ModelBuilder modelBuilder)
        {
            string adminRoleName = RoleConstants.Admin;
            string userRoleName = RoleConstants.User;
            string moderatorRoleName = RoleConstants.Moderator;

            Role adminRole = new Role { Id = ADMIN_ROLE_ID, Name = adminRoleName };
            Role moderatorRole = new Role { Id = 2, Name = moderatorRoleName };
            Role userRole = new Role { Id = 3, Name = userRoleName };

            (IList<User> users, IList<Profile> profiles) = GenerateUsersAndProfiles(MAX_GENERATED_USERS_COUNT);

            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, moderatorRole, userRole });
            modelBuilder.Entity<User>().HasData(users.ToArray());
            modelBuilder.Entity<Profile>().HasData(profiles.ToArray());

            string[] leagues = { "BRONZE", "SILVER", "GOLD", "PLATINUM", "DIAMOND", "MASTER", "GRANDMASTER" };
            League bronzeLeague = new League { Id = 0, Name = "BRONZE" };
            League silverLeague = new League { Id = 1, Name = "SILVER" };
            League goldLeague = new League { Id = 2, Name = "GOLD" };
            League platinumLeague = new League { Id = 3, Name = "PLATINUM" };
            League diamondLeague = new League { Id = 4, Name = "DIAMOND" };
            League masterLeague = new League { Id = 5, Name = "MASTER" };
            League grandmasterLeague = new League { Id = 6, Name = "GRANDMASTER" };

            modelBuilder.Entity<League>().HasData(
                new League[] {
                    bronzeLeague,
                    silverLeague,
                    goldLeague,
                    platinumLeague,
                    diamondLeague,
                    masterLeague,
                    grandmasterLeague
                });
        }

        private static (IList<User> users, IList<Profile> profiles) GenerateUsersAndProfiles(int count)
        {
            (IList<User> users, IList<Profile> profiles) = GenerateFakeUsersAndFakeProfiles(count);

            Profile classikProfile = CreateAdminProfile(
                7522559,
                "classikby",
                2,
                1,
                "https://static.starcraft2.com/starport/eadc1041-1c53-4c27-bf45-303ac5c6e33f/portraits/12-31.jpg",
                3
                );
            User classik = CreateAdmin(452767442, "classikby#2591", classikProfile.Id);
            users.Add(classik);
            profiles.Add(classikProfile);

            /*Profile slyBeetleProfile = CreateAdminProfile(
                2282080,
                "SlyBeetle",
                2,
                2,
                "https://static.starcraft2.com/starport/eadc1041-1c53-4c27-bf45-303ac5c6e33f/portraits/12-28.jpg",
                1
                );
            User slyBeetle = CreateAdmin(468199285, "SlyBeetle#21941", slyBeetleProfile.Id);
            users.Add(slyBeetle);
            profiles.Add(slyBeetleProfile);*/

            return (users, profiles);
        }

        private static (IList<User> users, IList<Profile> profiles) GenerateFakeUsersAndFakeProfiles(int count)
        {
            int maxBogusUsersCount = count - 10;
            Randomizer randomizer = new Randomizer();
            int bogusUsersCount = randomizer.Number(maxBogusUsersCount) + 8;
            int totalUsersCount = bogusUsersCount + 2;
            List<Profile> profiles = new List<Profile>(totalUsersCount);
            List<User> users = new List<User>(totalUsersCount);
            
            string[] races = { RaceConstants.TERRAN, RaceConstants.ZERG, RaceConstants.PROTOSS, RaceConstants.RANDOM };

            var profileFaker = new Faker<Profile>()
                .RuleFor(p => p.Id, f => f.IndexFaker)
                .RuleFor(p => p.AvatarUrl, f => f.Internet.Avatar())
                .RuleFor(p => p.Name, f => f.Name.LastName())
                .RuleFor(p => p.RegionId, (byte)2)
                .RuleFor(p => p.RealmId, (byte)1)
                .RuleFor(p => p.ClanTag, f => f.Hacker.Abbreviation())
                .RuleFor(p => p.LeagueId, f => (byte)randomizer.Number(6))
                .RuleFor(p => p.Race, f => races[randomizer.Number(3)])
                .RuleFor(p => p.Tier, f => (byte)(randomizer.Number(3)));
            var userFaker = new Faker<User>()
                .RuleFor(u => u.Id, f => f.IndexFaker + 1)
                .RuleFor(u => u.BattleTag, f => f.Name.LastName() + "#" + (f.IndexFaker + 1).ToString())
                .RuleFor(u => u.RoleId, USER_ROLE_ID);

            for (int i = 0; i < bogusUsersCount; i++)
            {
                Profile fakeProfile = profileFaker.Generate();
                User fakeUser = userFaker.Generate();
                fakeUser.ProfileId = fakeProfile.Id;
                if (fakeProfile.LeagueId == 6)
                    fakeProfile.Tier = 1;
                profiles.Add(fakeProfile);
                users.Add(fakeUser);
            }

            return (users, profiles);
        }

        private static User CreateAdmin(int id, string battleTag, int profileId) =>
            new User { Id = id, BattleTag = battleTag, RoleId = ADMIN_ROLE_ID, ProfileId = profileId };

        private static Profile CreateAdminProfile(
            int id,
            string name,
            byte regionId,
            byte realmId,
            string avatarUrl,
            byte tier
            ) =>
            new Profile {
                Id = id,
                Name = name,
                RegionId = regionId,
                RealmId = realmId,
                AvatarUrl = avatarUrl,
                ClanTag = "PLA2N",
                LeagueId = 4,
                Tier = tier,
                Race = "protoss"
            };
    }
}