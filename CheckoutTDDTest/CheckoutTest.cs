using System;
using Xunit;
using CheckoutTDD;
using System.Collections.Generic;
using System.Linq;

namespace CheckoutTDDTest
{
    public class CheckoutTest
    {
        // In reality this would be mocked as data would come from a database
        private List<Product> SetUpProductStore()
        {
            // Set up mock product data. This would probably be in a DB in production.
            List<Product> _productStore = new List<Product>();
            _productStore.Add(new Product { SKU = "A", UnitPrice = 50 });
            _productStore.Add(new Product { SKU = "B", UnitPrice = 30 });
            _productStore.Add(new Product { SKU = "C", UnitPrice = 20 });
            _productStore.Add(new Product { SKU = "D", UnitPrice = 15 });
            return _productStore;
        }

        // In reality this would be mocked as data would come from a database
        private Dictionary<string, ProductOffer> SetUpProductOffer()
        {
            // Set up mock product data. This would probably be in a DB in production.
            Dictionary<string, ProductOffer> _productOffer = new Dictionary<string, ProductOffer>();
            _productOffer.Add("A", new ProductOffer { OfferSKU = "A", OfferPrice = 130, OfferQuantity = 3 });
            _productOffer.Add("B", new ProductOffer { OfferSKU  = "B", OfferPrice = 45, OfferQuantity = 2 });
            return _productOffer;
        }

        [Fact]
        public void Initial_Checkout_Zero()
        {
            var checkout = new Checkout();

            var total = checkout.GetTotalPrice();

            Assert.Equal(0, total);
        }

        [Theory] // The Theory is that products can be scanned
        [InlineData("A", true)]
        [InlineData("B", true)]
        [InlineData("C", true)]
        [InlineData("D", true)]
        public void Can_Scan_Product_List(string sku, bool value)
        {
            Assert.True(IsItemScan(sku));
        }

        bool IsItemScan(string sku)
        {
            Dictionary<string, ProductOffer> _productOffer = SetUpProductOffer();
            List<Product> _productStore = SetUpProductStore();

            var checkout = new Checkout(_productStore, _productOffer);
            bool retv = false;
            try
            {
                checkout.Scan(sku);
                retv = true;
            }catch(Exception)
            {
                retv = false;
            }

            return retv;
        }


        [Theory] // The Theory is if 1 item is added to the basket the item prices should be return
        [InlineData("A", 50)]
        [InlineData("B", 30)]
        [InlineData("C", 20)]
        [InlineData("D", 15)]
        public void Can_Get_Correct_Price_For_Single_Item(string sku, int value)
        {
            Assert.Equal(value , IsSingleItem(sku));
        }

        int IsSingleItem(string sku)
        {
            Dictionary<string, ProductOffer> _productOffer = SetUpProductOffer();
            List<Product> _productStore = SetUpProductStore();

            var checkout = new Checkout(_productStore, _productOffer );
            checkout.Scan(sku);
            return checkout.GetTotalPrice();
            
        }

        [Theory] // Single product offer price, This will test dfferent sku and quanity
        [InlineData("A", 3, 130)]
        [InlineData("B", 2, 45)]
        public void Can_Gets_Multi_Offer_Discount(string sku, int offerquanity, int offerprice)
        {
            Dictionary<string, ProductOffer> _productOffer = SetUpProductOffer();
            List<Product> _productStore = SetUpProductStore();

            var checkout = new Checkout(_productStore, _productOffer);

            for (int iCnt = 0; iCnt < offerquanity; iCnt++)
                checkout.Scan(sku);

            var total = checkout.GetTotalPrice();

            Assert.Equal(offerprice,total);
        }

        [Theory]
        [InlineData("A", 7 , 310)] // 2 x Offer + unit price (2 * 130) + 50 = 310
        [InlineData("A", 8, 360)] // 2 x Offer + 2 x unit price (2 * 130) + (2 * 50) = 360
        public void Is_Full_Price_Used_For_Extra_Items_When_Not_Getting_Exact_Multiple_Of_Offer_Quantity(string sku, int quantity, int value)
        {
            Dictionary<string, ProductOffer> _productOffer = SetUpProductOffer();
            List<Product> _productStore = SetUpProductStore();

            var checkout = new Checkout(_productStore, _productOffer);

            for (int iCnt = 0; iCnt < quantity; iCnt++)
                checkout.Scan(sku);

            var total = checkout.GetTotalPrice();
            Assert.Equal(value, total);
        }

        // Can Calculate offers correctly for random list of items
        // [InlineData("A,A,A,A,A,A,A,A,B,B,B,B,B,B,B,C,C,C,D,D", 565)]
        // A = 2 x Offer + 2x Unit (2 * 130)  + (2 * 50) = 310
        // B = 3 x Offer + 1 x Unit (3 * 45) + 30 = 165 
        // C 3 x Unit   3 x 20 = 60
        // D 2 x Unit   2 X 15 = 30
        // [InlineData("A,A,B,C,C,C,D,D", 220)]
        // A = 2x Unit 2 * 50 = 100
        // B = 1 x Unit = 30 
        // C 3 x Unit   3 x 20 = 60
        // D 2 x Unit   2 X 15 = 30
        [Theory]
        [InlineData("A,A,A,A,A,A,A,A,B,B,B,B,B,B,B,C,C,C,D,D", 565 )]
        [InlineData("A,A,B,C,C,C,D,D", 220)]
        public void Can_Calculate_Offers_For_Random_Mixture_of_Items(string itemssku, int value)
        {
            Dictionary<string, ProductOffer> _productOffer = SetUpProductOffer();
            List<Product> _productStore = SetUpProductStore();

            var checkout = new Checkout(_productStore, _productOffer);

            var rnd = new Random();
            itemssku.Split(',').ToList().OrderBy(x => rnd.Next()).ToList().ForEach(x => checkout.Scan(x));
            var total = checkout.GetTotalPrice();
            Assert.Equal(value, total);
        }

    }
}
