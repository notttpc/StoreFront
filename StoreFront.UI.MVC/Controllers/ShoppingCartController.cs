using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StoreFront.DATA.EF.Models;
using StoreFront.UI.MVC.Models;

namespace StoreFront.UI.MVC.Controllers
{

    public class ShoppingCartController : Controller
    {
        #region Steps to Implement Session Based Shopping Cart
        /*
         * 1) Register Session in program.cs (builder.Services.AddSession... && app.UseSession())
         * 2) Create the CartItemViewModel class in [ProjName].UI.MVC/Models folder
         * 3) Add the 'Add To Cart' button in the Index and/or Details view of your Products
         * 4) Create the ShoppingCartController (empty controller -> named ShoppingCartController)
         *      - add using statements
         *          - using GadgetStore.DATA.EF.Models;
         *          - using Microsoft.AspNetCore.Identity;
         *          - using GadgetStore.UI.MVC.Models;
         *          - using Newtonsoft.Json;
         *      - Add props for the GadgetStoreContext && UserManager
         *      - Create a constructor for the controller - assign values to context && usermanager
         *      - Code the AddToCart() action
         *      - Code the Index() action
         *      - Code the Index View
         *          - Start with the basic table structure
         *          - Show the items that are easily accessible (like the properties from the model)
         *          - Calculate/show the lineTotal
         *          - Add the RemoveFromCart <a>
         *      - Code the RemoveFromCart() action
         *          - verify the button for RemoveFromCart in the Index view is coded with the controller/action/id
         *      - Add UpdateCart <form> to the Index View
         *      - Code the UpdateCart() action
         *      - Add Submit Order button to Index View
         *      - Code SubmitOrder() action
         * */
        #endregion

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
            //EMpty shell to hold LOCAL shopping cart items
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

        //public Dictionary<int, CartItemViewModel> GetCart() -> JSON Deserialization here.
        //public void SetCart(Dictioary<int, CartItemViewModel) -> JSON Serialization and Session assignment
    }

}
