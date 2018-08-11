using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using BusinessLogic.GrainInterfaces;
using DataModels.Models;
using DataModels.Exceptions;

namespace OrleansClient.Controllers
{
    public class OrderController : Controller
    {
        private IClusterClient _client;

        public OrderController(IClusterClient client)
        {
            _client = client;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var grain = _client.GetGrain<IAllOrders>("All");
            var orders = grain.GetAllOrders().Result;
            if(orders == null)
            {
                var emptyList = new List<Order>();
                return View(emptyList);
            }
            return View(orders);
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            if(id == 0)
            {
                return NotFound();
            }
            var grain = _client.GetGrain<IOrder>(id);
            var order = grain.GetOrder().Result;
            if(order.CreatedDate == DateTime.MinValue)
            {
                return NotFound();
            }
            return View(order);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId, OrderDescription, RowVersion, CreatedDate, orderType, Username")] Order order)
        {
            if(order != null)
            {
                var grain = _client.GetGrain<IOrder>(order.OrderId);
                if(grain.GetOrder().Result.CreatedDate == DateTime.MinValue)
                {
                    await grain.CreateOrder(order);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(order);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var grain = _client.GetGrain<IOrder>(id);
            var order = grain.GetOrder().Result;
            if(order.CreatedDate != DateTime.MinValue)
            {
                return View(order);
            }
            return NotFound();
        }

        public async Task<IActionResult> Edit(int id, [Bind("OrderId, OrderDescription, RowVersion, CreatedDate, orderType, Username")] Order order)
        {
            if(id != order.OrderId)
            {
                return NotFound();
            }
            var grain = _client.GetGrain<IOrder>(id);
            try
            {

            }
            catch(UpdateException ex)
            {
                var dbValues = (Order)ex.databaseValues;
                //TODO Optimistic concurrency
            }
            return View(order);
        }

        public async Task<IActionResult> Delete(int id)
        {
            if(id == 0)
            {
                return NotFound();
            }
            var grain = _client.GetGrain<IOrder>(id);
            var order = grain.GetOrder().Result;
            if(order.CreatedDate != DateTime.MinValue)
            {
                return View(order);
            }
            return NotFound();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if(id == 0)
            {
                return NotFound();
            }
            var grain = _client.GetGrain<IOrder>(id);
            var order = grain.GetOrder().Result;
            if(order.CreatedDate != DateTime.MinValue)
            {
                //TODO create delete method in order grain
            }
            return RedirectToAction(nameof(Index));
        }
    }
}