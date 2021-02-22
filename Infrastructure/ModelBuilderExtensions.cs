using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS5.Models;
using CMS5.Models.EnumTypes;


namespace CMS5.Infrastructure
{
    public static class ModelBuildeExtensions
    {
      
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasData(
                       new Student
                       {
                           Id = 1,
                           Name = "周龙",
                           Major =MajorEnum.ComputerScience,
                           Email = "5@5amcn.com"
                       }
                       );
            modelBuilder.Entity<Student>().HasData(
               new Student
               {
                   Id = 2,
                   Name = "张三",
                   Major = MajorEnum.Mathematics,
                   Email = "zs@5amcn.com"
               }
               );
            modelBuilder.Entity<Student>().HasData(
                new Student
                {
                    Id = 3,
                    Name = "李四",
                    Major = MajorEnum.ElectronicCommerce,
                    Email = "ls@5amcn.com"
                }
            );  
            modelBuilder.Entity<Student>().HasData(
                new Student
                {
                    Id = 4,
                    Name = "王五",
                    Major = MajorEnum.ElectronicCommerce,
                    Email = "ww@5amcn.com"
                }
            );
        }
    }
}
