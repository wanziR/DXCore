using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS5.Infrastructure.Repositories;
using CMS5.Models;
using CMS5.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using BCrypt.Net;

namespace CMS5.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IRepository<ApplicationUser, int> _userRepository;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,IRepository<ApplicationUser, int> userRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userRepository = userRepository;
        }

        #region 注册

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Phone,
                    PhoneNumber = model.Phone,
                    userNickname = model.userNickname,
                    userWorkPhone = model.userWorkPhone,
                    userAddress=model.userAddress,
                    userAddtime=DateTime.Now
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                //如果成功创建用户，则使用登录服务登录用户信息
                //并重定向到HomeController的Index操作方法中
                if (result.Succeeded)
                {
                    //如果用户已登录且为Admin角色
                    //那么就是Admin正在创建新用户
                    //所以重定向Admin用户到ListUsers的视图列表
                    if (_signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("ListUsers","Admin");
                    }
                    //否则就是登录当前注册用户并重定向到HomeController的
                    //Index()操作方法中
                    await _signInManager.SignInAsync(user,isPersistent: false);
                    return RedirectToAction("index","Demo");

                }
                //如果有任何错误，将它们添加到ModelState对象中
                //将由验证摘要标记助手显示到视图中
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }


        #endregion

        #region 注销
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Demo");
        }

        #endregion

        #region 登录

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {

            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByNameAsync(model.Phone);
                var result = await _userManager.CheckPasswordAsync(user, model.Password);

                if (result == true)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", "Demo");
                    }

                }
                ModelState.AddModelError(String.Empty, "登录失败，请重试");
            }

            return View(model);

        }

        #endregion

        #region 远程验证手机号是否被注册

        public async Task<IActionResult> IsPhoneInUser(string Phone)
        {
            var user = await _userManager.FindByNameAsync(Phone);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"手机号:{Phone}已经被注册使用了。");
            }
        }

        #endregion

       

    }
}
