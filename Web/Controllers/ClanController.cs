using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Web.Database;

namespace Web.Controllers
{
    public class ClanController : Controller
    {
        private readonly HellContext db;
        public ClanController(IServiceProvider service)
        {
            db = service.GetRequiredService<HellContext>();
        }
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "Date_desc" : "Date";
            ViewData["DateLastPlayedSortParm"] = sortOrder == "DateLastPlayed" ? "DateLastPlayed_desc" : "DateLastPlayed";
            ViewData["CurrentFilter"] = searchString;

            var destiny2Clan = db.Clan_Members.Include(c => c.Clan);

            if (!string.IsNullOrEmpty(searchString))
                destiny2Clan = destiny2Clan.Where(m => m.Name.Contains(searchString)).Include(c => c.Clan);

            if (destiny2Clan == null)
                return NotFound();

            switch (sortOrder)
            {
                case "Name_desc":
                    destiny2Clan.OrderByDescending(d => d.Name).Include(c => c.Clan);
                    break;
                case "Date":
                    destiny2Clan.OrderBy(d => d.ClanJoinDate).Include(c => c.Clan);
                    break;
                case "Date_desc":
                    destiny2Clan.OrderByDescending(m => m.ClanJoinDate).Include(c => c.Clan);
                    break;
                case "DateLastPlayed":
                    destiny2Clan.OrderBy(m => m.DateLastPlayed).Include(c => c.Clan);
                    break;
                case "DateLastPlayed_desc":
                    destiny2Clan.OrderByDescending(m => m.DateLastPlayed).Include(c => c.Clan);
                    break;
                default:
                    destiny2Clan.OrderBy(m => m.Name).Include(c => c.Clan);
                    break;
            }

            return View(await destiny2Clan.AsNoTracking().ToListAsync());
        }
    }
}