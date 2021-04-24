using ASPNETCORE.SignalR.Helpers;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;

namespace ASPNETCORE.SignalR.Hubs
{
    public class OrderHub : Hub
    {
        private readonly OrderChecker _orderChecker;

        public OrderHub(OrderChecker orderChecker)
        {
            _orderChecker = orderChecker;
        }

        public async Task OrderUpdateRequest(int orderId)
        {
            CheckResult result;
            do
            {
                result = _orderChecker.GetUpdate(orderId);
                Thread.Sleep(1000);
                if (result.New)
                    await Clients.All.SendAsync("OrderUpdateResponse", result.Update);

            } while (!result.Finished);
            await Clients.Caller.SendAsync("Finished");
        }
    }
}
