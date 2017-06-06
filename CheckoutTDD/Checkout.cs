using System;
using System.Collections.Generic;
using System.Text;

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
            foreach (Product product in _ShoppingBasket)
                value += product.UnitPrice;
            return value;
        }
    }
}
