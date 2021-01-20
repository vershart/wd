using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Sludinajumi.Api.Models;
using Sludinajumi.Api.Data;

namespace Sludinajumi.Api.Controllers
{
    [Route("api/[controller]")]
    public class AdController : Controller
    {

        private readonly SludinajumiContext _context;

        public AdController(SludinajumiContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
            _context.SaveChanges();

            if (_context.Ads.Count() == 0)
            {
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Atrast un izvadīt visus sludinājumus
        /// </summary>
        /// <returns>Sludinājumu saraksts JSON formātā</returns>
        [Produces("application/json")]
        [HttpGet]
        [AllowAnonymous]
        public IEnumerable<Ad> GetAll()
        {            
            return _context.Ads.ToList();
        }

        [HttpGet("{id}", Name = "GetAd")]
        [AllowAnonymous]
        public IActionResult GetById(long id)
        {
            var item = _context.Ads.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Create([FromBody] Ad item)
        {
            if (item == null)
                return BadRequest();

            _context.Ads.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetAd", new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Ad item) {

            if (item == null || item.Id != id)
                return BadRequest();
            
            var Ad = _context.Ads.FirstOrDefault(t => t.Id == id);
            if (Ad == null)
                return NotFound();

            Ad = item;

            _context.Ads.Update(Ad);
            _context.SaveChanges();
            return new OkResult();

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var Ad = _context.Ads.FirstOrDefault(t => t.Id == id);
            if (Ad == null)
                return NotFound();

            _context.Ads.Remove(Ad);
            _context.SaveChanges();
            return new NoContentResult();
        }


    }

}