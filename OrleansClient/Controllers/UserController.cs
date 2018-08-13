using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataModels.Models;
using Orleans;
using BusinessLogic.GrainInterfaces;
using DataModels.Exceptions;

namespace OrleansClient.Controllers
{
    public class UserController : Controller
    {
        private IClusterClient _client;

        public UserController( IClusterClient client)
        {
            _client = client;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            var grain = _client.GetGrain<IAllUsers>("All");
            var users = await grain.GetAllUsers();
            if (users == null)
            {
                var emptyList = new List<User>();
                return View(emptyList);
            }
            return View(users);
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var grain = _client.GetGrain<IUser>(id);
            var user = await grain.GetUser();
            if(user.CreatedDate == DateTime.MinValue)
            {
                return NotFound();
            }
            return View(user);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Username,FName,LName,CreatedDate,accountType,RowVersion")] User user)
        {
            if(user != null)
            {
                var grain = _client.GetGrain<IUser>(user.Username);
                if(grain.GetUser().Result.CreatedDate == DateTime.MinValue)
                {
                    await grain.CreateUser(user);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(user);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var grain = _client.GetGrain<IUser>(id);
            var user = await grain.GetUser();
            if(user.CreatedDate != DateTime.MinValue)
            {
                return View(user);
            }
            return NotFound();
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Username,FName,LName,CreatedDate,accountType,RowVersion")] User user)
        {
            if (id != user.Username)
            {
                return NotFound();
            }
            var grain = _client.GetGrain<IUser>(id);
            try
            {
                await grain.UpdateUser(user);
                return RedirectToAction(nameof(Index));
            }
            catch(UpdateException ex)
            {
                var databaseValues = (User)ex.databaseValues;

                if(databaseValues.FName != user.FName)
                {
                    ModelState.AddModelError("First Name", "Current value: " + databaseValues.FName);
                }
                if (databaseValues.LName != user.LName)
                {
                    ModelState.AddModelError("Last Name", "Current value: " + databaseValues.LName);
                }
                if (databaseValues.accountType != user.accountType)
                {
                    ModelState.AddModelError("Account Type", "Current value: " + databaseValues.accountType);
                }
                user.RowVersion = databaseValues.RowVersion;
            }
            return View(user);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var grain = _client.GetGrain<IUser>(id);
            var user = await grain.GetUser();
            if(user.CreatedDate != DateTime.MinValue)
            {
                return View(user);
            }
            return NotFound();
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var grain = _client.GetGrain<IUser>(id);
            var user = await grain.GetUser();
            if(user.CreatedDate != DateTime.MinValue)
            {
                await grain.DeleteUser();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
