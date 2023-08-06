using ETICARET.BLL.Abstract;
using ETICARET.WEBUI.Identity;
using ETICARET.WEBUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ETICARET.WEBUI.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private ICartService _cartService;
        private UserManager<ApplicationUser> _userManager;
        public CartController(ICartService cartService,UserManager<ApplicationUser> userManager)
        {
            _cartService = cartService;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var cart = _cartService.GetCartBbyUserId(_userManager.GetUserId(User));
            return View(new CartModel()
            {
                CartId=cart.Id,
                CartItems = cart.CartItems.Select(i=> new CartItemModel()
                {
                    CartItemId=i.Id,
                    ProductId=i.Product.Id,
                    Name=i.Product.Name,
                    Price=i.Product.Price,
                    Quantity=i.Quantity,
                    ImageUrl = i.Product.Images[0].ImageUrl
                }).ToList()
            });
        }

        [HttpPost]
        public IActionResult AddToCart(int productId,int quantity)
        {
            _cartService.AddToCart(_userManager.GetUserId(User), productId, quantity);
            return RedirectToAction("Index","Cart");
        }
    }
}
