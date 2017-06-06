using System;
using Xunit;
using CheckoutTDD;

namespace CheckoutTDDTest
{
    public class CheckoutTest
    {
        [Fact]
        public void Initial_Checkout_Zero()
        {
            var checkout = new Checkout();

            var total = checkout.GetTotalPrice();

            Assert.Equal(total, 0);
        }
    }
}
