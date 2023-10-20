using APP.Models;

namespace APP.Data
{
    public interface IApiContext
    {
        ApiSet<OrderModel> Orders { get; }
        ApiSet<ClientModel> Clients { get; }
        ApiSet<ProductModel> Products { get; }
    }
}
