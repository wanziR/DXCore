using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CMS5.CustomerMiddlewares.Utils;
using Microsoft.AspNetCore.Mvc;

namespace CMS5.ViewModels
{
    public class RegisterViewModel
    {

        //// [Required]
        //// [EmailAddress]
        //[Display(Name = "邮箱")]

        ////远程验证属性返回Json
        // //[Remote(action: "IsEmailInUse", controller: "Account")]

        ////自定义验证属性P316
        ////[ValidEmailDomain(allowedDomain: "5amcn.com",
        ////     ErrorMessage = "电子邮件的后缀必须是5amcn.com")]
        //public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]

        [Display(Name = "密码")]

        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password",
            ErrorMessage = "密码与确认密码不一致，请重新输入.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "姓名")]
        public string userNickname { set; get; }
        [Display(Name = "业务电话")]
        public string userWorkPhone { set; get; }
        [Display(Name = "家庭住址")]
        public string userAddress { set; get; }


        [Display(Name = "手机号")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "请输入正确的{0}")]
        [Remote(action: "IsPhoneInUser",controller:"Account")]
        public string Phone { get; set; }

    }
}
