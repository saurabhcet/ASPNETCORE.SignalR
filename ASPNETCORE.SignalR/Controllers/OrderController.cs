using ASPNETCORE.SignalR.Hubs;
using ASPNETCORE.SignalR.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCORE.SignalR.Controllers
{
    [Route("[controller]")]
    public class OrderController : Controller
    {
        private readonly IHubContext<OrderHub> _orderHub;

        public OrderController(IHubContext<OrderHub> orderHub)
        {
            _orderHub = orderHub;
        }

        [HttpGet]
        public string Get()
        {
            return "Its working";
        }

        [HttpPost]
        public async Task<IActionResult> Order([FromBody] Order order)
        {
            await _orderHub.Clients.All.SendAsync("NewOrder", order);
            return Accepted(1); //return order id
        }
    }
}
