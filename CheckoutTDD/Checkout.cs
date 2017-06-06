using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CheckoutTDD
{
    public class Checkout : ICheckout
    {
        private List<Product> _Products;
        private Dictionary<string, ProductOffer> _Offers;
        private List<Product> _ShoppingBasket = new List<Product>();
        public Checkout() { }
        public Checkout(List<Product> products, Dictionary<string,ProductOffer> offers)
        {
            this._Products = products;
            this._Offers = offers;
        }

        // Implementation of ICheckout Scan method
        public void Scan(string item)
        {
            Product productitem = _Products.Find(x => x.SKU == item);
            if (productitem != null)
                _ShoppingBasket.Add(productitem);
            else
                throw (new Exception("Item not found"));
        }


        // Implementation of ICheckout GetTotalPrice method
        public int GetTotalPrice()
        {
            int value = 0;
            // As offers are only on single products 
            // it is 3 of A not 3 of (A or B) etc
            // So i can just get a count of each product
            var basketproducts = _ShoppingBasket.GroupBy(x => x.SKU, (sku, items) =>
                new {
                    ProductQuantity = items.Count(),
                    ProductItem = items.First()
                });

            foreach (var product in basketproducts)
            {
                // Check to see if product has an offer
                if (_Offers.ContainsKey(product.ProductItem.SKU))
                {
                    value += _Offers[product.ProductItem.SKU].CalulatePrice(product.ProductItem, product.ProductQuantity);
                }
                else
                    value += product.ProductQuantity * product.ProductItem.UnitPrice;
            }
            return value;
        }
    }
}
