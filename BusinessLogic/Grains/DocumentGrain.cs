using BusinessLogic.GrainInterfaces;
using DataModels.Exceptions;
using DataModels.Models;
using Orleans;
using RepositoryLayer.Abstraction;
using RepositoryLayer.Repository;
using System;
using System.Threading.Tasks;

namespace BusinessLogic.Grains
{
    public class DocumentGrain : Grain, IDocument, IState
    {
        private Document state { get; set; }

        public IRepository<Document, Guid> _repository;

        public override Task OnActivateAsync()
        {
            _repository = new DocumentRepository();
            //Initializes on grain activation
            state = new Document();

            state.DocumentId = this.GetPrimaryKey();
            ReadState();

            return base.OnActivateAsync();
        }
        /// <summary>
        /// Updates the state on deactivation
        /// </summary>
        /// <returns></returns>
        public override Task OnDeactivateAsync()
        {
            UpdateState();
            return base.OnDeactivateAsync();
        }

        public Task<Guid> GetAssociatedOrder()
        {
            throw new NotImplementedException();
        }

        public Task<Document> GetDocument()
        {
            return Task.FromResult(state);
        }

        public Task CreateDocument(Document document)
        {
            this.state = document;
            WriteState();
            return Task.CompletedTask;
        }

        public Task DeleteDocument(Document document)
        {
            DeleteState();
            state = new Document();
            //Just in case for now
            state.DocumentId = this.GetPrimaryKey();
            return Task.CompletedTask;
        }

        public Task UpdateDocument(Document document)
        {
            try
            {
                this.state = document;
                UpdateState();
            }
            catch(UpdateException ex)
            {
                throw ex;
            }

            return Task.CompletedTask;
        }

        public void ReadState()
        {
            var temp = _repository.Get(this.GetPrimaryKey());
            if (temp != null)
            {
                state = temp;
            }
        }

        public void UpdateState()
        {
            _repository.Update(state);
        }

        public void WriteState()
        {
            _repository.Add(state);
        }

        public void DeleteState()
        {
            _repository.Delete(state);
        }
    }
}
