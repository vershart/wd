using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Sludinajumi.Api.Data;

namespace Sludinajumi.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Administrator")]
    public class DashboardController : Controller
    {

        private SludinajumiContext context { get; set; }

        public DashboardController(SludinajumiContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.adsCount = context.Ads.LongCount();
            ViewBag.usersCount = context.Users.LongCount();
            ViewBag.categoriesCount = context.Categories.LongCount();
            ViewData["Title"] = "Pārvaldes panelis";
            return View();
        }

    }
}