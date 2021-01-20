using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sludinajumi.Manage.Models;
using Sludinajumi.Api.Data;
using Sludinajumi.Api.Models;

namespace Sludinajumi.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Administrator")]
    public class CategoryPropertiesController : Controller
    {
        private readonly SludinajumiContext context;
        private const string NeutralTitleDescription = "Īpašības nosaukums";
        private const string NeutralDescriptionDescription = "Īpašības apraksts";
        private const string NeutralLanguageCode = "lv";

        public CategoryPropertiesController(SludinajumiContext context)
        {
            this.context = context;
        }

        [HttpGet("/Manage/Categories/{UrlTitle}/Properties/List")]
        public async Task<IActionResult> ListCategoryProperties(string UrlTitle) 
        {
            Category category = context.Categories
                .Where(cat => cat.UrlTitle == UrlTitle)
                .Include(cat => cat.Properties)
                    .ThenInclude(cp => cp.Property)
                        .ThenInclude(te => te.Name)
                            .ThenInclude(te => te.Translations)
                .Include(cat => cat.Properties)
                    .ThenInclude(cp => cp.Property)
                        .ThenInclude(te => te.Description)
                            .ThenInclude(te => te.Translations)
                .FirstOrDefault();
            if (category == null)
                return NotFound();
            var indexes = category
                .Properties
                .Select(prop => prop.PropertyId);
            CategoryPropertiesViewModel model = new CategoryPropertiesViewModel() {
                Category = category,
                AvailableProperties = await context.Properties
                    .Where(prop => (!indexes.Contains(prop.Id)))
                        .Include(prop => prop.Name)
                            .ThenInclude(te => te.Translations)
                        .Include(prop => prop.Description)
                            .ThenInclude(te => te.Translations)
                    .ToListAsync()
            };

            return View(model);
        }

        [HttpGet("/Manage/Categories/{UrlTitle}/Properties/AddProperty")]
        public async Task<IActionResult> AddProperty(string UrlTitle)
        {
            long? categoryId = context.Categories
                .Where(cat => cat.UrlTitle == UrlTitle)
                .Select(cat => cat.Id)
                .SingleOrDefault();
            if (categoryId == null)
                return NotFound();
            NewCategoryPropertyViewModel model = new NewCategoryPropertyViewModel() { CategoryId = (long)categoryId, UrlTitle = UrlTitle };
            return View(model);
        }

        [HttpPost("/Manage/Categories/{UrlTitle}/Properties/AddProperty")]
        public async Task<IActionResult> AddProperty(string UrlTitle, NewCategoryPropertyViewModel model)
        {
            if (model == null)
                return RedirectToAction(nameof(Index));
            if (context.Categories.Find(model.CategoryId) == null)
                return NotFound();
            if (ModelState.IsValid)
            {
                Property prop = new Property() {
                    Name = new TranslationEntry() {
                        NeutralValue = model.PropertyNeutralValue,
                        NeutralDescription = NeutralTitleDescription,
                        Translations = new List<Translation>() {
                            new Translation() {
                                Value = model.PropertyNeutralValue,
                                LanguageCode = NeutralLanguageCode
                            }
                        }
                    },
                    Description = new TranslationEntry() {
                        NeutralValue = model.PropertyNeutralDescription,
                        NeutralDescription = NeutralDescriptionDescription,
                        Translations = new List<Translation>() {
                            new Translation() {
                                Value = model.PropertyNeutralDescription,
                                LanguageCode = NeutralLanguageCode
                            }
                        }
                    },
                    IsRequired = model.PropertyIsRequired
                };
                context.CategoryProperties.Add(
                    new CategoryProperty() {
                        CategoryId = model.CategoryId,
                        Property = prop
                    }
                );
                await context.SaveChangesAsync();

                return RedirectToAction(nameof(ManagePropertyTranslations), new { PropertyId = prop.Id, CategoryId = model.CategoryId });
            }
            return View(model);
        }

        [HttpGet("/Manage/Categories/Properties/{PropertyId}/ManageTranslations/")]
        public async Task<IActionResult> ManagePropertyTranslations(long PropertyId, long? CategoryId)
        {
            Property property = await context.Properties
                .Where(p => p.Id == PropertyId)
                .Include(p => p.Name)
                    .ThenInclude(te => te.Translations)
                        .ThenInclude(t => t.Language)
                .Include(p => p.Description)
                    .ThenInclude(te => te.Translations)
                        .ThenInclude(t => t.Language)
                .FirstOrDefaultAsync();
            List<Language> availableLanguages = await context.Languages.ToListAsync();
            var model = new ManagePropertyTranslationsViewModel() {
                Property = property,
                AvailableLanguages = availableLanguages
            };
            return View(model);
        }

        [HttpGet("/Manage/Categories/Properties/{PropertyId}/{LanguageCode}/Translate")]
        public async Task<IActionResult> TranslateProperty(long PropertyId, string LanguageCode)
        {
            Property property = await context.Properties
                .Where(p => p.Id == PropertyId)
                .Include(p => p.Name)
                    .ThenInclude(te => te.Translations)
                .Include(p => p.Description)
                    .ThenInclude(te => te.Translations)
                .FirstOrDefaultAsync();

            if (property == null)
                return NotFound(); // TODO: Decide better navigation

            Translation translatedName = property.Name.Translations
                .Where(t => t.LanguageCode == LanguageCode)
                .SingleOrDefault();

            Translation TranslatedDescription = property.Description.Translations
                .Where(t => t.LanguageCode == LanguageCode)
                .SingleOrDefault();

            // Jau ir tulkots šī valodā
            
            Language language = await context.Languages
                .Where(lang => lang.Code == LanguageCode)
                .FirstOrDefaultAsync();
            // Šī valoda (vēl) nav atbalstīta
            if (language == null)
                return RedirectToAction(nameof(Index)); // TODO: Decide better navigation

            PropertyTranslationViewModel model = new PropertyTranslationViewModel() {
                Property = property,
                Language = language,
                TranslatedName = (translatedName == null) ? "" : translatedName.Value,
                TranslatedDescription = (TranslatedDescription == null) ? "" : TranslatedDescription.Value
            };
            return View(model);

        }

        [HttpPost("/Manage/Categories/Properties/{PropertyId}/{LanguageId}/Translate")]
        public async Task<IActionResult> TranslateProperty(PropertyTranslationViewModel model)
        {
            Property property = await context.Properties
                .Where(p => p.Id == model.Property.Id)
                .Include(p => p.Name)
                    .ThenInclude(te => te.Translations)
                .Include(p => p.Description)
                    .ThenInclude(te => te.Translations)
                .FirstOrDefaultAsync();
            
            if (property == null)
                return NotFound();

            Language language = await context.Languages
                .Where(lang => lang.Code == model.Language.Code)
                .SingleOrDefaultAsync();

            if (language == null)
                return NotFound();

            long nameId = property.Name.Translations
                .Where(t => t.LanguageCode == language.Code)
                .Select(t => t.Id)
                .SingleOrDefault();
            long descriptionId = property.Description.Translations
                .Where(t => t.LanguageCode == language.Code)
                .Select(t => t.Id)
                .SingleOrDefault();
            
            if (nameId != 0)
            {
                Translation translation = await context.Translations
                    .Where(t => t.Id == nameId)
                    .FirstOrDefaultAsync();
                translation.Modify(model.TranslatedName);
                context.Update(translation);
            }
            else
            {
                property.Name.Translations.Add(new Translation() {
                    Value = model.TranslatedName,
                    LanguageCode = model.Language.Code
                });
            }
            if (descriptionId != 0)
            {
                Translation translation = await context.Translations
                    .Where(t => t.Id == descriptionId)
                    .FirstOrDefaultAsync();
                translation.Modify(model.TranslatedDescription);
                context.Update(translation);
            }
            else 
            {
                property.Description.Translations.Add(new Translation() {
                    Value = model.TranslatedDescription,
                    LanguageCode = model.Language.Code
                });
            }

            await context.SaveChangesAsync();

            return RedirectToAction(nameof(ManagePropertyTranslations), new { PropertyId = model.Property.Id });
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(context.Properties.ToList());
        }

        [HttpGet]
        public IActionResult New()
        {
            Property model = new Property();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> New(Property model)
        {
            if (model == null || !ModelState.IsValid)
                return View(model);
            await context.Properties.AddAsync(model);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        } 

        [HttpGet]
        public IActionResult Edit(long Id)
        {
            Property model = context.Properties
                .Where(prop => prop.Id == Id)
                .FirstOrDefault();
            if (model == null)
                return NotFound();
            return View(model);
        }

        [HttpPatch]
        public async Task<IActionResult> Edit(Property model)
        {
            if (model == null || !ModelState.IsValid)
                return View(model);
            Property property =  await context.Properties
                .FindAsync(model.Id);
            if (property == null)
                return NotFound();
            property.Name = model.Name;
            property.Description = model.Description;
            property.Categories = model.Categories;
            context.Properties.Update(property);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(long Id)
        {
            Property prop = await context.Properties
                .FindAsync(Id);
            if (prop == null)
                return NotFound();
            context.Properties.Remove(prop);
            await context.SaveChangesAsync();
            return View();
        }


    }
}