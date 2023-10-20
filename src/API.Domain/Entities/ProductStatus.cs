namespace API.Domain.Entities
{    
    public class ProductStatus
    {            
        public ProductStatus(int status, string statusName)
        {
            Status = status;
            StatusName = statusName;
        }

        public int Status { get; set; }
        public string StatusName { get; set; }
    }
    public enum ProductStatusType
    {
        Active = 1,
        Inactive = 0
    }
}