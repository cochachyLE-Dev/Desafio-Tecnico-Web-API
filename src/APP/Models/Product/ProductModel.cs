namespace APP.Models
{
    public class ProductModel
    {                        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int Stock { get; set; } = 1;
        public double Price { get; set; } = 0.00;

        public double Discount { get; set; } = 0.00;
        public double FinalPrice { get => Price * (100 * Discount) / 100; }
        public string StatusName { get; set; }
    }
    public class ProductStatus
    {        
        public int Status { get; set; }
        public string StatusName { get; set; }
    }
    public enum ProductStatusType
    {
        Active = 1,
        Inactive = 0
    }
}