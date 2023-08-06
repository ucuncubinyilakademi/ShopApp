﻿using ETICARET.ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETICARET.DAL.Abstract
{
    public interface ICartDal : IRepository<Cart>
    {
        Cart GetCartBbyUserId(string userId);
    }
}
