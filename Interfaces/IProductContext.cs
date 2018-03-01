﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IProductContext
    {
        DataTable GetAllProducts();
        DataTable GetAllProducts(int _orderID);
        DataRow GetProductByID(int _productID);
    }
}
