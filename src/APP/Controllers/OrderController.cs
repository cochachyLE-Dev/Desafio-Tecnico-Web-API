using APP.Data;
using APP.Models;
using APP.Models.Order.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace APP.Controllers
{
    public class OrderController: Controller
    {
        private readonly IApiContext _apiContext;
        public OrderController(IApiContext apiContext)
        {
            _apiContext = apiContext;
        }
        [ActionName("Index")]
        public async Task<IActionResult> IndexAsync()
        {
            var orders = await _apiContext.Orders.GetAllAsync();
            return View(orders.List);
        }
        [ActionName("Create")]
        public async Task<IActionResult> CreateAsync()
        {
            var clients = await _apiContext.Clients.GetAllAsync();
             
            CreateViewModel vmodel = new CreateViewModel();
            vmodel.Order = new OrderModel();
            vmodel.Order.Details = new List<OrderDetailModel>();
            vmodel.Clients = new SelectList(clients.List, nameof(ClientModel.Id), nameof(ClientModel.FullName));
            vmodel.DisableDateArray = await vmodel.GetDisabledDateArrayDefault(new DateTime[] { });
            vmodel.DateRange = await vmodel.GetDateRangeDefault();

            return View(vmodel);
        }
        [HttpPost]        
        [ActionName("Save")]
        public async Task<IActionResult> SaveAsync(OrderModel order)
        {            
            var rq =  await _apiContext.Orders.InsertAsync(order);            
            return Ok();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Create")]
        public async Task<IActionResult> CreateAsync([Bind("OrderId,ClientId,ProductId,Description,Qty,UnitPrice")] OrderDetailModel item)
        {
            if (ModelState.IsValid)
            {
                OrderModel order = new OrderModel(item.OrderId, item.ClientId, item);
                var orderDetail = await _apiContext.Orders.InsertAsync(order, inMemoryCache: true);                
                return PartialView("~/Views/Order/Partials/grid_order_detail.cshtml", orderDetail.Entity?.Details ?? new List<OrderDetailModel>());
            }
            else
            { 
                return NotFound();
            }
        }        
        [ActionName("Add")]
        public async Task<IActionResult> AddAsync(OrderModel order) 
        {
            var saveorder = await _apiContext.Orders.InsertAsync(order, inMemoryCache: true);
            var products = await _apiContext.Products.GetAllAsync();

            CreateDetailViewModel vmodel = new CreateDetailViewModel();
            vmodel.Products = new SelectList(products.List, nameof(ProductModel.Id), nameof(ProductModel.Description));
            vmodel.Item = new OrderDetailModel { OrderId = saveorder.Entity.Id, ClientId = saveorder.Entity.ClientId };

            return PartialView("~/Views/Order/Partials/grid_order_detail_item.cshtml", vmodel);
        }
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteAsync(int orderId)
        {
            var delete = await _apiContext.Orders.DeleteAsync(orderId);            
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ActionName("DeleteItem")]
        public async Task<IActionResult> DeleteItemAsync(int orderId, int productId)
        {
            var delete = await _apiContext.Orders.DeleteItemAsync(orderId, productId);
            var orderDetail = await _apiContext.Orders.SingleOrDefaultAsync(orderId);

            return PartialView("~/Views/Order/Partials/grid_order_detail.cshtml", orderDetail.List?.ToArray()[0]?.Details ?? new List<OrderDetailModel>());
        }
    }
}
