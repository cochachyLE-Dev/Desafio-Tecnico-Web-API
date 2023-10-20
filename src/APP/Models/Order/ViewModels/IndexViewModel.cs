using System.Collections.Generic;

namespace APP.Models.Order.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<OrderModel> Orders { get; set; }
    }
}
