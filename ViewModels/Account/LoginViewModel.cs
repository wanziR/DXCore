using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CMS5.ViewModels
{
    public class LoginViewModel
    {

        [Display(Name = "手机号")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "请输入正确的{0}")]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "密码")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "记住我")]
        public bool RememberMe { get; set; }



    }
}
