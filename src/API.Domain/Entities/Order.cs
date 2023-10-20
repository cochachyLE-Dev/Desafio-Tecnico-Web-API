using System;
using System.Collections.Generic;

namespace API.Domain.Entities
{
    public class Order
    {        
        public int Id { get; set; }
        public DateTime IssueIn { get; set; }
        public int ClientId { get; set; }
        public double TotalPrice { get; set; }
        public Client Client { get; set; }

        public List<OrderDetail> Details { get; set; }
        public Order Copy() => this.MemberwiseClone() as Order;
    }
}