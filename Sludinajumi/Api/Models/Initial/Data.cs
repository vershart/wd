using System;
using System.Collections.Generic;
using Sludinajumi.Api.Models;

namespace Sludinajumi.Api.Models.Data
{
    public class InitialData
    {
        public List<Language> GenerateLanguages()
        {
            return new List<Language>() {
                new Language() {
                    Code = "lv",
                    SelfName = "latviešu",
                    NeutralSelfName = "latviešu"
                },
                new Language() {
                    Code = "en",
                    SelfName = "English",
                    NeutralSelfName = "angļu"
                },
                new Language() {
                    Code = "ru",
                    SelfName = "Русский",
                    NeutralSelfName = "krievu"
                }
            };
        }

        public List<Category> GenerateCategories()
        {

            return new List<Category>() {
                new Category() {
                    Title = "Sludinājumi",
                    Description = "Visi sludinājumi (pamatkategorija)",
                    ImagePath = "/static/all_ads.jpg",
                    UrlTitle = "All"
                },
                new Category() {
                    Title = "Elektrotehnika",
                    Description = "Tehnisko iekārtu sludinājumi",
                    ImagePath = "/static/all_electronics.jpg",
                    ParentId = 1,
                    UrlTitle = "Electronics"
                },
                new Category() {
                    Title = "Darbs",
                    Description = "Darba sludinājumi darba meklētājiem un darba dēvējiem",
                    ImagePath = "/static/all_jobs.jpg",
                    ParentId = 1,
                    UrlTitle = "Jobs"
                },
                new Category() {
                    Title = "Apģērbi",
                    Description = "Apģērbi un apavi visai ģimenei",
                    ImagePath = "/static/all_clothes_footwear.jpg",
                    ParentId = 1,
                    UrlTitle = "Clothes&Footwear"
                },
                new Category() {
                    Title = "Dzīvnieki",
                    Description = "Kaķīši, sunīši, zivji, putni un citi mūsu mazākie brāļi",
                    ImagePath = "/static/all_animals.jpg",
                    ParentId = 1,
                    UrlTitle = "Animals"
                },
                new Category() {
                    Title = "Nekustāmie īpašumi",
                    Description = "Privātas mājas, ēkas, dzīvokli, vasarnīcas un citi nekustāmie īpašumi",
                    ImagePath = "/static/all_realestates.jpg",
                    ParentId = 1,
                    UrlTitle = "RealEstates"
                },
                new Category() {
                    Title = "Transports",
                    Description = "Automobili, kravas automašīnas, velosipēdi, limuzīni un viss, kas ar tiem ir saistīts",
                    ImagePath = "/static/all_transport.jpg",
                    ParentId = 1,
                    UrlTitle = "Transport"
                },
            };
        }

    }
}