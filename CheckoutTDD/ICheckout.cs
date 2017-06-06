// Checkout Interface
namespace CheckoutTDD
{
    public interface ICheckout
    {
        void Scan(string item);
        int GetTotalPrice();
    }
}
