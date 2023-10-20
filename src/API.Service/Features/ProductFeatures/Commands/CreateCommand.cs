using API.Domain.Entities;
using API.Domain.Shared;
using API.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Service.Features.ProductFeatures.Commands
{
    public class CreateCommand : IRequest<Response<Product>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; } = 1;
        public double Price { get; set; } = 0.00;        
        public string StatusName { get; set; }

        public List<OrderDetail> Details { get; set; }

        public class CreateCommandHandler : IRequestHandler<CreateCommand, Response<Product>>
        {
            private readonly IApplicationDbContext _context;
            public CreateCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Response<Product>> Handle(CreateCommand request, CancellationToken cancellationToken)
            {                
                try
                {
                    int index = await _context.Products.CountAsync();

                    var product = new Product();
                    //product.Id = ++index;
                    product.Name = request.Name;
                    product.Description = request.Description;
                    product.Stock = request.Stock;
                    product.Price = request.Price;

                    var add = _context.Products.Add(product);
                    await _context.SaveChangesAsync();                                       

                    return Response<Product>.Success(add.Entity, "Successful process");                    
                }
                catch (Exception ex)
                {
                    return Response<Product>.Fail(StatusCode.InvalidArgument, ex.Message);
                }                
            }
        }
    }
}
