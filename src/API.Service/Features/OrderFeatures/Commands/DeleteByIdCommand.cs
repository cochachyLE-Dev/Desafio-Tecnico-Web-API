using API.Domain.Shared;
using API.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace API.Service.Features.OrderFeatures.Commands
{
    public class DeleteByIdCommand : IRequest<Response>
    {
        public DeleteByIdCommand(int id) => Id = id;
        public int Id { get; set; }
        public class DeleteByIdCommandHandler : IRequestHandler<DeleteByIdCommand, Response>
        {
            private readonly IApplicationDbContext _context;
            public DeleteByIdCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Response> Handle(DeleteByIdCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var order = await _context.Orders.FirstOrDefaultAsync();
                    if (order == null) return Response.Fail(StatusCode.InvalidArgument, "order not found");
                    _context.Orders.Remove(order);

                    await _context.SaveChangesAsync();
                    return Response.Success("Successful");
                }
                catch (Exception ex)
                {
                    return Response.Fail(StatusCode.InvalidArgument, ex.Message);
                }
            }
        }
    }
}
