using ETICARET.BLL.Abstract;
using ETICARET.DAL.Abstract;
using ETICARET.ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETICARET.BLL.Concrete
{
    public class CartManager : ICartService
    {
        private ICartDal _cartDal;
        public CartManager(ICartDal cartDal)
        {
            _cartDal = cartDal;
        }

        public void AddToCart(string userId, int productId, int quantity)
        {
            var cart = GetCartBbyUserId(userId);

            if (cart != null)
            {
                int index = cart.CartItems.FindIndex(i => i.ProductId == productId);

                if (index < 0)
                {
                    cart.CartItems.Add(new CartItem()
                    {
                        ProductId = productId,
                        Quantity = quantity,
                        CartId = cart.Id
                    });
                }
                else
                {
                    cart.CartItems[index].Quantity += quantity;
                }

                _cartDal.Update(cart);
            }
        }

        public Cart GetCartBbyUserId(string userId)
        {
            return _cartDal.GetCartBbyUserId(userId);
        }

        public void InitializeCart(string userId)
        {
            _cartDal.Create(new Cart() { UserId=userId});
        }
    }
}
