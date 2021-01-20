using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Sludinajumi.Api.Data;
using Sludinajumi.Api.Models;
using Sludinajumi.Manage.Models;
using System.IO;

namespace Sludinajumi.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Administrator")]
    public class CategoryController : Controller
    {

        private readonly SludinajumiContext context;
        
        public CategoryController(SludinajumiContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("/Manage/Categories/")]
        [Route("/Manage/Categories/Index")]
        [Route("/Manage/Categories/{UrlTitle?}")]
        public async Task<IActionResult> Index(string UrlTitle = "All")
        {
            CategoryViewModel model = new CategoryViewModel();
            model.Category = context.Categories
                .Include(cat => cat.Parent)
                .Include(cat => cat.Ads)
                .Include(cat => cat.Subcategories)
                    .ThenInclude(subcat => subcat.Subcategories)
                .Include(cat => cat.Subcategories)
                    .ThenInclude(subcat => subcat.Ads)
                .Where(cat => cat.UrlTitle == UrlTitle)
                .FirstOrDefault();

            if (model.Category == null)
                return NotFound();
            return View(model);
        }

        [HttpGet("/Manage/Categories/{UrlTitle}/Edit")]
        public async Task<IActionResult> Edit(string UrlTitle)
        {
            if (UrlTitle == null || UrlTitle == "")
                return new BadRequestResult();
            Category category = context.Categories.FirstOrDefault(cat => cat.UrlTitle == UrlTitle);
            if (category == null)
                return NotFound();
            ViewBag.parents = context.Categories.Where(cat => cat.Id != category.Id).ToList();
            ViewBag.files = GetFiles();
            return View(category);
        }

        [HttpPost("/Manage/Categories/{Id?}/Edit")]
        public async Task<IActionResult> Edit(long? Id, Category editedCategory)
        {
            if (!ModelState.IsValid)
                return View(editedCategory);
            Category category = context.Categories.FirstOrDefault(cat => cat.Id == Id);
            category.Title = editedCategory.Title;
            category.Description = editedCategory.Description;
            category.ImagePath = editedCategory.ImagePath;
            category.ParentId = editedCategory.ParentId;
            category.UrlTitle = editedCategory.UrlTitle;
            context.Categories.Update(category);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("/Manage/Categories/{UrlTitle?}/Details")]
        public async Task<IActionResult> Details(string UrlTitle)
        {
            var model = context.Categories
                .Include(cat => cat.Parent)
                .FirstOrDefault(cat => cat.UrlTitle == UrlTitle);
            if (model == null)
                return NotFound();
            return View(model);
        } 

        [HttpGet("/Manage/Categories/New")]
        public async Task<IActionResult> New()
        {
            CreateCategoryViewModel model = new CreateCategoryViewModel();
            model.Parents =  await context.Categories.ToListAsync();
            model.Pictures = GetFiles();
            return View(model);
        }

        [HttpPost("/Manage/Categories/New")]
        public async Task<IActionResult> New(CreateCategoryViewModel model)
        {
            if (model != null && ModelState.IsValid) {
                Category cat = new Category() {
                    Title = model.Title,
                    Description = model.Description,
                    UrlTitle = model.UrlTitle,
                    ImagePath = model.ImagePath,
                    ParentId = model.ParentId,
                };
                context.Categories.Add(cat);
                await context.SaveChangesAsync();
                return RedirectToAction(
                    "ListCategoryProperties",
                    "CategoryProperties",
                    new { UrlTitle = cat.UrlTitle });
            }
            model.Parents =  await context.Categories.ToListAsync();
            model.Pictures = GetFiles();
            return View(model);
        }

        [HttpGet("/Manage/Categories/{UrlTitle}/Remove")]
        public async Task<IActionResult> Remove(string UrlTitle)
        {
            Category category = await context.Categories
                .Include(cat => cat.Ads)
                .Include(cat => cat.Properties)
                    .ThenInclude(cp => cp.Property)
                        .ThenInclude(p => p.Name)
                            .ThenInclude(te => te.Translations)
                .Include(cat => cat.Properties)
                    .ThenInclude(cp => cp.Property)
                        .ThenInclude(p => p.Description)
                            .ThenInclude(te => te.Translations)
                .Include(cat => cat.Subcategories)
                    .Where(cat => cat.UrlTitle == UrlTitle).FirstOrDefaultAsync();
            if (category == null)
                return NotFound();
            if (category.Properties != null) {
                foreach (CategoryProperty prop in category.Properties)
                {
                    context.Properties.Remove(prop.Property);
                }
            }
            context.Categories.Remove(category);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public List<PictureInfoModel> GetFiles()
        {
            var root = Directory.GetCurrentDirectory();
            var pictures = System.IO.Path.Combine(root, "wwwroot\\static\\");

            var picsNames = Directory.EnumerateFiles(pictures).Select(x => new PictureInfoModel {
                Name = Path.GetFileName(x),
                Extension = Path.GetExtension(x) }).ToList();
            return picsNames;
        }

        [HttpPost]
        public async Task<string> UploadImage([FromForm]ImageUploadModel model)
        {
            if (model == null)
                return "Forma nav aizpildīta!";
            if (model.UploadedFile == null)
                return "Attēls nav izvēlēts!";
            if (model.FileName == null)
                return "Attēla nosaukums nav ievadīts!";

            var filePath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\static\\", 
                        model.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.UploadedFile.CopyToAsync(stream);
            }                                                                                          
            
            return filePath;

        }


    }
}