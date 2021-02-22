using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CMS5.ViewModels
{
    public class EditUserViewModel
    {
        //实现EditUserViewModel构造函数并初始化Claims和Roles，这是为了避免在运行时出现NullReFerence异常
        public EditUserViewModel() {
            Claims = new List<string>();
            Roles = new List<string>();
        }
        [Display(Name ="用户ID")]
        public string Id { get; set; }

        [Display(Name = "用户名")]
        public string  UserName { get; set; }


        [Display(Name = "邮箱")]
        public string  Email { get; set; }

        [Display(Name = "城市")]
        public string  City { get; set; }

        public List<string> Claims { get; set; }
        
        //【注】：Ilist
        public IList<string> Roles { get; set; }
    }
}
