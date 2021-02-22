using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CMS5.ViewModels
{
    public class EditRoleViewModel
    {
       
        /// <summary>
        /// 解决 EditRole.cshtml 中 Model.Users.Any()出错的问题
        /// </summary>
        public EditRoleViewModel() {
            Users = new List<string>();
        }


        [Display(Name ="角色ID")]
        public string Id { get; set; }

        [Required(ErrorMessage ="角色名称是必填的")]
        [Display(Name = "角色名称")]
        public string RoleName { get; set; }
        public List<string> Users { get; set; }

    }
}
