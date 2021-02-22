﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CMS5.ViewModels
{
    /// <summary>
    /// 用户拥有的角色列表
    /// </summary>
    public class RolesInUserViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsSelected { get; set; }
    }
}
