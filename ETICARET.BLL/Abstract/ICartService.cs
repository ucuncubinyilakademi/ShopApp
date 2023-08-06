using ETICARET.ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETICARET.BLL.Abstract
{
    public interface ICartService
    {
        void InitializeCart(string userId);
        Cart GetCartBbyUserId(string userId);
        void AddToCart(string userId,int productId,int quantity);
    }
}
