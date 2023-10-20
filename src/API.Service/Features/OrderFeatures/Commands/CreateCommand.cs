using API.Domain.Entities;
using API.Domain.Shared;
using API.Persistence;
using API.Service.Implementation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Service.Features.OrderFeatures.Commands
{
    public class CreateCommand : IRequest<Response<Order>>
    {                
        public int Id { get; set; }
        public int ClientId { get; set; }
        public double TotalPrice { get; set; }
        public string Hash { get; set; }

        public List<OrderDetail> Details { get; set; }

        public class CreateCommandHandler : IRequestHandler<CreateCommand, Response<Order>>
        {
            private readonly IApplicationDbContext _context;
            private readonly MemoryCacheService _memoryCacheService;
            public CreateCommandHandler(IApplicationDbContext context, MemoryCacheService memoryCacheService)
            {
                _context = context;
                _memoryCacheService = memoryCacheService;
            }
            public async Task<Response<Order>> Handle(CreateCommand request, CancellationToken cancellationToken)
            {
                Response<Order> response = new Response<Order>();
                try
                {
                    if (_memoryCacheService.OrderTryGetValue(request.ClientId, out Order o))
                    {
                        int maxid = _context.Orders.Count();
                        o.Id = ++maxid;

                        foreach (var item in o.Details)
                        {
                            item.OrderId = o.Id;
                        }
                                                
                        _context.Orders.Add(o);
                        await _context.SaveChangesAsync();

                        _memoryCacheService.RemoveOrder(request.ClientId);                    

                        response = Response<Order>.Success("Successful process");
                    }
                    else
                    {
                        response = Response<Order>.Fail(StatusCode.InvalidArgument, "Not found.");
                    }
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
