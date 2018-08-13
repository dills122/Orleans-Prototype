using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLogic.GrainInterfaces;
using DataModels.Models;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using OrleansClient.ViewModels;

namespace OrleansClient.Controllers
{
    public class EventController : Controller
    {
        private IClusterClient _client;

        public EventController(IClusterClient client)
        {
            _client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Search(User id)
        {
            List<OrderSearchViewModel> orderSearchViewModels = new List<OrderSearchViewModel>();

            var userGrain = _client.GetGrain<IUser>(id.Username);
            var orderIds = await userGrain.GetOrders();
            foreach(Guid orderID in orderIds)
            {
                var orderGrain = _client.GetGrain<IOrder>(orderID);
                var order = await orderGrain.GetOrder();

                var OrderEventsGrain = _client.GetGrain<IOrderEvents>(orderID);
                var orderEvents = await OrderEventsGrain.GetOrdersEvents();

                OrderSearchViewModel orderSearchViewModel = new OrderSearchViewModel(order, (List<Event>)orderEvents);
                orderSearchViewModels.Add(orderSearchViewModel);
            }
            ViewBag.Orders = orderSearchViewModels;
            ViewBag.Username = id.Username;
            return View();
        }
    }
}