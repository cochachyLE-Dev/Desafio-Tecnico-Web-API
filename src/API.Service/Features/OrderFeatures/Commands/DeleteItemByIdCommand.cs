using API.Domain.Shared;
using API.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace API.Service.Features.OrderFeatures.Commands
{
    public class DeleteItemByIdCommand : IRequest<Response>
    {
        public DeleteItemByIdCommand(int orderId, int productId) => (OrderId, ProductId) = (orderId, productId);
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public class DeleteItemByIdCommandHandler : IRequestHandler<DeleteItemByIdCommand, Response>
        {
            private readonly IApplicationDbContext _context;
            public DeleteItemByIdCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Response> Handle(DeleteItemByIdCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var detail = await _context.OrderDetails.SingleOrDefaultAsync(c => c.OrderId == request.OrderId && c.ProductId == request.ProductId);
                    if (detail == null) return Response.Fail(StatusCode.InvalidArgument, "order item not found");

                    var order = await _context.Orders.SingleOrDefaultAsync(c => c.Id == request.OrderId);
                    order.TotalPrice -= detail.Total;

                    _context.Orders.Update(order);
                    _context.OrderDetails.Remove(detail);

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
