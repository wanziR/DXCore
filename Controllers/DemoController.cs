using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CMS5.DataRespositories;
using CMS5.Models;
using CMS5.ViewModels;
using CMS5.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace CMS5.Controllers
{
    [Authorize]
    public class DemoController : Controller
    {
        private readonly IRepository<Student, int> _studentRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<DemoController> _logger;
        private readonly IConfiguration _configuration;


        public DemoController(
            IRepository<Student,int> studentRepository,
            IWebHostEnvironment webHostEnvironment,
            ILogger<DemoController> logger, 
            IConfiguration configuration
            )
        {
            _studentRepository = studentRepository;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _configuration = configuration;
          

        }

        #region 列表
      
        // //属性路由
        // [Route("")]
        // [Route("Demo")]
        // [Route("Demo/Index")]
        public IActionResult Index(string ReturnUrl)
        {
            List<Student> model = _studentRepository.GetAllList();
            return View(model);
        }

        #endregion

        #region 详情
        // public IActionResult Details(int id)
        // {
        //     var model = _studentRepository.GetStudent(id);
        //     return View(model);
        // }

        // [HttpGet]
        // public JsonResult Details()
        // {
        //     var model = _studentRepository.GetStudent(2);
        //     return Json(model);
        // }

        // //属性路由参数
        // [Route("Demo/Details/{id}")]
        // //属性路由可选参数
        // [Route("Demo/Details/{id?}")]
        public ViewResult Details(int id)
        {
            var student = _studentRepository.FirstOrDefault(a=>a.Id==id);
            //判断学生是否存在
            if (student == null)
            {
                ViewBag.ErrorMessage = $"学生Id={id}的信息不存在，请重试。";
                return View("NotFound");
            }
            DemoDetailsViewModel demoDetailsViewModel=new DemoDetailsViewModel
            {
                Student = student,
                PageTitle = "学生详情"
            };
            return View(demoDetailsViewModel);
        }

        #endregion

        #region 添加
        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(StudentCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                Student newStudent = new Student
                {
                    Name = model.Name,
                    Email = model.Email,
                    Major = model.Major,
                    PhotoPath = ProcessUploadedFile(model)

                };
                _studentRepository.Insert(newStudent);
                return RedirectToAction("Details", new { id = newStudent.Id });
            }
            return View();
        }

        #endregion

        #region 编辑
        [HttpGet]
        public ViewResult Edit(int id)
        {
            var student = _studentRepository.FirstOrDefault(a => a.Id == id);
            if (student == null)
            {
                ViewBag.ErrorMessage = $"学生Id={id}的信息不存在，请重试。";
                return View("NotFound");
            }
            StudentEditViewModel studentEditViewModel = new StudentEditViewModel
            {
                Id = id,
                Name = student.Name,
                Email = student.Email,
                Major = student.Major,
                ExistingPhotoPath = student.PhotoPath
            };
            return View(studentEditViewModel);
        }
        [HttpPost]
        public IActionResult Edit(StudentEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var student = _studentRepository.FirstOrDefault(a => a.Id == model.Id);
                student.Name = model.Name;
                student.Email = model.Email;
                student.Major = model.Major;

                #region 更新图片路径
                //如果用户想要更改图片，那么可以上传新图片文件，它会被模型对象上的Photo属性接收
                //如果用户没有上传图片，那么我们会保留现有的图片信息
                //因为兼容了多图片上传，所以将这里的！=null判断修改为判断Photo的总数是否大于0
                if (model.ExistingPhotoPath != null)
                {
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "avatars", model.ExistingPhotoPath);
                
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
                //我们将保存新的照片到 wwwroot/images/avatars  文件夹中，并且会更新
                //Student对象中的PhotoPath属性，然后最终都会将它们保存到数据库中
                student.PhotoPath = ProcessUploadedFile(model);
                #endregion

                Student updatedstudent = _studentRepository.Update(student);

                return RedirectToAction("Index");
            }
            return View();
        }

        #endregion

        #region 删除
        [HttpPost]
        public async  Task<IActionResult> Delete(int id)
        {
            var student = _studentRepository.FirstOrDefault(a=>a.Id==id);
            if (student ==null)
            {
                ViewBag.ErrorMessage = $"无法找到ID为{id}的用户";
                return View("NotFound");
            }
            else
            {
               await _studentRepository.DeleteAsync(a=>a.Id==id);
               return RedirectToAction("Index");
            }

        }

        #endregion

        #region 私有方法
        /// <summary>
        /// 将照片保存到指定的路径中，并返回唯一的文件名
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private string ProcessUploadedFile(StudentCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null)
            {
                string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "avatars");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);
                model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
            }

            return uniqueFileName;
        }

        #endregion

        #region 控制器中的操作方法
            //访问 http://localhost:5001/wc 或者 http://localhost:5001/wc/index

        [Route("Wc")]
        [Route("Wc/Index")]
        public string welcome()
        {
            return "我是控制器中的welcome()操作方法";

        }
        #endregion


    }
}
