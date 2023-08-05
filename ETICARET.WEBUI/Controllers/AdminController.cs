using ETICARET.BLL.Abstract;
using ETICARET.ENTITY;
using ETICARET.WEBUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETICARET.WEBUI.Controllers
{
    [Authorize(Roles ="admin")]
    public class AdminController : Controller
    {
        private IProductService _productService;
        private ICategoryService _categoryService;

        public AdminController(IProductService productService,ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }
        [Route("/admin/products")]
        public IActionResult ProductList()
        {
            return View(new ProductListModel()
            {
                Products=_productService.GetAll()
            });
        }
        [HttpGet]
        public IActionResult CreateProduct()
        {
            return View(new ProductModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductModel model,List<IFormFile> files)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var entity = new Product()
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price
            };

            if (files != null)
            {
                foreach (var file in files)
                {
                    Image image = new Image();
                    image.ImageUrl = file.FileName;

                    entity.Images.Add(image);

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", file.FileName);
                    using(var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }

            _productService.Create(entity);

            return RedirectToAction("ProductList");
        }

        public IActionResult EditProduct(int? id)
        {
            if (id == null) return NotFound();
            var model = _productService.GetByIdWithCategories(id.Value);

            ViewBag.Categories = _categoryService.GetAll();

            return View(new ProductModel()
            {
                Id=model.Id,
                Name=model.Name,
                Images=model.Images,
                Description=model.Description,
                Price=model.Price,
                SelectedCategories = model.ProductCategories.Select(i=> i.Category).ToList()
            });            
        }
        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductModel model,List<IFormFile> files, int[] categoryIds)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _categoryService.GetAll();
                var m = _productService.GetByIdWithCategories(model.Id);
                model.Images = m.Images;
                model.SelectedCategories = m.ProductCategories.Select(i => i.Category).ToList();
                return View(model);
            }
                

            var entity = _productService.GetById(model.Id);

            if (entity == null)
            {
                return NotFound();
            }

            entity.Name = model.Name;
            entity.Price = model.Price;
            entity.Description = model.Description;           

            if (files != null)
            {
                foreach (var file in files)
                {
                    Image image = new Image();
                    image.ImageUrl = file.FileName;

                    entity.Images.Add(image);

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", file.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                }

                _productService.Update(entity,categoryIds);
                return RedirectToAction("ProductList");
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult DeleteProduct(int productId)
        {
            var entity = _productService.GetById(productId);
            if (entity != null)
            {
                _productService.Delete(entity);
            }

            return RedirectToAction("ProductList");
        }

        public IActionResult CategoryList()
        {
            return View(new CategoryListModel()
            {
                Categories=_categoryService.GetAll()
            });
        }
        public IActionResult CreateCategory()
        {
            return View(new CategoryModel());
        }
        [HttpPost]
        public IActionResult CreateCategory(CategoryModel model)
        {
            var entity = new Category()
            {
                Name = model.Name
            };
            _categoryService.Create(entity);
            return RedirectToAction("CategoryList");
        }
        public IActionResult EditCategory(int id)
        {
            var category = _categoryService.GetByIdWithProducts(id);
            return View(new CategoryModel()
            {
                Id=category.Id,
                Name=category.Name,
                Products=category.ProductCategories.Select(i=> i.Product).ToList()
            });
        }
        [HttpPost]
        public IActionResult EditCategory(CategoryModel model)
        {
            var entity = _categoryService.GetById(model.Id);

            if (entity == null)
                return NotFound();

            entity.Name = model.Name;
            _categoryService.Update(entity);
            return RedirectToAction("CategoryList");
        }
        [HttpPost]
        public IActionResult DeleteCategory(int categoryId)
        {
            var entity = _categoryService.GetById(categoryId);

            if (entity == null)
                return NotFound();

            
            _categoryService.Delete(entity);
            return RedirectToAction("CategoryList");
        }

        [HttpPost]
        public IActionResult DeleteFromCategory(int categoryId, int productId)
        {
            _categoryService.DeleteFromCategory(categoryId, productId);

            return Redirect("/admin/categories/" + categoryId);
        }
    }
}
