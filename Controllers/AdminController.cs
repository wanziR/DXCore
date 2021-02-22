using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CMS5.Models;
using CMS5.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace CMS5.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        #region 注入

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ErrorController> _logger;

        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ILogger<ErrorController> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
        }
        #endregion


        #region 角色创建
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                //指定一个不重复的角色名来创建新角色 
                IdentityRole IdentityRole = new IdentityRole
                {
                    Name = model.RoleName
                };
                //将角色保存互ApsNetRoles表中
                IdentityResult result = await _roleManager.CreateAsync(IdentityRole);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Admin");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        #endregion

        #region 角色列表
        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        #endregion

        #region 角色编辑
        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"角色Id={id}的信息不存在,请重试.";
                return View("NotFound");
            }
            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };

            var users = _userManager.Users.ToList();
            //查询所有用户
            foreach (var user in users)
            {
                //如果用户拥有此角色,将用户添加到EditRoleViewModel中Users属性中
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"角色Id={model.Id}的信息不存在,请重试.";
                return View("NotFound");
            }
            else
            {
                role.Name = model.RoleName;
                //使用UpdateAsync()更新角色
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        #endregion

        #region 角色删除
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"无法找到ID为{id}的角色。";
                return View("NotFound");
            }
            else
            {
                //将代码包装在try catch中
                try
                {
                    var result = await _roleManager.DeleteAsync(role);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListRoles");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View("ListRoles");

                }
                //如果触发的异常是DbUpdateException，则知道我们无法删除角色
                //因为该角色中已存在用户信息
                catch (DbUpdateException ex)
                {
                    //将异常记录到日志文件中。我们之前已经学习使用NLog配置日志信息
                    _logger.LogError($"发生异常 :{ex}");
                    //我们使用ViewBag.ErrorTitle和ViewBag.ErrorMessage来传递
                    //错误标题和详情信息到错误视图
                    //错误视图会将这些数据显示给用户
                    ViewBag.ErrorTitle = $"角色:{role.Name} 正在被使用中...";
                    ViewBag.ErrorMessage = $" 无法删除{role.Name}角色，因为此角色中已经存在用户。如果想删除此角色，需要先从该角色中删除用户，然后尝试删除该角色本身。";
                    return View("Error");
                }
            }

        }
        #endregion

        #region 添加或删除用户到角色中
        [HttpGet]
        public async Task<IActionResult> EditUserSInRole(string roleId)
        {
            ViewBag.roleId = roleId;
            //通过rolrId查询角色实体信息
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"角色Id={roleId}的信息不存在，请重试。";
                return View("NotFound");
            }
            var model = new List<UserRoleViewModel>();
            var users = _userManager.Users.ToList();
            foreach (var user in users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                //判断当前用户是否已经存在于角色中
                var isInRole = await _userManager.IsInRoleAsync(user, role.Name);

                if (isInRole)
                {//存在则设置为选中状态,值为true
                    userRoleViewModel.IsSelected = true;

                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }
                model.Add(userRoleViewModel);
            }


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"角色Id={roleId}的信息不存在，请重试。";
                return View("NotFound");
            }
            for (int i = 0; i < model.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(model[i].UserId);
                IdentityResult result = null;
                //检查当前用户是否被选中,如果选中则添加到角色中
                if (model[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }//如果没有选中则从userroles表中移除
                else if (!model[i].IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                { //对于其他情况不做处理，继续新的循环
                    continue;
                }

                if (result.Succeeded)
                {//判断当前用户是否为最后一个用户,如果是,则跳转到EditRole视图
                 //不是则进入下一循环
                    if (i < model.Count - 1)
                        continue;
                    else
                        return RedirectToAction("EditRole", new { Id = roleId });

                }

            }
            return RedirectToAction("EditRole", new { Id = roleId });
        }
        #endregion


        #region 用户管理
        public IActionResult ListUsers()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }
        #endregion

        #region 用户编辑
        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            //根据ID取各用户
            var user = await _userManager.FindByIdAsync(id);
            //非空判断
            if (user == null)
            {
                ViewBag.ErrorMessage = $"用户Id={id}的信息不存在，请重试。";
                return View("NotFound");
            }
            // GetClaimsAsync()返回用户声明列表
            var userClaims = await _userManager.GetClaimsAsync(user);
            // GetRolesAsync()返回用户角色列表
            var userRoles = await _userManager.GetRolesAsync(user);
            var model = new EditUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                City = user.City,
                Claims = userClaims.Select(c => c.Value).ToList(),
                Roles = userRoles
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"无法找到ID为{model.Id}的用户。";
                return View("NotFound");
            }
            else
            {
                user.Email = model.Email;
                user.City = model.City;
                user.UserName = model.UserName;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }

        }
        #endregion

        #region 删除用户
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"无法找到ID为{id}的用户。";
                return View("NotFound");
            }
            else
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("ListUsers");


            }



        }
        #endregion

        #region 管理用户角色
        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            ViewBag.userId = userId;
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"无法找到ID为{userId}的用户。";
                return View("NotFound");
            }
            var model = new List<RolesInUserViewModel>();
            foreach (var role in _roleManager.Roles)
            {
                var roleInUserViewModel = new RolesInUserViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                //判断当前用户是否拥有该角色
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    //将已拥有的角色信息设置为选中
                    roleInUserViewModel.IsSelected = true;
                }
                else
                {
                    roleInUserViewModel.IsSelected = false;
                }
                model.Add(roleInUserViewModel);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(List<RolesInUserViewModel>  model,string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"无法找到ID为{userId}的用户";
                return View("NotFound");
            }
            var roles =await _userManager.GetRolesAsync(user);
            //移除当前用户中的所有角色信息
            var result =await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("","无法删除用户中的现有角色");
                return View(model);
            }
            //查询模型列表中被选中的RoleName并添加到用户中
            result = await _userManager.AddToRolesAsync(user,
                model.Where(x => x.IsSelected).Select(y => y.RoleName));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("","无法向用户添加选定的角色");
                return View(model);
            }

            return RedirectToAction("EditUser",new { Id = userId });
        }

        #endregion

        #region 登录后未授权用户访问重定向
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
        #endregion
    }
}
