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
    public class SearchByFilterQuery : IRequest<Response<Order>>
    {
        public SearchByFilterQuery(string filter) => Filter = filter;
        public string Filter { get; set; }
        public class SearchByFilterQueryHandler : IRequestHandler<SearchByFilterQuery, Response<Order>>
        {
            private readonly IApplicationDbContext _context;
            public SearchByFilterQueryHandler(IApplicationDbContext context) => _context = context;
            public async Task<Response<Order>> Handle(SearchByFilterQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var orders = await _context.Orders.Where(c => string.IsNullOrEmpty(request.Filter) || c.Client.FirstName.Contains(request.Filter.ToLower()) || c.Client.LastName.Contains(request.Filter.ToLower())).Include(x => x.Client).ToListAsync();
                    return Response<Order>.Success(orders, "Ok");
                }
                catch (Exception ex)
                {
                    return Response<Order>.Fail(StatusCode.InvalidArgument,ex.Message);
                }
            }
        }
    }
}
