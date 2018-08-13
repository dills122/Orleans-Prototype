using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using BusinessLogic.GrainInterfaces;
using DataModels.Models;
using DataModels.Exceptions;
using Microsoft.AspNetCore.Http;
using PipelineService.Interfaces;
using System.IO;
using PipelineService.Models;
using PipelineService.Pipelines;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OrleansClient.Controllers
{
    public class OrderController : Controller
    {
        private IClusterClient _client;
        private IPipelineAlloc<OrderProcessingPipeline> _pipelineAlloc;
        private IPipeline<OrderProcessing> _pipeline;

        public OrderController(IClusterClient client, IPipelineAlloc<OrderProcessingPipeline> pipelineAlloc)
        {
            _pipelineAlloc = pipelineAlloc;
            _client = client;
            _pipeline = _pipelineAlloc.RetrievePipeline().Result;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var grain = _client.GetGrain<IAllOrders>("All");
            var orders = await grain.GetAllOrders();
            if (orders == null)
            {
                var emptyList = new List<Order>();
                return View(emptyList);
            }
            return View(orders);
        }
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var grain = _client.GetGrain<IOrder>(id);
            var order = await grain.GetOrder();
            if (order.CreatedDate == DateTime.MinValue)
            {
                return NotFound();
            }
            return View(order);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var grain = _client.GetGrain<IAllUsers>("All");
            var users = await grain.GetAllUsers();
            var selectlist = users.OrderBy(u => u.Username).Select(x => new { Id = x.Username, Value = x.Username });

            ViewBag.Users = new SelectList(selectlist, "Id","Value");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order, List<IFormFile> files)
        {
            order.CreatedDate = DateTime.Now;
            order.OrderId = Guid.NewGuid();
            //Static for now since not session state/auth set up for knowing
            OrderProcessing orderProcessing = new OrderProcessing();
            orderProcessing.order = order;
            
            foreach (IFormFile file in files)
            {
                using (var memoryStream = new MemoryStream())
                {
                    file.OpenReadStream().CopyTo(memoryStream);
                    var FilesBytes = memoryStream.ToArray();
                    if (FilesBytes != null)
                    {
                        InputFile inputFile = new InputFile
                        {
                            documentType = DocumentType.Other,
                            FileExtension = file.ContentType,
                            FileName = file.FileName,
                            FilesBytes = FilesBytes
                        };
                        orderProcessing.files.Add(inputFile);
                    }
                }
            }

            List<OrderProcessing> orderProcessings = new List<OrderProcessing>();
            orderProcessings.Add(orderProcessing);

            _pipeline = await _pipelineAlloc.RetrievePipeline();
            //var test = await _pipeline.ProcessWaitForResults(orderProcessings);
            _pipeline.ProcessAndForget(orderProcessings);

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var grain = _client.GetGrain<IOrder>(id);
            var order = await grain.GetOrder();
            if (order.CreatedDate != DateTime.MinValue)
            {
                return View(order);
            }
            return NotFound();
        }

        public async Task<IActionResult> Edit(Guid id, [Bind("OrderId, OrderDescription, RowVersion, CreatedDate, orderType, Username")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }
            var grain = _client.GetGrain<IOrder>(id);
            try
            {
                await grain.UpdateOrder(order);
            }
            catch (UpdateException ex)
            {
                var dbValues = (Order)ex.databaseValues;
                if(dbValues.orderType != order.orderType)
                {
                    ModelState.AddModelError("Order Type", "Current value: " + dbValues.orderType);
                }
                if(dbValues.OrderDescription != order.OrderDescription)
                {
                    ModelState.AddModelError("Order Description", "Current value: " + dbValues.OrderDescription);
                }
                order.RowVersion = dbValues.RowVersion;
            }
            return View(order);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var grain = _client.GetGrain<IOrder>(id);
            var order = await grain.GetOrder();
            if (order.CreatedDate != DateTime.MinValue)
            {
                return View(order);
            }
            return NotFound();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var grain = _client.GetGrain<IOrder>(id);
            var order = await grain.GetOrder();
            if (order.CreatedDate != DateTime.MinValue)
            {
                await grain.RemoveOrder();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}