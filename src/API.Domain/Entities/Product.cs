using System.ComponentModel.DataAnnotations.Schema;

namespace API.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int Stock { get; set; }
        public double Price { get; set; }
        
        [NotMapped]
        public double Discount { get; set; }
        [NotMapped]
        public double FinalPrice { get => Price * (100 - Discount) / 100; }
        [NotMapped]
        public string StatusName { get; set; }
    }
}