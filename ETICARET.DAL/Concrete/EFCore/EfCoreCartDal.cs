using ETICARET.DAL.Abstract;
using ETICARET.ENTITY;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETICARET.DAL.Concrete.EFCore
{
    public class EfCoreCartDal : EfCoreGenericRepository<Cart, ShopContext>, ICartDal
    {
        public Cart GetCartBbyUserId(string userId)
        {
           using(var context = new ShopContext())
            {
                return context.Carts
                    .Include(i => i.CartItems)
                    .ThenInclude(i => i.Product)
                    .ThenInclude(i => i.Images)
                    .FirstOrDefault(i => i.UserId == userId);

            }
        }

        public override void Update(Cart entity)
        {
            using(var context = new ShopContext())
            {
                context.Carts.Update(entity);
                context.SaveChanges();
            }
        }
    }
}
