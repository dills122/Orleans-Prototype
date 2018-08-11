using DataModels.Exceptions;
using BusinessLogic.GrainInterfaces;
using DataModels.Models;
using Microsoft.EntityFrameworkCore;
using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Grains
{
    public class OrderGrain : Grain, IOrder
    {
        public Order state { get; set; }

        public override Task OnActivateAsync()
        {
            //Initializes on grain activation
            state = new Order();

            state.OrderId = this.GetPrimaryKey();
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

        public Task CreateOrder(Order order)
        {
            WriteState();
            return Task.CompletedTask;
        }

        public Task UpdateOrder(Order order)
        {
            try
            {
                this.state = order;
                UpdateState();
            }
            catch (UpdateException ex)
            {
                throw ex;
            }
            return Task.CompletedTask;
        }

        public Task<Order> GetOrder()
        {
            return Task.FromResult(state);
        }

        public Task ProcessOrder()
        {
            throw new NotImplementedException();
        }

        public void ReadState()
        {
            //using(var context = new OrleansContext())
            //{
            //    var order = context.orders.Where(x => x.OrderId == state.OrderId).SingleOrDefault();
            //    if(order == null)
            //    {
            //        this.DeactivateOnIdle();
            //    }
            //    state = order;
            //}
        }
        public void DeleteState()
        {
            //using (var context = new OrleansContext())
            //{
            //    context.orders.Remove(state);
            //    context.SaveChanges();
            //    //Should deactive grain on deletion
            //    this.DeactivateOnIdle();
            //}
        }

        public void UpdateState()
        {
            //using (var context = new OrleansContext())
            //{
            //    context.orders.Update(state);
            //    try
            //    {
            //        context.SaveChanges();
            //    }
            //    catch (DbUpdateConcurrencyException ex)
            //    {
            //        var entry = ex.Entries.Single();
            //        var clientValues = (Order)entry.Entity;
            //        var databaseEntry = entry.GetDatabaseValues();
            //        //Make sure the row wasn't deleted
            //        if (databaseEntry != null)
            //        {
            //            var databaseValues = (Order)databaseEntry.ToObject();
            //            if (clientValues.orderType != databaseValues.orderType)
            //            {
            //                //Bubble up the exception to controller for proper handling
            //                throw new UpdateException("Order type has been changed",databaseValues);
            //            }
            //            //Update Row Version to allow update
            //            state.RowVersion = databaseValues.RowVersion;
            //            context.SaveChanges();
            //        }
            //    }
            //}
        }

        public void WriteState()
        {
            throw new NotImplementedException();
        }
    }
}
