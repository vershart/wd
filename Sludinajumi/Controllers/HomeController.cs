using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sludinajumi.Api.Data;
using Sludinajumi.Api.Models;
using Sludinajumi.Models;

namespace Sludinajumi.Controllers
{
    public class HomeController : Controller
    {

        private readonly SludinajumiContext context;

        public HomeController(SludinajumiContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            ViewData["Title"] = "Visi sludinājumi";
            HomeViewModel model = new HomeViewModel();
            model.Categories = context.Categories.Where(cat => cat.ParentId == 1).ToList();
            long yesterday = DateTime.Now.AddDays(-1).Ticks;
            long today = DateTime.Now.Ticks;
            model.LatestAds = context.Ads.Include(ad => ad.Category).Where(ad => 
                (ad.CreatedAt.Ticks > yesterday &&
                ad.CreatedAt.Ticks < today)).ToList();
            return View(model);
        }

        public IActionResult About()
        {
            ViewData["Title"] = "Par projektu";
            return View();
        }

        public IActionResult Contacts()
        {
            ViewData["Title"] = "Kontakti";
            return View();
        }

        public IActionResult Uses()
        {
            ViewData["Title"] = "Izmantotie resursi";
            return View();
        }

    }
}