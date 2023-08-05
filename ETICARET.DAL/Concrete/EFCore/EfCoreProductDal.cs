using ETICARET.DAL.Abstract;
using ETICARET.ENTITY;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ETICARET.DAL.Concrete.EFCore
{
    public class EfCoreProductDal : EfCoreGenericRepository<Product, ShopContext>, IProductDal
    {
        public override IEnumerable<Product> GetAll(Expression<Func<Product, bool>> filter)
        {
            using (var context = new ShopContext())
            {
                var products = context.Products.Include(i => i.Images).AsQueryable();

                return filter == null ? products.ToList() : products.Where(filter).ToList();
            }
        }

        public Product GetByIdWithCategories(int id)
        {
            using(var context = new ShopContext())
            {
                return context.Products.Where(i => i.Id == id)
                        .Include(i => i.ProductCategories)
                        .ThenInclude(i => i.Category)
                        .Include(i=>i.Images)
                        .FirstOrDefault();
            }
        }

        public List<Product> GetProductdByCategory(string category, int page, int pageSize)
        {
            using (var context = new ShopContext())
            {
                var products = context.Products.Include("Images").AsQueryable();

                if (!string.IsNullOrEmpty(category)) //category parametresi boş değilse
                {
                    products = products.Include(i => i.ProductCategories)
                        .ThenInclude(i => i.Category)
                        .Where(i => i.ProductCategories.Any(a => a.Category.Name.ToLower() == category.ToLower()));
                }
                return products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
        }

        public Product GetProductDetails(int id)
        {
            using (var context = new ShopContext())
            {
                return context.Products
                    .Where(i => i.Id == id)
                    .Include("Images")
                    .Include(i=> i.Comments)
                    .Include(i => i.ProductCategories)
                    .ThenInclude(i => i.Category)
                    .FirstOrDefault();
            }
        }

        public override void Update(Product entity)
        {
            using (var context = new ShopContext())
            {
                context.Images.RemoveRange(context.Images.Where(i => i.ProductId == entity.Id).ToList());

                var product =context.Products.Where(i => i.Id == entity.Id).FirstOrDefault();

                product.Description = entity.Description;
                product.Name = entity.Name;
                product.Price = entity.Price;
                product.Images = entity.Images;

                context.SaveChanges();
            }
        }

        public void Update(Product entity, int[] categoryIds)
        {
            using (var context = new ShopContext())
            {
                var product = context.Products
                        .Include(i => i.ProductCategories)
                        .ThenInclude(i => i.Category)
                        .FirstOrDefault();

                if (product != null)
                {
                    context.Images.RemoveRange(context.Images.Where(i => i.ProductId == entity.Id).ToList());

                   

                    product.Description = entity.Description;
                    product.Name = entity.Name;
                    product.Price = entity.Price;
                    product.Images = entity.Images;
                    product.ProductCategories = categoryIds.Select(catid => new ProductCategory()
                    {
                        ProductId = entity.Id,
                        CategoryId = catid
                    }).ToList();
                    context.SaveChanges();
                }
            }
        }
    }
}
