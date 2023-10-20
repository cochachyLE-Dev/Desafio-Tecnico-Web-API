using API.Domain.Entities;
using API.Service.Features.ProductFeatures.Commands;
using API.Service.Features.ProductFeatures.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private IMediator _mediator;
        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _mediator.Send(new GetAllQuery()));
        }
        [HttpPost("Insert")]
        public async Task<IActionResult> InsertAsync([FromBody] Product request)
        {
            return Ok(await _mediator.Send(new CreateCommand
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
                Stock = request.Stock,
                Price = request.Price,
                StatusName = request.StatusName
            }));
        }
    }
}
