using System;
using System.Collections.Generic;
using System.Text;

namespace CheckoutTDD
{
    public class Checkout : ICheckout
    {
        private List<Product> _Products;
        private Dictionary<string, ProductOffer> _Offers;
        public Checkout() { }
        public Checkout(List<Product> products, Dictionary<string,ProductOffer> offers)
        {
            this._Products = products;
            this._Offers = offers;
        }

        // Implementation of ICheckout Scan method
        public void Scan(string item)
        {
            throw (new Exception("Not Implemented"));
        }


        // Implementation of ICheckout GetTotalPrice method
        public int GetTotalPrice()
        {
            return -1;
        }
    }
}
