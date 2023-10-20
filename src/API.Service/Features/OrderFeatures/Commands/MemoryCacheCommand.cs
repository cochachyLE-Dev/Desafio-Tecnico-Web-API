using API.Domain.Entities;
using API.Domain.Shared;
using API.Persistence;
using API.Service.Implementation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Service.Features.OrderFeatures.Commands
{
    public class MemoryCacheCommand : IRequest<Response<Order>>
    {                
        public int Id { get; set; }
        public int ClientId { get; set; }
        public double TotalPrice { get; set; }

        public List<OrderDetail> Details { get; set; }

        public class MemoryCacheCommandHandler : IRequestHandler<MemoryCacheCommand, Response<Order>>
        {            
            private readonly MemoryCacheService _memoryCacheService;
            public MemoryCacheCommandHandler(MemoryCacheService memoryCacheService)
            {
                _memoryCacheService = memoryCacheService;
            }
            public async Task<Response<Order>> Handle(MemoryCacheCommand request, CancellationToken cancellationToken)
            {
                Response<Order> response = new Response<Order>();
                try
                {
                    Order o = new Order();
                    o.ClientId = request.ClientId;
                    o.TotalPrice = request.TotalPrice;
                    o.Details = request.Details ?? new List<OrderDetail>();
                    o.IssueIn = DateTime.Now;

                    o = await _memoryCacheService.GetOrCreateOrderAsync(request.ClientId, o);

                    response = Response<Order>.Success(o, "Successful process");                    
                }
                catch (Exception ex)
                {
                    response = Response<Order>.Fail(StatusCode.InvalidArgument, ex.Message);
                }
                return response;
            }
        }
    }
}
