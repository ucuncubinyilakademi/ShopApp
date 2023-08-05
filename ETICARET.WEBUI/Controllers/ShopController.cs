using ETICARET.BLL.Abstract;
using ETICARET.WEBUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ETICARET.WEBUI.Controllers
{
    public class ShopController : Controller
    {
        private IProductService _productService;
        public ShopController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("products/{category?}")] //products/telefon?page=2
        public IActionResult List(string category,int page=1)
        {
            const int pageSize = 3;
            return View(new ProductListModel()
            {
                PageModel = new PageInfo()
                {
                    TotalItems=_productService.GetCountByCategory(category),
                    CurrentPage=page,
                    ItemsPerPage=pageSize,
                    CurrentCategory=category
                },
                Products=_productService.GetProductdByCategory(category,page,pageSize)
            });
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _productService.GetProductDetails((int)id);

            if (product == null)
            {
                return NotFound();
            }
            return View(new ProductDetailsModel()
            {
                Product=product,
                Categories=product.ProductCategories.Select(i=> i.Category).ToList(),
                Comments=product.Comments
            }); 
        }
    }
}
