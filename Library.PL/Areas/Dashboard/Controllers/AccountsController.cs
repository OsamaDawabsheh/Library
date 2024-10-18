﻿using AutoMapper;
using Library.DAL.Data;
using Library.DAL.Models;
using Library.PL.Areas.Dashboard.ViewModels;
using Library.PL.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using System.Data;

namespace Library.PL.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    //[Authorize(Roles = "Admin")]
    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountsController(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager ,RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = userManager.Users.ToList();

            var userVMs = new List<UserVM>();

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);

                var userVM = mapper.Map<UserVM>(user);
                userVM.Roles = roles;

                userVMs.Add(userVM);
            }

                return View(userVMs);
        }

        public IActionResult Register()
        {

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(CreateUserVM model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.Img = FilesSettings.UploadFile(model.Image, "users");


            var user = mapper.Map<ApplicationUser>(model);

            var result = await userManager.CreateAsync(user,model.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user,"user");
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            var user = context.Users.Find(id);
            if (user is null)
            {
                return NotFound();
            }

            return View(mapper.Map<UserVM>(user));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UserVM model)
        {
            if (model.Image is null)
            {
                ModelState.Remove("image");
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                FilesSettings.DeleteFile(model.Img, "users");
                model.Img = FilesSettings.UploadFile(model.Image, "users");
            }


            var user = context.Users.Find(model.Id);

            if (user is null)
            {
                return NotFound();
            }

            mapper.Map(model, user);

            context.SaveChanges();

            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            var user = context.Users.Find(id);

            if (user is null)
            {
                return NotFound();
            }

            return View(mapper.Map<UserVM>(user));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(UserVM model)
        {
            var user =  context.Users.Find(model.Id);

            if (user is null)
            {
                return RedirectToAction(nameof(Index));
            }

            FilesSettings.DeleteFile(user.Img, "users");
            await userManager.DeleteAsync(user);

            context.SaveChanges();

            return RedirectToAction(nameof(Index));

        }

        [HttpGet]

        public IActionResult Details(string id)
        {

            var user = context.Users.Find(id);

            return View(mapper.Map<UserVM>(user));
        }


        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(RoleVM model)
        {

            var role = new IdentityRole
            {
                Name = model.Name
            };

            var result = await roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Roles));
            }
            return View(model);
        }

        public IActionResult Roles()
        {

            var roles = roleManager.Roles.ToList();

            return View(mapper.Map<List<RoleVM>>(roles));
        }

        [HttpGet]
        public IActionResult EditRole(string id)
        {
            var role = context.Roles.Find(id);

            if (!ModelState.IsValid)
            {
                return View(role);
            }
            if (role is null)
            {
                return NotFound();
            }
            return View(mapper.Map<RoleVM>(role));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditRole(RoleVM model)
        {
            
                if (!ModelState.IsValid)
                {
                    return View(model);
                }


            var role = context.Roles.Find(model.Id);

            if (role is null)
            {
                return NotFound();
            }

            mapper.Map(model, role);

            context.SaveChanges();

            return RedirectToAction(nameof(Roles));

        }

        [HttpGet]
        public IActionResult DeleteRole(string id)
        {
            var role = context.Roles.Find(id);

            if (!ModelState.IsValid)
            {
                return View(role);
            }

            if (role is null)
            {
                return NotFound();
            }

            return View(mapper.Map<RoleVM>(role));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRole(RoleVM model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);

            if (!ModelState.IsValid)
            {
                return View(role);
            }

            if (role is null)
            {
                return RedirectToAction(nameof(Roles));
            }

            var result = await roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Roles));
            }


            return RedirectToAction(nameof(Roles));

        }

        [HttpGet]
        public IActionResult ChangeUserRole(string id)
        {
            var user = context.Users.Find(id);


            if (user is null)
            {
                return NotFound();
            }

            var changeRoleVM = new ChangeRoleVM
            {
                SelectedRole = userManager.GetRolesAsync(user).Result.ToString(),
                selectLists = roleManager.Roles.Select(role => new SelectListItem
                {
                    Value = role.Id,
                    Text = role.Name
                }).ToList()
            };
           
            return View(changeRoleVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUserRole(ChangeRoleVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByIdAsync(model.Id);

            var currentRoles = await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(user, currentRoles);

            var role = await roleManager.FindByIdAsync(model.SelectedRole);
            await userManager.AddToRoleAsync(user , role.Name);


            return RedirectToAction(nameof(Index));
        }

    }

}