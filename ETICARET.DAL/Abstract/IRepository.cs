using ETICARET.ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ETICARET.DAL.Abstract
{
    // T: Generic data type
    public interface IRepository<T>
    {
        T GetById(int id);
        T GetOne(Expression<Func<T, bool>> filter); //i => i.Id==id
        IEnumerable<T> GetAll(Expression<Func<T, bool>> filter=null);

        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
