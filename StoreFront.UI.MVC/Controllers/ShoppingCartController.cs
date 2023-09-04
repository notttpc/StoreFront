using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StoreFront.DATA.EF.Models;
using StoreFront.UI.MVC.Models;

namespace StoreFront.UI.MVC.Controllers
{

    public class ShoppingCartController : Controller
    {
        //fields
        private readonly AnimeShopContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ShoppingCartController(AnimeShopContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            //retrieve the contents of the session cart, if it exists, and convert those to C#
            //Var shoppingCart = GetCart();

            //Retrieve cart contenets from session:
            string? sessionCart = HttpContext.Session.GetString("cart");
            //create the shell dictionary
            Dictionary<int, CartItemViewModel> shoppingCart;

            if (string.IsNullOrEmpty(sessionCart))
            {
                shoppingCart = new();
            }
            else
            {
                shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart) ?? new();
            }

            if (!shoppingCart.Any())
            {
                ViewBag.Message = "There are no items in your cart";
            }
            else
            {
                ViewBag.Message = null;
                ViewBag.Total = shoppingCart.Values.Sum(x => x.Product.ProductPrice * x.Qty).ToString("c");
            }
            //Pass the collection back to a strongly-typed view to display.
            return View(shoppingCart);
        }

        public IActionResult AddToCart(int id)
        {
            //Empty shell to hold LOCAL shopping cart items
            //Key -> int for ProductId
            //Value -> CartItemViewModel -> Product & Qty
            Dictionary<int, CartItemViewModel> shoppingCart;

            var sessionCart = HttpContext.Session.GetString("cart");
            if (string.IsNullOrEmpty(sessionCart))
            {
                //if the session cart didnt exist yet, we need a new one.
                shoppingCart = new();
            }
            else
            {
                //session cart exists - trasnfer existing cart from session into out LOCAl variable shoppingCart
                shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart) ?? new();
            }
            //GetCart() ends

            //add newly selected product to the cart
            Product? product = _context.Products.Find(id);

            //if the product is already in the shopping cart, we want to increase the qty by 1.
            if (shoppingCart.ContainsKey(product.ProductId))
            {
                //update the qty
                shoppingCart[product.ProductId].Qty++;
            }
            else
            {
                //if the product is no in the cart, add it
                //Key              Value => new CartItemViewModel(Qty: 1, Product: product)
                shoppingCart.Add(product.ProductId, new(1, product));
            }
            //SetCart(shoppingCart)
            //update the session version of the cart
            //Take the LOCAL copy and serialize it to a JSON string. Then, assign that value to our session
            string jsonCart = JsonConvert.SerializeObject(shoppingCart);
            HttpContext.Session.SetString("cart", jsonCart);

            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int id)
        {
            if (id == null)
            {
                ViewBag.Message = "ERROR";
                return RedirectToAction("Index");
            }
            //var shoppingCart = GetCart();
            //The long way:
            var sessionCart = HttpContext.Session.GetString("cart");

            var shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);

            //remove the cart item:
            shoppingCart.Remove(id);

            //if there are no more cart items, remove the cart from session.

            if (shoppingCart.Count == 0)
            {
                HttpContext.Session.Remove("cart");
            }
            else
            {
                //update the session variable
                //SetCart(shoppingCart)
                string jsonCart = JsonConvert.SerializeObject(shoppingCart);
                HttpContext.Session.SetString("cart", jsonCart);
            }
            return RedirectToAction("Index");
        }

        public IActionResult UpdateCart(int productId, int qty)
        {
            if (qty <= 0)
            {
                RemoveFromCart(productId);
            }
            else
            {

                var sessionCart = HttpContext.Session.GetString("cart");
                var shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);

                shoppingCart[productId].Qty = qty;

                string jsonCart = JsonConvert.SerializeObject(shoppingCart);
                HttpContext.Session.SetString("cart", jsonCart);
            }
            return RedirectToAction("Index");
        }
        [Authorize]
        public async Task<IActionResult> CheckoutAsync()
        {
            var sessionCart = HttpContext.Session.GetString("cart");
            var shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);
            ViewBag.Total = shoppingCart.Sum(x => x.Value.Qty * x.Value.Product.ProductPrice).ToString("c");
            ViewBag.UserId = (await _userManager.GetUserAsync(HttpContext.User)).Id;
            //ViewBag.CartItem = shoppingCart.ToString($"{ProductPrice}");
            ViewBag.CartItemPrice = shoppingCart.Sum(x => x.Value.Product.ProductPrice).ToString("c");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SubmitOrder([Bind("OrderId,UserId,OrderDate,ShipToName,ShipCity,ShipState,ShipZip")] Order order)
        {
            #region Old Way

            ///* 
            // * Create an order object and then save to the DB
            // * - OrderDate will be now
            // * - UserId will be the current User
            // * - ShipToName, ShipCity, ShipState, ShipZip --> could be populated from a checkout page, but we're going to pull from the Userdetails page.
            // * - Add the order to the _context and save.
            // * 
            // * Create OrderProducts object for each item in the Cart
            // * - ProductId -> available from the cart
            // * - OrderId -> available from the Order Object
            // * - Qty -> from the cart
            // * - ProductPrice -> from the cart
            // * - Add the rocord to the _context and save
            // */
            //string? userId = (await _userManager.GetUserAsync(HttpContext.User))?.Id;

            ////Retrieve the UserDetails record associated with that ID
            //UserDetail? ud = _context.UserDetails.Find(userId);

            ////Create the order object and assign values.
            //Order o = new()
            //{
            //    OrderDate = DateTime.Now,
            //    UserId = userId,
            //    ShipCity = ud?.City ?? "Not Given",
            //    ShipToName = ud?.FullName ?? "Not Given",
            //    ShipState = ud?.State ?? "NO",
            //    ShipZip = ud?.Zip ?? "[N/A]"
            //};

            ////add it to the context
            //_context.Orders.Add(o);
            #endregion

            //order.OrderDate = DateTime.Now;
            //order.UserId = (await _userManager.GetUserAsync(HttpContext.User))?.Id;
            //TryValidateModel(order);
            //if (ModelState.IsValid)
            //{
            //Create the OrderProducts object
            if (ModelState.IsValid)
            {

                var sessionCart = HttpContext.Session.GetString("cart");
                var shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);

                foreach (var item in shoppingCart.Values)
                {
                    order.OrderProducts.Add(new()
                    {
                        OrderId = order.OrderId,
                        ProductId = item.Product.ProductId,
                        ProductPrice = item.Product.ProductPrice,
                        Quantity = (short)item.Qty
                    });
                }
                _context.Add(order);
                await _context.SaveChangesAsync();
                HttpContext.Session.Remove("cart");
                return RedirectToAction("Index", "Orders");
            }
            //}
            return View("Checkout", order);
        }
        //public Dictionary<int, CartItemViewModel> GetCart() -> JSON Deserialization here.
        //public void SetCart(Dictioary<int, CartItemViewModel) -> JSON Serialization and Session assignment
    }

}
