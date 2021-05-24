using OrderSolution.Core.Commands;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrderSupervisor.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderSupervisor.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IHandler<CreateOrderCommand> commandHandler;

        public OrderController(ILogger<OrderController> logger, IHandler<CreateOrderCommand> commandHandler)
        {
            _logger = logger;
            this.commandHandler = commandHandler;
        }

        [HttpPost]
        public async Task<object> Post([FromBody] CreateOrderCommand createOrderCommand)
        {
            var result = await this.commandHandler.Handle(createOrderCommand);
            return Ok(result.Data);
        }
    }
}
