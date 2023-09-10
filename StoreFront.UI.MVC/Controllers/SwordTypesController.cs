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
    public class SwordTypesController : Controller
    {
        private readonly AnimeShopContext _context;

        public SwordTypesController(AnimeShopContext context)
        {
            _context = context;
        }

        // GET: SwordTypes
        public async Task<IActionResult> Index()
        {
            ViewBag.Swords = _context.Products.Select(x => x.SwordId).ToList();
            return _context.SwordTypes != null ? 
                          View(await _context.SwordTypes.ToListAsync()) :
                          Problem("Entity set 'AnimeShopContext.SwordTypes'  is null.");
        }

        [Authorize]
        [AcceptVerbs("POST")]
        public JsonResult AjaxDelete(int id)
        {
            SwordType sword = _context.SwordTypes.Find(id);
            _context.SwordTypes.Remove(sword);
            _context.SaveChanges();

            string message = $"Deleted the sword {sword.SwordType1} from the database!";
            return Json(new { id, message });
        }

        public PartialViewResult SwordDetails(int id)
        {
            return PartialView(_context.SwordTypes.Find(id));
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AjaxCreate(SwordType sword)
        {
            _context.SwordTypes.Add(sword);
            _context.SaveChanges();
            return Json(sword);
        }

        [Authorize]
        [HttpGet]
        public PartialViewResult SwordEdit(int id)
        {
            return PartialView(_context.SwordTypes.Find(id));
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AjaxEdit(SwordType sword)
        {
            _context.Update(sword);
            _context.SaveChanges();
            return Json(sword);
        }

        #region Old Scaffold
        // GET: SwordTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SwordTypes == null)
            {
                return NotFound();
            }

            var swordType = await _context.SwordTypes
                .FirstOrDefaultAsync(m => m.SwordId == id);
            if (swordType == null)
            {
                return NotFound();
            }

            return View(swordType);
        }

        // GET: SwordTypes/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: SwordTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SwordId,SwordType1")] SwordType swordType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(swordType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(swordType);
        }

        // GET: SwordTypes/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SwordTypes == null)
            {
                return NotFound();
            }

            var swordType = await _context.SwordTypes.FindAsync(id);
            if (swordType == null)
            {
                return NotFound();
            }
            return View(swordType);
        }

        // POST: SwordTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SwordId,SwordType1")] SwordType swordType)
        {
            if (id != swordType.SwordId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(swordType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SwordTypeExists(swordType.SwordId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(swordType);
        }

        // GET: SwordTypes/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SwordTypes == null)
            {
                return NotFound();
            }

            var swordType = await _context.SwordTypes
                .FirstOrDefaultAsync(m => m.SwordId == id);
            if (swordType == null)
            {
                return NotFound();
            }

            return View(swordType);
        }

        // POST: SwordTypes/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SwordTypes == null)
            {
                return Problem("Entity set 'AnimeShopContext.SwordTypes'  is null.");
            }
            var swordType = await _context.SwordTypes.FindAsync(id);
            if (swordType != null)
            {
                _context.SwordTypes.Remove(swordType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        private bool SwordTypeExists(int id)
        {
          return (_context.SwordTypes?.Any(e => e.SwordId == id)).GetValueOrDefault();
        }
    }
}
