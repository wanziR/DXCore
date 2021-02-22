using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS5.Models
{
    public class ApplicationUser:IdentityUser
    {
        public int userAge { set; get; }
        public string userTag { set; get; }
        public string userPwd { set; get; }
        public string userNickname { set; get; }
        public string userImg { set; get; }
        public string userPhone { set; get; }
        public string mobile { set; get; }
        public string userEdu { set; get; }
        public string userSex { set; get; }
        public string userEmail { set; get; }
        public string userWechatid { set; get; }
        public string userWechatname { set; get; }
        public DateTime userAddtime { set; get; }
        public string userAddress { set; get; }
        public string userWorkPhone { set; get; }
        public string City { get; set; }

    }
}
