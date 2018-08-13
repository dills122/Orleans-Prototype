﻿using DataModels.Models;
using Gridsum.DataflowEx;
using Microsoft.Win32.SafeHandles;
using PipelineService.Interfaces;
using PipelineService.Models;
using PipelineService.Processing;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace PipelineService.Pipelines
{
    public class OrderProcessingPipeline : Dataflow<OrderProcessing>, IPipeline<OrderProcessing>, IDisposable
    {
        public override ITargetBlock<OrderProcessing> InputBlock { get { return _inputBlock; } }

        private TransformBlock<OrderProcessing, OrderProcessing> _inputBlock;
        private ActionBlock<OrderProcessing> _resultsBlock;

        private List<OrderProcessing> _results { get; set; }

        bool disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        public OrderProcessingPipeline() : base(DataflowOptions.Default)
        {
            _results = new List<OrderProcessing>();

            _inputBlock = new TransformBlock<OrderProcessing, OrderProcessing>(obj =>
            {
                var success = OrderProcessor.ProcessOrder(obj);
                if(!success)
                {
                    EventCreation.CreateEvent(obj.order.OrderId, EventType.ErrorProcessing);
                } else
                {
                    EventCreation.CreateEvent(obj.order.OrderId, EventType.StartProcessing);
                }
                return obj;
            });
            _resultsBlock = new ActionBlock<OrderProcessing>(obj =>
            {
                var success = OrderProcessor.ProcessFiles(obj);
                if(!success)
                {
                    EventCreation.CreateEvent(obj.order.OrderId, EventType.ErrorProcessing);
                }
                else
                {
                    _results.Add(obj);
                    EventCreation.CreateEvent(obj.order.OrderId, EventType.FinishedProcessing);
                }
            });

            _inputBlock.LinkTo(_resultsBlock, new DataflowLinkOptions() { PropagateCompletion = true });

            RegisterChild(_inputBlock);
            RegisterChild(_resultsBlock);
        }

        public Task FillPipeline(OrderProcessing input)
        {
            InputBlock.Post(input);
            return Task.CompletedTask;
        }

        public Task<List<OrderProcessing>> GetResults()
        {
            return Task.FromResult(_results);
        }

        public async Task WaitForResults()
        {
            await this.CompletionTask;
        }

        public override void Complete()
        {
            InputBlock.Complete();
            base.Complete();
        }

        public async Task<List<OrderProcessing>> ProcessWaitForResults(List<OrderProcessing> inputs)
        {
            foreach (OrderProcessing def in inputs)
            {
                await FillPipeline(def);
            }
            Complete();
            await WaitForResults();
            var results = await GetResults();
            return results;
        }

        public void ProcessAndForget(List<OrderProcessing> ts)
        {
            //Fire and forget
            Task.Factory.StartNew(() => ProcessWaitForResults(ts));
        }

        /// <summary>
        /// Right now should only be used on WaitForResults
        /// Might dispose of pipeline before finished if not
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            if (disposing)
            {
                handle.Dispose();
            }
            disposed = true;
        }
    }
}
