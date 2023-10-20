using API.Domain.Entities;
using API.Domain.Shared;
using API.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Service.Features.OrderFeatures.Queries
{
    public class SingleByIdQuery : IRequest<Response<Order>>
    {
        public SingleByIdQuery(int id) => Id = id;
        public int Id { get; set; }
        public class SingleByIdQueryHandler : IRequestHandler<SingleByIdQuery, Response<Order>>
        {
            private readonly IApplicationDbContext _context;
            public SingleByIdQueryHandler(IApplicationDbContext context) => _context = context;
            public async Task<Response<Order>> Handle(SingleByIdQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var orders = await _context.Orders.Where(c => request.Id == c.Id).Include(x => x.Client).Include(x => x.Details).ThenInclude(x => x.Product).ToListAsync();
                    return Response<Order>.Success(orders, "Ok");
                }
                catch (Exception ex)
                {
                    return Response<Order>.Fail(StatusCode.InvalidArgument, ex.Message);
                }
            }
        }
    }
}
