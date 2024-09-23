using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Interfaces;
using MyApp.Domain.Models;
using MyApp.Domain.ViewModels;

namespace MyApp.Mvc.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        // GET: UserController
        public async Task<ActionResult> Index(FilterUserViewModel filter)
        {
            var result = await _userService.FilterAsync(filter);
            return View(result); 
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            User user = _userService.GetUserById(id);
            if(user==null)
            {
                return NotFound();
            }
            return View(user);
        }

        public IActionResult Create()
        {
            var viewModel = new CreateUserViewModel(); 
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(CreateUserViewModel viewModel) 
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    Age = viewModel.Age
                };

                _userService.CreateUser(user); 
                return RedirectToAction("Index"); 
            }

            return View(viewModel); 
        }


        public IActionResult Edit(int id)
        {
            var user = _userService.GetUserById(id);

          
            if (user == null)
            {
                return NotFound(); 
            }

            var viewModel = new EditUserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Age = user.Age
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(EditUserViewModel viewModel) 
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Id = viewModel.Id, 
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    Age = viewModel.Age
                };

                _userService.UpdateUser(user);
                return RedirectToAction("Index");
            }

            return View(viewModel); 
        }



        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            User user = _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            bool result = _userService.DeleteUser(id);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = "Unable to delete user.";
            return RedirectToAction(nameof(Index));
        }




    }
}
