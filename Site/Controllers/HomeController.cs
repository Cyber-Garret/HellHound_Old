using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Site.Models;
using Site.ViewModels;

namespace Site.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly DiscordSocketClient discord;
        private readonly DiscordSettings settings;

        public HomeController(IServiceProvider service)
        {
            logger = service.GetRequiredService<ILogger<HomeController>>();
            discord = service.GetRequiredService<DiscordSocketClient>();
            settings = service.GetRequiredService<IOptions<DiscordSettings>>().Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("Rules")]
        public IActionResult Privacy()
        {
            return View();
        }

        [Route("Admins")]
        public IActionResult Admins()
        {
            var guild = discord.GetGuild(settings.HoundDiscordServerId);
            var model = new AdminViewModel
            {
                Owner = guild.Owner,
                D2Cerberus = guild.Roles.First(r => r.Name == "D2 Цербер").Members,
                LACerberus = guild.Roles.First(r => r.Name == "LA Цербер").Members,
                WFCerberus = guild.Roles.First(r => r.Name == "WF Цербер").Members
            };

            return View(model);
        }

        [Route("Cyber_Garret")]
        public IActionResult Github() => RedirectPermanent($"https://github.com/Cyber-Garret");

        [Route("DiscordServer")]
        public IActionResult OpenDiscord() => RedirectPermanent($"https://discord.gg/UuqJDXy");

        [Route("SteamGroup")]
        public IActionResult OpenSteam() => RedirectPermanent($"https://steamcommunity.com/groups/D2_HellHound");

        [Route("VKGroup")]
        public IActionResult OpenVk() => RedirectPermanent($"https://github.com/Cyber-Garret");

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
