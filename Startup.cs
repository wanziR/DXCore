using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS5.Models;
using CMS5.CustomerMiddlewares;
using CMS5.DataRespositories;
using CMS5.Infrastructure;
using CMS5.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CMS5
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        // 此方法由运行时调用。使用此方法向容器添加服务。
        public void ConfigureServices(IServiceCollection services)
        {
            //使用Sqlserver数据，通过IConfiguration访问去获取，自定义名称的"MockStudentDBConnection"作为我们的链接字符串
            services.AddDbContextPool<AppDbContext>(
                options => options.UseSqlServer(_configuration.GetConnectionString("Cms5DBConnection")));
            services.AddControllersWithViews().AddXmlSerializerFormatters();
           
            services.AddControllersWithViews();

            //services.AddScoped<IStudentRepository,MockStudentRepository>();
            //services.AddScoped<IStudentRepository, SQLStudentRepository>();
        
            //将IRepository.cs和RepositoryBase.cs注册到应用中
            services.AddTransient(typeof(IRepository<,>), typeof(RepositoryBase<,>));

            //AddIdentity()是指为系统提供默认的用户和角色类型的的身份验证系统 ,并配置中文错误提示
            services.AddIdentity<ApplicationUser, IdentityRole>().AddErrorDescriber<CustomIdentityErrorDescriber>().AddEntityFrameworkStores<AppDbContext>();
            

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.SignIn.RequireConfirmedEmail = true;
                //通过自定义的CustomEmailConfirmation名称来覆盖旧有token名称，是它与AddTokenProvider<CustomEmailConfirmationTokenProvider<ApplicationUser>>("CustomEmailConfirmation")关联在一起
                options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            });
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                //修改登录地址的路由
                // options.LoginPath = new PathString("/Admin/Login");
                //修改注销地址的路由
                // options.LogoutPath = new PathString("/Admin/LogOut");
                //统一系统全局的Cookie名称
                options.Cookie.Name = "CMS5CookieName";
                // 登录用户Cookie的有效期
                //options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                //是否对Cookie启用滑动过期时间
                options.SlidingExpiration = true;

                options.LoginPath = "/Account/Login";
                //修改拒绝访问的路由地址
                options.AccessDeniedPath = new PathString("/Admin/AccessDenied");


            });
        }

        // 此方法由运行时调用。使用此方法配置HTTP请求管道。
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //否则显示友好的错误页面
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                app.UseHsts();
            }
         
            #region //中间件工作流程

            // app.Use(async (context, next) =>
            // {
            //     context.Response.ContentType = "text/plain; charset=utf-8"; //防止乱码
            //     logger.LogInformation("MW1:传入请求");
            //     await next();
            //     logger.LogInformation("MW1:传出响应");
            // });
            //
            // app.Use(async (context, next) =>
            // {
            //     logger.LogInformation("MW2: 传入请求");
            //     await next();
            //     logger.LogInformation("MW2: 传出响应");
            // });
            //
            // app.Run(async (context) =>
            // {
            //     await context.Response.WriteAsync("MW3: 处理请求并生成响应");
            //     logger.LogInformation("MW3: 处理请求并生成响应");
            // });

            #endregion

            app.UseHttpsRedirection();

            #region //将abc.html指定为默认文档  添加静态文件中间件

            // ////将abc.html指定为默认文档
            // //DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
            // //defaultFilesOptions.DefaultFileNames.Clear();
            // //defaultFilesOptions.DefaultFileNames.Add("abc.html.html");
            // ////添加默认文件中间件
            // //app.UseDefaultFiles(defaultFilesOptions);
            // ////添加静态文件中间件
            app.UseStaticFiles();
            //
            //
            // FileServerOptions fileServerOptions = new FileServerOptions();
            // fileServerOptions.DefaultFilesOptions.DefaultFileNames.Clear();
            // fileServerOptions.DefaultFilesOptions.DefaultFileNames.Add("abc.html");
            // app.UseFileServer(fileServerOptions);

            #endregion


            //添加验证中间件
            app.UseAuthentication();

            //
            app.UseRouting();

            //添加验证中间件
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Admin}/{action=ListRoles}/{id?}");
                // pattern: "{controller=Account}/{action=Login}");
            });
            //
           

           
        }
    }
}
