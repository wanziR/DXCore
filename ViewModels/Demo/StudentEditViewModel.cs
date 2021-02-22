using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS5.ViewModels
{
    public class StudentEditViewModel:StudentCreateViewModel
    {
        public int Id { get; set; }
        /// <summary>
        /// 已存在数据库中的图片文件路径
        /// </summary>
        public string ExistingPhotoPath { get; set; }
    }
}
