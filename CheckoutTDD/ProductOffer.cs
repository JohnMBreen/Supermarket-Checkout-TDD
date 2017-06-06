using System;
using System.Collections.Generic;
using System.Text;

namespace CheckoutTDD
{
    public class ProductOffer
    {
        public string OfferSKU { get; set; }
        public int OfferPrice { get; set; }
        public int OfferQuantity { get; set; }
        public int CalulatePrice(Product product, int productQuantity)
        {
            int offerPrice = 0;
            // to get the number of offers you need to devide item quantity by the number of items in the offer
            // to get the full item quantity you need to get the remainder when divided by offer
            int fullQuantity = productQuantity % this.OfferQuantity;
            int offerQuantity = productQuantity / this.OfferQuantity;
            // Add price where full price is charged
            offerPrice += fullQuantity * product.UnitPrice;
            // Add price of offer items
            offerPrice += offerQuantity * this.OfferPrice;
            return offerPrice;
        }
    }
}
