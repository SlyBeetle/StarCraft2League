using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarCraft2League.Constants.Users;
using StarCraft2League.Models;
using StarCraft2League.Models.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StarCraft2League.Controllers
{
    public partial class AccountsController : Controller
    {
        private readonly LeagueContext _leagueContext;

        public AccountsController(LeagueContext leagueContext)
        {
            _leagueContext = leagueContext;
        }

        public virtual IActionResult Login(string returnUrl = "/")
        {
            return Challenge(new AuthenticationProperties() { RedirectUri = returnUrl });
        }

        [Authorize]
        public virtual IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(MVC.Home.Index());
        }

        [Authorize(Roles = RoleConstants.Admin)]
        public async virtual Task<IActionResult> Users()
        {
            IEnumerable<Role> roles = await GetRolesAsync();
            return View(roles);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleConstants.Admin)]
        public async virtual Task<IActionResult> Users([FromForm]User[] users)
        {
            foreach (User user in users)
            {
                User dbUser = _leagueContext.Users.Find(user.Id);
                dbUser.RoleId = user.RoleId;
            }
            _leagueContext.SaveChanges();
            return View(await GetRolesAsync());
        }

        private async Task<IList<Role>> GetRolesAsync() =>
            await _leagueContext.Roles.Include(r => r.Users).ToListAsync();
    }
}