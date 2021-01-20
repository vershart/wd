using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Sludinajumi.Api.Models;
using Sludinajumi.Api.Data;

namespace Sludinajumi.Api.Controllers
{
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {

        private readonly SludinajumiContext _context;

        public CategoryController(SludinajumiContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public IEnumerable<Category> GetAll()
        {            
            return _context.Categories.ToList();
        }

        // [HttpGet]
        // public IEnumerable<Category> GetSubCategories(long id) 
        // {
        //     throw new NotImplementedException();
        // }

        [HttpGet("GetById/{id}", Name = "GetById"), ActionName("GetById")]
        public IActionResult GetById(long id)
        {
            var item = _context.Categories.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpGet("{title}", Name = "GetByUrlTitle")]
        public IActionResult GetByTitle(string title)
        {
            var item = _context.Categories
                .Include(i => i.Title).FirstOrDefault(t => t.UrlTitle == title);
            if (item == null)
                return NotFound();
            return new ObjectResult(item);       
        }

        [HttpPost]
        public IActionResult Create([FromBody] Category item)
        {
            if (item == null)
                return BadRequest();

            _context.Categories.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetByUrlTitle", new { title = item.UrlTitle }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Category item) {

            if (item == null || item.Id != id)
                return BadRequest();
            
            var category = _context.Categories.FirstOrDefault(t => t.Id == id);
            if (category == null)
                return NotFound();

            category.ImagePath = item.ImagePath;

            _context.Categories.Update(category);
            _context.SaveChanges();
            return new OkResult();

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var category = _context.Categories.FirstOrDefault(t => t.Id == id);
            if (category == null)
                return NotFound();

            _context.Categories.Remove(category);
            _context.SaveChanges();
            return new NoContentResult();
        }


    }
}