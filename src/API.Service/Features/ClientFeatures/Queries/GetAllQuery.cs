using API.Domain.Entities;
using API.Domain.Shared;
using API.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace API.Service.Features.ClientFeatures.Queries
{
    public class GetAllQuery : IRequest<Response<Client>>
    {
        public class GetAllQueryHandler : IRequestHandler<GetAllQuery, Response<Client>>
        {
            private readonly IApplicationDbContext _context;
            public GetAllQueryHandler(IApplicationDbContext context) => _context = context;
            public async Task<Response<Client>> Handle(GetAllQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var clients = await _context.Clients.ToListAsync();
                    return Response<Client>.Success(clients.AsReadOnly(), "Ok");
                }
                catch (Exception ex)
                {
                    return Response<Client>.Fail(StatusCode.InvalidArgument, ex.Message);
                }
            }
        }
    }
}
