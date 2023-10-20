using API.Domain.Entities;
using API.Domain.Shared;
using API.Persistence;
using API.Service.Implementation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Service.Features.ProductFeatures.Commands
{
    public class UpdateStatusCommand : IRequest<Response>
    {
        public int Id { get; set; }
        public ProductStatusType StatusType { get; set; }
        public class UpdateStatusCommandHandler : IRequestHandler<UpdateStatusCommand, Response>
        {
            private readonly IApplicationDbContext _context;
            private readonly MemoryCacheService _statusCacheService;
            public UpdateStatusCommandHandler(IApplicationDbContext context, MemoryCacheService statusCache) => (_context, _statusCacheService) = (context, statusCache);
            public async Task<Response> Handle(UpdateStatusCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var product = await _context.Products.Where(c => c.Id == request.Id).FirstOrDefaultAsync();

                    if (product != null)
                    { 
                        _statusCacheService.GetOrCreateProductStatus(product.Id, request.StatusType);
                    }
                    
                    return Response.Success("Ok");
                }
                catch (Exception ex)
                {
                    return Response.Fail(StatusCode.InvalidArgument, ex.Message);
                }
            }
        }
    }
}
