using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StarCraft2League.Constants;
using StarCraft2League.Constants.Users;
using StarCraft2League.Models;
using StarCraft2League.Models.Users;
using StarCraft2League.Services;
using StarCraft2League.Services.Interfaces;

namespace StarCraft2League
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSingleton<IAlarmClock, AlarmClock>();
            services.AddSingleton<ILeagueService, LeagueService>();
            services.AddTransient<IDateTimeService, DateTimeService>();
            services.AddTransient<ISeasonService, SeasonService>();
            services.AddTransient<IPlayoffsService, PlayoffsService>();
            services.AddTransient<IGroupRoundService, GroupRoundService>();
            services.AddTransient<IPlayoffsRoundService, PlayoffsRoundService>();
            services.AddTransient<IGroupsService, GroupsService>();
            services.AddTransient<IMatchService, MatchService>();
            services.AddTransient<IMatchesStatisticsService, MatchesStatisticsService>();

            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<LeagueContext>(options => options.UseSqlServer(connection));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "BattleNet";
            })
            .AddCookie()
            .AddOAuth("BattleNet", options => SetOAuthOptions(options));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.Use(async (context, next) =>
                {
                    context.Request.Scheme = "https";
                    context.Request.Host = new HostString("af3350d3.ngrok.io");
                    await next.Invoke();
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void SetOAuthOptions(OAuthOptions options)
        {
            options.ClientId = Configuration[SecretManagerConstants.BattleNetClientID];
            options.ClientSecret = Configuration[SecretManagerConstants.BattleNetClientSecret];
            options.CallbackPath = new PathString("/signin-battlenet");

            options.AuthorizationEndpoint = "https://eu.battle.net/oauth/authorize";
            options.TokenEndpoint = "https://eu.battle.net/oauth/token";
            options.UserInformationEndpoint = "https://eu.battle.net/oauth/userinfo";

            options.Scope.Add("sc2.profile");
            options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            options.ClaimActions.MapJsonKey(ClaimTypes.Name, "battletag");
            options.SaveTokens = true;

            options.Events = new OAuthEvents
            {
                OnCreatingTicket = async context => { await CreateOAuthTicket(context); },

                OnRemoteFailure = context =>
                {
                    context.HandleResponse();
                    context.Response.Redirect("/Home/Error?message=" + context.Failure.Message);
                    return Task.FromResult(0);
                }
            };
        }

        private async Task CreateOAuthTicket(OAuthCreatingTicketContext context)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

            var response = await context.Backchannel.SendAsync(request, context.HttpContext.RequestAborted);
            response.EnsureSuccessStatusCode();

            context.HttpContext.Response.Cookies.Append("token", context.AccessToken);

            var user = JObject.Parse(await response.Content.ReadAsStringAsync());
            context.RunClaimActions(user);

            LeagueContext leagueContext = context.HttpContext.RequestServices.GetService<LeagueContext>();
            int id = int.Parse((string)user["id"]);
            User dbUser = leagueContext.Users.Find(id);
            Role role = null;
            if (dbUser == null)
            {
                role = leagueContext.Roles.First(r => r.Name == RoleConstants.User);
                dbUser = new User()
                {
                    Id = id,
                    BattleTag = context.Identity.Name,
                    Role = role
                };
                leagueContext.Users.Add(dbUser);
                leagueContext.SaveChanges();
            }
            if (dbUser.ProfileId == null)
            {
                Profile profile = await GetFullProfileAsync(dbUser, context, leagueContext);
                if (profile != null)
                {
                    leagueContext.Profiles.Add(profile);
                    dbUser.ProfileId = profile.Id;
                    leagueContext.Attach(dbUser);
                    leagueContext.Entry(dbUser).Property(u => u.ProfileId).IsModified = true;
                    leagueContext.SaveChanges();
                    SetHasProfileClaimAsTrue(context);
                }
            }
            else
                SetHasProfileClaimAsTrue(context);
            role = role ?? leagueContext.Roles.Find(dbUser.RoleId);
            context.Identity.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, role.Name));
        }

        private void SetHasProfileClaimAsTrue(OAuthCreatingTicketContext context) =>
            context.Identity.AddClaim(new Claim("HasProfile", "True"));

        private async Task<Profile> GetProfileAsync(User dbUser, OAuthCreatingTicketContext context)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                "https://eu.api.blizzard.com/sc2/player/" + dbUser.Id + "?access_token=" + context.AccessToken
                );

            string response = await GetResponseAsync(request, context);

            Profile[] profiles = JsonConvert.DeserializeObject<Profile[]>(response);

            Profile profileEU = profiles.FirstOrDefault(p => p.RegionId == 2 && p.RealmId == 1);
            Profile profileRU = profiles.FirstOrDefault(p => p.RegionId == 2 && p.RealmId == 2);
            Profile anyProfile = profiles.FirstOrDefault();

            if (profileEU != null)
                return profileEU;
            else if (profileRU != null)
                return profileRU;
            else
                return anyProfile;
        }

        private async Task<(string clanTag, League league, string race)> GetClanTagAndLeagueAndRaceAsync(
            Profile profile,
            OAuthCreatingTicketContext context,
            LeagueContext leagueContext
            )
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                "https://eu.api.blizzard.com/sc2/profile/" +
                profile.RegionId + "/" +
                profile.RealmId + "/" +
                profile.Id +
                "?access_token=" + context.AccessToken
                );

            var response = await GetResponseAsync(request, context);

            var pr = JObject.Parse(response);
            string clanTag = (string)pr["summary"]["clanTag"];

            League league = leagueContext.Leagues.FirstOrDefault(l => l.Name == (string)pr["career"]["current1v1LeagueName"]);
            if (league == null)
            {
                league = leagueContext.Leagues.First(l => l.Name == (string)pr["career"]["best1v1Finish"]["leagueName"]);
            }

            int terranParameter = int.Parse((string)pr["career"]["terranWins"]);
            int zergParameter = int.Parse((string)pr["career"]["zergWins"]);
            int protossParameter = int.Parse((string)pr["career"]["protossWins"]);            

            if (terranParameter == 0 & zergParameter == 0 && protossParameter == 0)
            {
                terranParameter = int.Parse((string)pr["swarmLevels"][RaceConstants.TERRAN]["level"]);
                zergParameter = int.Parse((string)pr["swarmLevels"][RaceConstants.ZERG]["level"]);
                protossParameter = int.Parse((string)pr["swarmLevels"][RaceConstants.PROTOSS]["level"]);
            }

            string race;
            if (terranParameter > zergParameter && terranParameter > protossParameter)
                race = RaceConstants.TERRAN;
            else if (zergParameter > terranParameter && zergParameter > protossParameter)
                race = RaceConstants.ZERG;
            else if (protossParameter > terranParameter && protossParameter > zergParameter)
                race = RaceConstants.PROTOSS;
            else
                race = RaceConstants.RANDOM;

            return (clanTag, league, race);
        }

        private async Task<(string currentRace, string ladderId)> GetCurrentRaceAndLadderIdAsync(
            Profile profile,
            OAuthCreatingTicketContext context
            )
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                "https://eu.api.blizzard.com/sc2/profile/" +
                profile.RegionId + "/" +
                profile.RealmId + "/" +
                profile.Id +
                "/ladder/summary" +
                "?access_token=" + context.AccessToken
                );

            var response = await GetResponseAsync(request, context);

            JObject jObject = JObject.Parse(response);
            JArray ladders = (JArray)jObject["showCaseEntries"];
            IEnumerable<JToken> laddersWithLeagues = ladders.Where(l => (string)l["leagueName"] == profile.League.Name);
            JToken _1x1Ladder =
                laddersWithLeagues.FirstOrDefault(ll => (string)ll["team"]["localizedGameMode"] == "1v1");
            string race = null;
            string ladderId = null;
            if (_1x1Ladder != null)
            {
                race = (string)_1x1Ladder["team"]["members"][0]["favoriteRace"];
                ladderId = (string)_1x1Ladder["ladderId"];
            }
            return (race, ladderId);
        }

        private async Task<string> GetSeasonIdAsync(OAuthCreatingTicketContext context)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                "https://eu.api.blizzard.com/sc2/ladder/season/2" +
                "?access_token=" + context.AccessToken
                );

            var response = await GetResponseAsync(request, context);

            var jObject = JObject.Parse(response);
            return (string)jObject["seasonId"];
        }

        private async Task<byte> GetTierAsync(string seasonId, byte leagueId, string ladderId, OAuthCreatingTicketContext context)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                "https://eu.api.blizzard.com/data/sc2/league/" +
                seasonId + "/201/0/" +
                leagueId +
                "?access_token=" + context.AccessToken
                );

            var response = await GetResponseAsync(request, context);

            var jObject = JObject.Parse(response);
            JArray tiers = (JArray)jObject["tier"];
            byte tier = 0;
            for (byte i = 0; i < 3; i++)
            {
                JArray divisions = (JArray)tiers[i]["division"];
                if (divisions.Any(d => (string)d["ladder_id"] == ladderId))
                {
                    tier = ++i;
                    break;
                }
            }
            return tier;
        }

        private async Task<Profile> GetFullProfileAsync(User dbUser, OAuthCreatingTicketContext context, LeagueContext leagueContext)
        {
            Profile profile = await GetProfileAsync(dbUser, context);
            if (profile == null)
                return profile;

            var (clanTag, league, race) = await GetClanTagAndLeagueAndRaceAsync(profile, context, leagueContext);
            profile.ClanTag = clanTag;
            profile.League = league;
            profile.Race = race;

            var (currentRace, ladderId) = await GetCurrentRaceAndLadderIdAsync(profile, context);
            if (currentRace != null)
                profile.Race = currentRace;

            string seasonId = await GetSeasonIdAsync(context);

            profile.Tier = await GetTierAsync(seasonId, profile.League.Id, ladderId, context);

            return profile;
        }

        private async Task<string> GetResponseAsync(HttpRequestMessage request, OAuthCreatingTicketContext context)
        {
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await context.Backchannel.SendAsync(request, context.HttpContext.RequestAborted);
            response.EnsureSuccessStatusCode();

            context.HttpContext.Response.Cookies.Append("token", context.AccessToken);

            return await response.Content.ReadAsStringAsync();
        }
    }
}