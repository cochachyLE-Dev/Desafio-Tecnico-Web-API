using API.Domain.Entities;
using API.Domain.Shared;
using API.Persistence;
using API.Service.Implementation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace API.Service.Features.ProductFeatures.Queries
{
    public class GetAllQuery : IRequest<Response<Product>>
    {
        public class GetAllQueryHandler : IRequestHandler<GetAllQuery, Response<Product>>
        {
            private readonly IApplicationDbContext _context;
            private readonly MemoryCacheService _statusCacheService;
            public GetAllQueryHandler(IApplicationDbContext context, MemoryCacheService statusCache) => (_context, _statusCacheService) = (context, statusCache);
            public async Task<Response<Product>> Handle(GetAllQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var products = await _context.Products.ToListAsync();

                    foreach (var product in products)
                    {
                        product.Discount = 10;                        
                        product.StatusName = _statusCacheService.GetOrCreateProductStatus(product.Id)?.StatusName;
                    }

                    return Response<Product>.Success(products.AsReadOnly(),"Ok");
                }
                catch (Exception ex)
                {
                    return Response<Product>.Fail(StatusCode.InvalidArgument, ex.Message);
                }
            }
        }
    }
}
