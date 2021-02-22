using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS5.Models;
using Microsoft.EntityFrameworkCore;
using CMS5.Infrastructure;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CMS5.Infrastructure
{
    public class AppDbContext: IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        :base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region 添加种子数据

            //modelBuilder.Entity<Student>().HasData(
            //    new Student
            //    {
            //        Id = 1,
            //        Name = "周龙",
            //        Major = "计算机",
            //        Email = "5@5amcn.com"
            //    }
            //    );
            //modelBuilder.Entity<Student>().HasData(
            //   new Student
            //   {
            //       Id = 2,
            //       Name = "张三",
            //       Major = "工商管理",
            //       Email = "zs@5amcn.com" 
            //   }
            //   );

            #endregion
            //====>创建一个seed()方法来保存这些种子数据，而这个方法是扩展方法，参数中包含this关键字(/Infrastructure/ModelBuilderExtensions.cs)。
            base.OnModelCreating(modelBuilder);//解决"The entity type 'IdentityUserLogin<string>' requires a primary key to be defined."的错误 
            modelBuilder.Seed();

            //获取当前系统中所有领域模型上的外键列表
            var foreignKeys = modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys());

            foreach (var foreignKey in foreignKeys)
            {
                //将它们的删除行为配置为Restrict，即无操作
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
