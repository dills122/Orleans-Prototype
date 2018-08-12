using DataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrleansClient.ViewModels
{
    public class OrderSearchViewModel
    {
        public Order order { get; set; }
        public IEnumerable<Event> events { get; set; }

        public OrderSearchViewModel()
        {
            events = new List<Event>();
        }
        public OrderSearchViewModel(Order order, List<Event> events)
        {
            this.order = order;
            this.events = events;
        }
    }
}
