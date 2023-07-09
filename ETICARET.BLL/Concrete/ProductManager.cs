using ETICARET.BLL.Abstract;
using ETICARET.DAL.Abstract;
using ETICARET.DAL.Concrete.EFCore;
using ETICARET.ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETICARET.BLL.Concrete
{
    public class ProductManager : IProductService
    {
    #region Injection
        private IProductDal _productDal;

        public ProductManager(IProductDal productDal)
        {
            _productDal = productDal;
        }
    #endregion
        public void Create(Product entity)
        {
            _productDal.Create(entity);
        }

        public void Delete(Product entity)
        {
            _productDal.Delete(entity);
        }

        public List<Product> GetAll()
        {
            return _productDal.GetAll().ToList();
        }

        public Product GetById(int id)
        {
            return _productDal.GetById(id);
        }

        public void Update(Product entity)
        {
            _productDal.Update(entity);
        }
    }
}
