using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.GrainInterfaces;
using Microsoft.AspNetCore.Mvc;
using Orleans;

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

    }
}