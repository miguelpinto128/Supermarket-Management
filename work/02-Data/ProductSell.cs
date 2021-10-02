using System;
using System.Collections.Generic;
using System.Text;
using work.Data;

namespace work.Data
{
    [Serializable]
    class ProductSell
    {
        public Product Product { get;  set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public ProductSell(Product Product, int Quantity, decimal Price)
        {
            this.Product = Product;
            this.Quantity = Quantity;
            this.Price = Price;
        }
    }
}
