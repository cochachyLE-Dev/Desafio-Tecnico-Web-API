namespace APP.Models
{
    public class OrderDetailModel
    {
        public int Row { get; set; }
        public int OrderId { get; set; }
        public int ClientId { get; set; }
        public int ProductId { get; set; }
        public string Description { get; set; }
        public int Qty { get; set; }
        public double UnitPrice { get; set; }
        public double Total { get; set; }
                
        public ProductModel Product { get; set; }
    }
}