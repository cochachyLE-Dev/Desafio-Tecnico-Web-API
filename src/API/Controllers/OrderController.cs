using API.Domain.Entities;
using API.Service.Features.OrderFeatures.Commands;
using API.Service.Features.OrderFeatures.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]    
    public class OrderController : ControllerBase
    {
        private IMediator _mediator;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IMediator mediator, ILogger<OrderController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        [HttpPost("Add")]
        public async Task<IActionResult> AddAsync([FromBody] Order request)
        {
            _logger.LogInformation("Use memoryCache at: {time}", DateTimeOffset.Now);

            return Ok(await _mediator.Send(new MemoryCacheCommand
            {
                Id = request.Id,
                ClientId = request.ClientId,
                TotalPrice = request.TotalPrice,
                Details = request.Details?.ToList()
            }));
        }
        [HttpPost("Insert")]
        public async Task<IActionResult> InsertAsync([FromBody] Order request)
        {
            _logger.LogInformation("Save order at: {time}", DateTimeOffset.Now);

            return Ok(await _mediator.Send(new CreateCommand { 
                Id = request.Id,
                ClientId = request.ClientId,
                TotalPrice = request.TotalPrice,
                Details = request.Details?.ToList()
            }));
        }
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            return Ok(await _mediator.Send(new DeleteByIdCommand(id)));
        }
        [HttpPost("DeleteItem/{orderId}/{productId}")]
        public async Task<IActionResult> DeleteItemAsync(int orderId, int productId)
        {
            return Ok(await _mediator.Send(new DeleteItemByIdCommand(orderId, productId)));
        }
        [HttpGet("Search")]
        public async Task<IActionResult> SearchAsync(string filter)
        {
            return Ok(await _mediator.Send(new SearchByFilterQuery(filter)));
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _mediator.Send(new SearchByFilterQuery(null)));
        }
        [HttpGet("Single/{id}")]
        public async Task<IActionResult> SingleByIdAsync(int id)
        {
            return Ok(await _mediator.Send(new SingleByIdQuery(id)));
        }
    }
}
