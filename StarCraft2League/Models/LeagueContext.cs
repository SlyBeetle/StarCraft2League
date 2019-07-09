using Microsoft.EntityFrameworkCore;
using StarCraft2League.Extensions.Seeding;
using StarCraft2League.Models.Seasons;
using StarCraft2League.Models.Seasons.Rounds;
using StarCraft2League.Models.Users;

namespace StarCraft2League.Models
{
    public class LeagueContext : DbContext
    {
        public DbSet<Role> Roles { get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Round> Rounds { get; set; }
        public DbSet<GroupRound> GroupRounds { get; set; }
        public DbSet<PlayoffsRound> PlayoffsRounds { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<UserSeason> UserSeasons { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }

        public LeagueContext(DbContextOptions<LeagueContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserSeason>()
                .HasKey(us => new { us.UserId, us.SeasonId });
            modelBuilder.Entity<UserGroup>()
                .HasKey(ug => new { ug.UserId, ug.GroupId });                      

            SetupRounds(modelBuilder);

            SetupMatches(modelBuilder);

            modelBuilder.Seed();
        }

        private void SetupRounds(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Round>()
                .HasDiscriminator<bool>("IsGroupRound")
                .HasValue<GroupRound>(true)
                .HasValue<PlayoffsRound>(false);

            modelBuilder.Entity<PlayoffsRound>()
                .HasOne(pr => pr.Season)
                .WithMany(s => s.PlayoffsRounds)
                .HasForeignKey(pr => pr.SeasonId)
                .OnDelete(DeleteBehavior.Restrict);           
        }

        private void SetupMatches(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Match>()
                .HasOne(m => m.FirstPlayer)
                .WithMany()
                .HasForeignKey(m => m.FirstPlayerId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Match>()
                .HasOne(m => m.SecondPlayer)
                .WithMany()
                .HasForeignKey(m => m.SecondPlayerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}