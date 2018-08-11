﻿using DataModels.Models;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.GrainInterfaces
{
    public interface IAllOrders : IGrainWithStringKey, IState
    {
        Task<List<Order>> GetAllOrders();
    }
}