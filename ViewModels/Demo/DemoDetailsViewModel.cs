using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS5.Models;

namespace CMS5.ViewModels
{
    public class DemoDetailsViewModel
    {
      public Student Student { get; set; }

      public Student GetAllStudents { get; set; }
      public string PageTitle { get; set; }

    }
}
