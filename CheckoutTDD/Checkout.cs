using System;
using System.Collections.Generic;
using System.Text;

namespace CheckoutTDD
{
    public class Checkout : ICheckout
    {
        // Implementation of ICheckout Scan method
        public void Scan(string item)
        { }

        // Implementation of ICheckout GetTotalPrice method
        public int GetTotalPrice()
        {
            return 0;
        }
    }
}
