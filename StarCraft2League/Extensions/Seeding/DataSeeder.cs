using Bogus;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using StarCraft2League.Models;
using StarCraft2League.Models.Seasons;
using StarCraft2League.Services.Interfaces;
using StarCraft2League.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StarCraft2League.Extensions.Seeding
{
    public static class DataSeeder
    {
        public static IWebHost SeedData(this IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                try
                {
                    var services = scope.ServiceProvider;
                    var context = services.GetRequiredService<LeagueContext>();
                    var seasonService = services.GetRequiredService<ISeasonService>();
                    var dateTimeService = services.GetRequiredService<IDateTimeService>();
                    var leagueService = services.GetRequiredService<ILeagueService>();
                    if (!leagueService.IsStarted)
                    {
                        SeedSeason(context, seasonService, dateTimeService);
                        SeedSeason(context, seasonService, dateTimeService);
                    }
                }
                catch (Exception e)
                {

                }
            }
            return host;
        }

        private static void SeedMatchResult(Randomizer randomizer, out byte winnerResult, out byte loserResult)
        {
            winnerResult = 2;
            loserResult = (byte)randomizer.Number(1);
        }

        private static void SeedCurrentPlayoffsRoundResults(LeagueContext leagueContext)
        {
            IEnumerable<Match> currentPlayoffsRoundMatches = leagueContext.PlayoffsRounds.Last().Matches;
            SeedMatchesResults(leagueContext, currentPlayoffsRoundMatches);
        }

        private static void SeedGroupMatchesResults(LeagueContext leagueContext)
        {
            SeedMatchesResults(leagueContext, leagueContext.GroupRounds.SelectMany(gr => gr.Matches).ToArray());
        }

        private static void SeedMatchesResults(LeagueContext leagueContext, IEnumerable<Match> matches)
        {
            Randomizer randomizer = new Randomizer();
            foreach (Match match in matches)
                SeedMatchResult(leagueContext, randomizer, match);
        }

        private static void SeedMatchResult(LeagueContext leagueContext, Randomizer randomizer, Match match)
        {
            byte firstPlayerWins;
            byte secondPlayerWins;
            if (randomizer.Bool())
                SeedMatchResult(randomizer, out firstPlayerWins, out secondPlayerWins);
            else
                SeedMatchResult(randomizer, out secondPlayerWins, out firstPlayerWins);
            match.FirstPlayerWins = firstPlayerWins;
            match.SecondPlayerWins = secondPlayerWins;
            leagueContext.Entry(match).Property(m => m.FirstPlayerWins).IsModified = true;
            leagueContext.Entry(match).Property(m => m.SecondPlayerWins).IsModified = true;
            leagueContext.SaveChanges();
        }

        private static void SeedRegistration(LeagueContext leagueContext, ISeasonService seasonService)
        {
            IEnumerable<int> userIds = leagueContext.Users.Select(u => u.Id);
            foreach (int userId in userIds)
            {
                seasonService.Register(userId);
            }
        }

        private static void SeedSeason(LeagueContext leagueContext, ISeasonService seasonService, IDateTimeService dateTimeService)
        {
            seasonService.Create(new TimeSpan(SeasonOptions.DefaultDaysBetweenRounds, 0, 0, 0));
            SeedRegistration(leagueContext, seasonService);
            seasonService.CreateGroupStage(DateTime.Now);
            SeedGroupMatchesResults(leagueContext);
            DateTime currentRoundStartMoments = dateTimeService.GetDateTimeOfStartOfPlayoffsOfCurrentSeason(DateTime.Now);
            seasonService.CreateFirstPlayoffsRound(currentRoundStartMoments);
            do
                SeedCurrentPlayoffsRoundResults(leagueContext);
            while (seasonService.TryCreateNextPlayoffsRound(
                currentRoundStartMoments = dateTimeService.GetNextRoundStartMoment(currentRoundStartMoments)
                ));
        }
    }
}