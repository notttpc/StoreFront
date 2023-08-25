﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreFront.DATA.EF.Models;

namespace StoreFront.UI.MVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AnimeShopContext _context;

        public ProductsController(AnimeShopContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var animeShopContext = _context.Products.Include(p => p.Category).Include(p => p.Company).Include(p => p.Genre).Include(p => p.ProductStatus).Include(p => p.Sword);
            return View(await animeShopContext.ToListAsync());
        }

        public async Task<IActionResult> AdminView()
        {
            var animeShopContext = _context.Products.Include(p => p.Category).Include(p => p.Company).Include(p => p.Genre).Include(p => p.ProductStatus).Include(p => p.Sword);
            return View(await animeShopContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Company)
                .Include(p => p.Genre)
                .Include(p => p.ProductStatus)
                .Include(p => p.Sword)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "CompanyName");
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreName");
            ViewData["ProductStatusId"] = new SelectList(_context.ProductStatuses, "ProductStatusId", "StatusDescription");
            ViewData["SwordId"] = new SelectList(_context.SwordTypes, "SwordId", "SwordId");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ProductDescription,ProductImage,CategoryId,ProductStatusId,CompanyId,SwordId,GenreId,ProductPrice,IsFeatured")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "CompanyName", product.CompanyId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreName", product.GenreId);
            ViewData["ProductStatusId"] = new SelectList(_context.ProductStatuses, "ProductStatusId", "StatusDescription", product.ProductStatusId);
            ViewData["SwordId"] = new SelectList(_context.SwordTypes, "SwordId", "SwordId", product.SwordId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "CompanyName", product.CompanyId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreName", product.GenreId);
            ViewData["ProductStatusId"] = new SelectList(_context.ProductStatuses, "ProductStatusId", "StatusDescription", product.ProductStatusId);
            ViewData["SwordId"] = new SelectList(_context.SwordTypes, "SwordId", "SwordId", product.SwordId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ProductDescription,ProductImage,CategoryId,ProductStatusId,CompanyId,SwordId,GenreId,ProductPrice,IsFeatured")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "CompanyName", product.CompanyId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreName", product.GenreId);
            ViewData["ProductStatusId"] = new SelectList(_context.ProductStatuses, "ProductStatusId", "StatusDescription", product.ProductStatusId);
            ViewData["SwordId"] = new SelectList(_context.SwordTypes, "SwordId", "SwordId", product.SwordId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Company)
                .Include(p => p.Genre)
                .Include(p => p.ProductStatus)
                .Include(p => p.Sword)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'AnimeShopContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}