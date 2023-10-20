using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace APP.Models
{
    public class OrderModel
    {
        public OrderModel() { }
        public OrderModel(int id, int clientId, OrderDetailModel item) {
            Id = id;
            ClientId = clientId;
            Details = new List<OrderDetailModel>();
            Details.Add(item);
        }
        
        public int Id { get; set; }        
        public DateTime IssueIn { get; set; }        
        public int ClientId { get; set; }        
        public double TotalPrice { get; set; }        
        public ClientModel Client { get; set; }

        public List<OrderDetailModel> Details { get; set; }
    }
}