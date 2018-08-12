using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IActionResult> Index()
        {
            //Static orderID for testing
            Guid guid = Guid.Parse("fc0dc48c-eb6d-45b6-a458-fc24e0c6f340");
            var grain = _client.GetGrain<IOrderEvents>(guid);
            var events = await grain.GetOrdersEvents();
            ViewBag.Events = events;
            return View();
        }

        public async Task<IActionResult> Search(string id)
        {
            List<OrderSearchViewModel> orderSearchViewModels = new List<OrderSearchViewModel>();
            List<Order> orders = new List<Order>();
            //EWW
            List<List<Event>> events = new List<List<Event>>();
            var userGrain = _client.GetGrain<IUser>(id);
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
            return View();
        }
    }
}