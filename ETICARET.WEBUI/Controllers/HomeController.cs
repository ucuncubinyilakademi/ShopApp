using ETICARET.BLL.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace ETICARET.WEBUI.Controllers
{
    public class HomeController : Controller
    {
        private IProductService _productService;

        public HomeController(IProductService productService)
        {
            _productService = productService;
        }
        public IActionResult Index()
        {
            return View(_productService.GetAll());
        }
    }
}
