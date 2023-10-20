using Microsoft.AspNetCore.Mvc.Rendering;

namespace APP.Models.Product.ViewModels
{
    public class CreateViewModel
    {
        public SelectList Status { get; set; }
        public ProductModel Product { get; set; }
    }
}
