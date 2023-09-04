using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreFront.DATA.EF.Models;
using Microsoft.AspNetCore.Authorization;
using System.Drawing;
using GadgetStore.UI.MVC.Utilities;
using X.PagedList;

namespace StoreFront.UI.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly AnimeShopContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(AnimeShopContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Products
        [AllowAnonymous]
        public async Task<IActionResult> Index(string searchTerm, int categoryId = 0, int page = 1, int swordTypeId = 0, int genreId = 0)
        {
            var products = _context.Products.Include(p => p.Category).Include(p => p.Company).Include(p => p.Genre).Include(p => p.ProductStatus).Include(p => p.Sword).ToList();
            ViewBag.Categories = _context.Categories.ToList();

            #region Optional Category Filter
            ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "CategoryName", categoryId);
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Category = 0;//added to persist category during pagination

            if (swordTypeId != 0)
            {
                products = products.Where(p => p.SwordId == swordTypeId).ToList();
                ViewBag.SwordType = swordTypeId;
                ViewBag.NbrResults = products.Count;
                ViewBag.SearchTerm = searchTerm;
            }

            if (genreId != 0)
            {
                products = products.Where(p => p.GenreId == genreId).ToList();
                ViewBag.Genre = genreId;
                ViewBag.NbrResults = products.Count;
                ViewBag.SearchTerm = searchTerm;
            }

            if (categoryId != 0)
            {
                products = products.Where(p => p.CategoryId == categoryId).ToList();
                ViewBag.Category = categoryId;
                ViewBag.NbrResults = products.Count;
                ViewBag.SearchTerm = searchTerm;
            }
            #endregion

            #region Optional Search Filter
            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p =>
                p.ProductName.ToLower().Contains(searchTerm.ToLower())
                || p.Category.CategoryName.ToLower().Contains(searchTerm.ToLower())
                || p.ProductDescription.ToLower().Contains(searchTerm.ToLower())).ToList();
                
            }
            #endregion

            return View(products.ToPagedList(page, 6));
        }


        public async Task<IActionResult> AdminView()
        {
            var animeShopContext = _context.Products.Include(p => p.Category).Include(p => p.Company).Include(p => p.Genre).Include(p => p.ProductStatus).Include(p => p.Sword);
            return View(await animeShopContext.ToListAsync());
        }

        // GET: Products/Details/5
        [AllowAnonymous]
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

            ViewBag.Products = _context.Products.Where(c => c.CategoryId == product.CategoryId).ToList();
            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "CompanyName");
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreName");
            ViewData["ProductStatusId"] = new SelectList(_context.ProductStatuses, "ProductStatusId", "StatusDescription");
            ViewData["SwordId"] = new SelectList(_context.SwordTypes, "SwordId", "SwordType1");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ProductDescription,ProductImage,CategoryId,ProductStatusId,CompanyId,SwordId,GenreId,ProductPrice,IsFeatured,Image")] Product product)
        {
            if (ModelState.IsValid)
            {
                #region File Upload - CREATE

                //Check if a file was uploaded
                if (product.Image != null)
                {
                    //Check the file type of the image by retrieving the extension
                    string ext = Path.GetExtension(product.Image.FileName);

                    //Create a list of valid extensions to check against
                    string[] validExts = { ".jpeg", ".jpg", ".gif", ".png" };

                    //Verify that the uploaded file has one of the appropriate extensions listed above
                    //and that it is within the file size limitation for our .NET app
                    if (validExts.Contains(ext.ToLower()) && product.Image.Length < 4_194_303) //Underscores don't change the value
                                                                                               //of the number -- just the appearance
                    {
                        //Generate a unique file name for the image
                        product.ProductImage = Guid.NewGuid() + ext;

                        //Save the file to the web server (Here, in our app, we will save to wwwroot/images)
                        //To access the wwwroot, we have to add a field to the controller for the _webHostEnvironment.

                        //Retrieve the path to the wwwroot
                        string webRootPath = _webHostEnvironment.WebRootPath;

                        //Create the path for where we want to save the images
                        string fullImagePath = webRootPath + "/img/";

                        //Create a MemoryStream to read the image into server memory
                        using (var memoryStream = new MemoryStream())
                        {
                            //Transfer the file from the request into server memory
                            await product.Image.CopyToAsync(memoryStream);

                            //Create a copy of the image so we can manipulate & save it as needed
                            //(Will need to add a using statement for the System.Drawing namepsace)
                            using (var img = Image.FromStream(memoryStream))
                            {
                                //Use the ImageUtility for resizing and thumbnail creation

                                //Items needed for the ImageUtility.ResizeImage():
                                //1) (string) full path where the file will be saved
                                //2) (string) the name of the file
                                //3) (Image) the image itself
                                //4) (int) maxmimum image size
                                //5) (int) maximum thumbnail size

                                int maxImageSize = 500; //500 px
                                int maxThumbSize = 100; //100 px

                                ImageUtility.ResizeImage(fullImagePath, product.ProductImage, img, maxImageSize, maxThumbSize);

                                //If you need to save a file type that is a not an image...
                                //myFile.Save("path/to/folder", "fileName");

                            }
                        }

                    }
                }
                else
                {
                    //If no image was uploaded, assign a default file name. We can then add our 
                    //own placeholder image (with whatever file name you'd like, as well as an optional 
                    //t_ version for the thumbnail) to our wwwroot/images that will be displayed instead.

                    product.ProductImage = "noimage.png";
                }

                #endregion

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "CompanyName", product.CompanyId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreName", product.GenreId);
            ViewData["ProductStatusId"] = new SelectList(_context.ProductStatuses, "ProductStatusId", "StatusDescription", product.ProductStatusId);
            ViewData["SwordId"] = new SelectList(_context.SwordTypes, "SwordId", "SwordType1", product.SwordId);
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
            ViewData["SwordId"] = new SelectList(_context.SwordTypes, "SwordId", "SwordType1", product.SwordId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ProductDescription,ProductImage,CategoryId,ProductStatusId,CompanyId,SwordId,GenreId,ProductPrice,IsFeatured,Image")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                #region File Upload - EDIT

                //Retain the old image file name so we can delete it if a new file was uploaded
                string oldImageName = product.ProductImage;

                //Check to see if the user uploaded a file
                if (product.Image != null)
                {
                    //Get the file extension
                    string ext = Path.GetExtension(product.Image.FileName);

                    //Create a list of valid exts
                    string[] validExts = { ".jpeg", ".jpg", ".png", ".gif" };

                    //Check to ensure the ext is good and the image isn't too big
                    if (validExts.Contains(ext.ToLower()) && product.Image.Length < 4_194_303)
                    {
                        //Generate a unique file name
                        product.ProductImage = Guid.NewGuid() + ext;

                        //Build our file path to save the image
                        string webRootPath = _webHostEnvironment.WebRootPath;
                        string fullPath = webRootPath + "/img/";

                        //Delete the old image 
                        if (oldImageName != "noimage.png")
                        {
                            ImageUtility.Delete(fullPath, oldImageName);
                        }

                        //Save the new image
                        using (var memoryStream = new MemoryStream())
                        {
                            await product.Image.CopyToAsync(memoryStream);

                            using (var img = Image.FromStream(memoryStream))
                            {
                                int maxImageSize = 500;
                                int maxThumbSize = 100;

                                ImageUtility.ResizeImage(fullPath, product.ProductImage, img, maxImageSize, maxThumbSize);
                            }
                        }

                    }
                }

                #endregion

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
            ViewData["SwordId"] = new SelectList(_context.SwordTypes, "SwordId", "SwordType1", product.SwordId);
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
