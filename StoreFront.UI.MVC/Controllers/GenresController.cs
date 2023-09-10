using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreFront.DATA.EF.Models;

namespace StoreFront.UI.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GenresController : Controller
    {
        private readonly AnimeShopContext _context;

        public GenresController(AnimeShopContext context)
        {
            _context = context;
        }

        // GET: Genres
        public async Task<IActionResult> Index()
        {
            ViewBag.Genres = _context.Products.Select(x => x.GenreId).ToList();
            return _context.Genres != null ? 
                          View(await _context.Genres.ToListAsync()) :
                          Problem("Entity set 'AnimeShopContext.Genres'  is null.");
        }

        [Authorize]
        [AcceptVerbs("POST")]
        public JsonResult AjaxDelete(int id)
        {
            Genre genre = _context.Genres.Find(id);
            _context.Genres.Remove(genre);
            _context.SaveChanges();

            string message = $"Deleted the genre {genre.GenreName} from the database!";
            return Json(new { id, message });
        }

        public PartialViewResult GenreDetails(int id)
        {
            return PartialView(_context.Genres.Find(id));
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AjaxCreate(Genre genre)
        {
            _context.Genres.Add(genre);
            _context.SaveChanges();
            return Json(genre);
        }

        [Authorize]
        [HttpGet]
        public PartialViewResult GenreEdit(int id)
        {
            return PartialView(_context.Genres.Find(id));
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AjaxEdit(Genre genre)
        {
            _context.Update(genre);
            _context.SaveChanges();
            return Json(genre);
        }

        #region Old Scaffold
        //// GET: Genres/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null || _context.Genres == null)
        //    {
        //        return NotFound();
        //    }

        //    var genre = await _context.Genres
        //        .FirstOrDefaultAsync(m => m.GenreId == id);
        //    if (genre == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(genre);
        //}

        //// GET: Genres/Create
        //[Authorize]
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Genres/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("GenreId,GenreName")] Genre genre)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(genre);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(genre);
        //}

        //// GET: Genres/Edit/5
        //[Authorize]
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.Genres == null)
        //    {
        //        return NotFound();
        //    }

        //    var genre = await _context.Genres.FindAsync(id);
        //    if (genre == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(genre);
        //}

        //// POST: Genres/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("GenreId,GenreName")] Genre genre)
        //{
        //    if (id != genre.GenreId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(genre);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!GenreExists(genre.GenreId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(genre);
        //}

        //// GET: Genres/Delete/5
        //[Authorize]
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.Genres == null)
        //    {
        //        return NotFound();
        //    }

        //    var genre = await _context.Genres
        //        .FirstOrDefaultAsync(m => m.GenreId == id);
        //    if (genre == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(genre);
        //}

        //// POST: Genres/Delete/5
        //[Authorize]
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.Genres == null)
        //    {
        //        return Problem("Entity set 'AnimeShopContext.Genres'  is null.");
        //    }
        //    var genre = await _context.Genres.FindAsync(id);
        //    if (genre != null)
        //    {
        //        _context.Genres.Remove(genre);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}
        #endregion

        private bool GenreExists(int id)
        {
          return (_context.Genres?.Any(e => e.GenreId == id)).GetValueOrDefault();
        }
    }
}
