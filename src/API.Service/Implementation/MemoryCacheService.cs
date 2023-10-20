using API.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Service.Implementation
{
    public class MemoryCacheService
    {    
        public MemoryCache ProductMemoryCache { get; } = new MemoryCache(new MemoryCacheOptions
        {
            SizeLimit = 1024 * 4
        });

        public MemoryCacheOptions Options = new MemoryCacheOptions()
        {
            SizeLimit = 1024 * 4,
            ExpirationScanFrequency = TimeSpan.FromDays(1)
        };
        public MemoryCache OrderMemoryCache { get; set; }
        public MemoryCacheService()
        {
            OrderMemoryCache = new MemoryCache(Options);
        }

        public ProductStatus GetOrCreateProductStatus(int productId, ProductStatusType statusType = ProductStatusType.Active)
        {
            return ProductMemoryCache.GetOrCreate<ProductStatus>(productId, (ICacheEntry cacheEntry) =>
            {
                cacheEntry.SlidingExpiration = TimeSpan.FromSeconds(1);
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                cacheEntry.Size = 1;
                return new ProductStatus((int)statusType, Enum.GetName(typeof(ProductStatusType), statusType));
            });
        }
        public bool OrderTryGetValue(int clientId, out Order order)
        {
            return OrderMemoryCache.TryGetValue(clientId, out order);
        }
        public void RemoveOrder(int clientId) => OrderMemoryCache.Remove(clientId);
        public async Task<Order> GetOrCreateOrderAsync(int clientId, Order order)
        {
            return await Task.Run<Order>(() =>
            {
                if (!OrderMemoryCache.TryGetValue(clientId, out Order o))
                    getOrCreateOrder(clientId, o = order);
                else
                {
                    o.IssueIn = order.IssueIn;
                    o.ClientId = order.ClientId;
                    o.Client = order.Client;
                    o.TotalPrice = order.TotalPrice;

                    var detail = order.Details.Except(o.Details);
                    foreach (var item in detail)
                    {
                        if(!o.Details.Exists(c => c.ProductId == item.ProductId))
                            o.Details.Add(item);
                        else
                            o.Details.Single(c => c.ProductId == item.ProductId).Qty += item.Qty;
                    }

                    OrderMemoryCache.Remove(clientId);
                    getOrCreateOrder(clientId, o);
                }
                return o;
            });

            Order getOrCreateOrder(int clientId, Order o)
            {
                return OrderMemoryCache.GetOrCreate<Order>(clientId, (ICacheEntry cacheEntry) =>
                {
                    cacheEntry.SlidingExpiration = TimeSpan.FromHours(24);
                    cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24);
                    cacheEntry.Size = 1;
                    
                    return o;
                });
            }
        }
    }
}