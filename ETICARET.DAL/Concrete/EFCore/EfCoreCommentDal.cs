using ETICARET.DAL.Abstract;
using ETICARET.ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETICARET.DAL.Concrete.EFCore
{
    public class EfCoreCommentDal:EfCoreGenericRepository<Comment,ShopContext>,ICommentDal
    {
    }
}
