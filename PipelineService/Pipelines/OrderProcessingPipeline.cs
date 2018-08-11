using Gridsum.DataflowEx;
using PipelineService.Interfaces;
using PipelineService.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace PipelineService.Pipelines
{
    public class OrderProcessingPipeline : Dataflow<OrderProcessing>, IPipeline<OrderProcessing>
    {
        public override ITargetBlock<OrderProcessing> InputBlock { get { return _inputBlock; } }

        private TransformBlock<OrderProcessing, OrderProcessing> _inputBlock;
        private ActionBlock<OrderProcessing> _resultsBlock;

        private List<OrderProcessing> _results { get; set; }

        public OrderProcessingPipeline() : base(DataflowOptions.Default)
        {
            _results = new List<OrderProcessing>();

            _inputBlock = new TransformBlock<OrderProcessing, OrderProcessing>(obj => obj);
            _resultsBlock = new ActionBlock<OrderProcessing>(obj => _results.Add(obj));

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
    }
}
