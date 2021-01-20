using System;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Sludinajumi.Api.Models;
using Sludinajumi.Api.Data;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Sludinajumi.Controllers
{
    public class AdController : Controller
    {

        private readonly SludinajumiContext context;
        private readonly UserManager<ApplicationUser> userManager;
        
        public AdController(SludinajumiContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> New()
        {
            ViewData["Title"] = "Izvietot jaunu sludinājums";
            List<Category> categories = context.Categories.ToList();
            categories.Remove(categories.Where(cat => cat.Id == 1).FirstOrDefault());
            List<Language> languages = context.Languages.ToList();
            ViewBag.languages = languages;
            ViewBag.categories = categories;
            ApplicationUser curUser = await userManager.GetUserAsync(this.User);
            ViewBag.CreatedById = curUser.Id;
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> New(Ad model)
        {
            if (ModelState.IsValid)
            {
                await context.Ads.AddAsync(model);
                await context.SaveChangesAsync();
                var urlTitle = context.Categories.Find(model.CategoryId).UrlTitle;
                return RedirectToAction(nameof(ShowAd), new { Category = urlTitle, AdId = model.Id });
            }

            ViewData["Title"] = "Izvietot jaunu sludinājums";
            List<Category> categories = context.Categories.ToList();
            categories.Remove(categories.Where(cat => cat.Id == 1).FirstOrDefault());
            List<Language> languages = context.Languages.ToList();
            ViewBag.languages = languages;
            ViewBag.categories = categories;

            return View(model);
        }

        [HttpGet("/Categories/{Category}/Edit/{AdId}")]
        public IActionResult Edit(string Category, long AdId)
        {
            Ad model = context.Ads
                .Include(ad => ad.Category)
                .Include(ad => ad.CreatedBy)
                .Where(ad => ad.Id == AdId)
                .FirstOrDefault();

            if (model == null)
                return NotFound();

            ViewBag.categories = context.Categories.ToList();
            ViewBag.languages = context.Languages.ToList();
            return View(model);
        }

        [HttpPost("/Ad/Edit")]
        public async Task<IActionResult> Edit(Ad model)
        {
            if (model != null || ModelState.IsValid)
            {
                Ad ad = context.Ads.Find(model.Id);
                if (ad == null)
                    return NotFound();
                ad = model;
                context.Update(ad);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(ShowAd), new { Category = ad.Category.Title, AdId = ad.Id });
            }
            return View(model); 
        }

        [HttpGet("/Ad/Remove/{AdId}")]
        public async Task<IActionResult> Remove(long AdId)
        {
            Ad ad = await context.Ads.FindAsync(AdId);
            if (ad == null)
                return NotFound();
            context.Ads.Remove(ad);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(HomeController.Index));
        }


        [HttpGet("/Categories/{Category}/ShowAd/{AdId}")]
        public async Task<IActionResult> ShowAd(string Category, long AdId)
        {
            Ad model = context.Ads
                .Include(ad => ad.Category)
                .Include(ad => ad.CreatedBy)
                .Where(ad => ad.Id == AdId)
                .FirstOrDefault();
            if (model == null)
                return NotFound();
            
            var relatedAds = context.Ads.Include(ad => ad.Category).Where(ad => 
                (ad.CreatedAt.Ticks > DateTime.Now.AddDays(-1).Ticks &&
                ad.CreatedAt.Ticks < DateTime.Now.Ticks &&
                ad.Category.UrlTitle == Category)).ToList();
            ViewBag.relatedAds = relatedAds;
            ViewBag.relatedCount = relatedAds.Count();

            return View(model);
        }

        [HttpGet("/Categories/{UrlTitle}")]
        public async Task<IActionResult> ShowCategory(string UrlTitle)
        {
            Category category = await context.Categories.Include(cat => cat.Ads)
                .Include(cat => cat.Subcategories)
                .Where(cat => cat.UrlTitle == UrlTitle)
                .SingleOrDefaultAsync();
            return View(category);
        }

    }
}