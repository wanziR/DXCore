using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CMS5.Models.EnumTypes;
using Microsoft.AspNetCore.Http;

namespace CMS5.ViewModels
{
    public class StudentCreateViewModel
    {
        [Required(ErrorMessage = "请输入名字！"), MaxLength(50, ErrorMessage = "名字长度不能超过50个字符")]
        [Display(Name = "名字")]
        public string Name { get; set; }


        [Required(ErrorMessage = "请选择一门科目")]
        [Display(Name = "主修科目")]
        public MajorEnum Major { get; set; }

        [Display(Name = "电子邮箱")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
            ErrorMessage = "邮箱的格式不正确")]
        [Required(ErrorMessage = "请输入邮箱地址，它不能为空")]
        public string Email { get; set; }



        [Display(Name = "头像")]
        public IFormFile Photo { get; set; }

        /// <summary>
        /// 入学时间
        /// </summary>
        [Display(Name = "入学时间")]
        public DateTime EnrollmentDate { get; set; }
    }
}
