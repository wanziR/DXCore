using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using CMS5.Models;
using CMS5.ViewModels;
using Microsoft.Extensions.Logging;

namespace CMS5.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Error()
        {
            //获取异常详情信息
            var exceptionHandlerPathFeature =
                                HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            //LogError()方法将异常记录作为日志中的错误类别记录
            _logger.LogError($"路径 {exceptionHandlerPathFeature.Path} " +
                $"产生了一个错误{exceptionHandlerPathFeature.Error}");
            return View("Error");
        }
    }
}
