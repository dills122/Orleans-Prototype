using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessLogic;
using DataModels.Models;
using Orleans;
using BusinessLogic.GrainInterfaces;
using Microsoft.EntityFrameworkCore.Storage;
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
            //var grain = _client.GetGrain<IAllUsers>("All");
            //var users = grain.GetAllUsers().Result;
            //if(users == null)
            //{
            //    var emptyList = new List<User>();
            //    return View(emptyList);
            //}
            return View(new List<User>());
        }

        // GET: User/Details/5
        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var grain = _client.GetGrain<IUser>(id);
            var user = grain.GetUser().Result;
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
            var user = grain.GetUser().Result;
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
            //Throwing error from grain is not getting caught in controller
            try
            {
                //Since error is not bubbling up the await isn't returning
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
                //var clientValues = (User)ex.Entries.Single().Entity;
                //var databaseEntry = ex.Entries.Single().GetDatabaseValues();
                //if(databaseEntry == null)
                //{
                //    ModelState.AddModelError(string.Empty, "Unable to save changes. The user was deleted by another admin.");
                //}
                //else
                //{
                //    var databaseValues = (User)databaseEntry.ToObject();
                //    if(databaseValues.accountType != clientValues.accountType)
                //    {
                //        ModelState.AddModelError("Account Type", "Current value: " + databaseValues.accountType);
                //    }
                //    user.RowVersion = databaseValues.RowVersion;
                //}
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
            var user = grain.GetUser().Result;
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
            var user = grain.GetUser().Result;
            if(user.CreatedDate != DateTime.MinValue)
            {
                await grain.DeleteUser();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            var grain = _client.GetGrain<IUser>(id);
            var user = grain.GetUser().Result;
            if(user.CreatedDate != DateTime.MinValue)
            {
                return true;
            }
            return false;
        }
    }
}
