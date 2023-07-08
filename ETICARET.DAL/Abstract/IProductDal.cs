using ETICARET.ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ETICARET.DAL.Abstract
{
    public interface IProductDal
    {
        Product GetById(int id);
        Product GetOne(Expression<Func<Product,bool>> filter); //i => i.Id==id
        IQueryable<Product> GetAll(Expression<Func<Product, bool>> filter);
    }
}
