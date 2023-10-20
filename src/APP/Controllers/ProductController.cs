using APP.Data;
using APP.Models;
using APP.Models.Product.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APP.Controllers
{
    public class ProductController: Controller
    {
        private readonly IApiContext _apiContext;
        public ProductController(IApiContext apiContext)
        {
            _apiContext = apiContext;
        }
        [ActionName("Index")]
        public async Task<IActionResult> IndexAsync()
        {
            var products = await _apiContext.Products.GetAllAsync();
            return View(products.List);
        }
        [HttpGet]
        [ActionName("Create")]
        public async Task<IActionResult> CreateAsync()
        {
            List<ProductStatus> productStatus = new List<ProductStatus>()
            {
                new ProductStatus{ Status = (int)ProductStatusType.Active, StatusName = Enum.GetName(typeof(ProductStatusType), ProductStatusType.Active) },
                new ProductStatus{ Status = (int)ProductStatusType.Inactive, StatusName = Enum.GetName(typeof(ProductStatusType), ProductStatusType.Inactive) }
            };
            
            CreateViewModel vmodel = new CreateViewModel();
            vmodel.Product = new ProductModel();
            vmodel.Status = new SelectList(productStatus, nameof(ProductStatus.Status), nameof(ProductStatus.StatusName));

            return View(vmodel);
        }
        [HttpPost]
        [ActionName("Create")]
        public async Task<IActionResult> CreateAsync(ProductModel product)
        {
            if (ModelState.IsValid)
            {                
                await _apiContext.Products.InsertAsync(product);
                return RedirectToAction("Index");
            }
            else
            { 
                return NotFound();
            }
        }        

        [ActionName("Delete")]
        public async Task<IActionResult> DeleteAsync(int productId)
        {
            var delete = await _apiContext.Products.DeleteAsync(productId);            
            return RedirectToAction("Index");
        }
    }
}
