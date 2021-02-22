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


        // �˷���������ʱ���á�ʹ�ô˷�����������ӷ���
        public void ConfigureServices(IServiceCollection services)
        {
            //ʹ��Sqlserver���ݣ�ͨ��IConfiguration����ȥ��ȡ���Զ������Ƶ�"MockStudentDBConnection"��Ϊ���ǵ������ַ���
            services.AddDbContextPool<AppDbContext>(
                options => options.UseSqlServer(_configuration.GetConnectionString("Cms5DBConnection")));
            services.AddControllersWithViews().AddXmlSerializerFormatters();
           
            services.AddControllersWithViews();

            //services.AddScoped<IStudentRepository,MockStudentRepository>();
            //services.AddScoped<IStudentRepository, SQLStudentRepository>();
        
            //��IRepository.cs��RepositoryBase.csע�ᵽӦ����
            services.AddTransient(typeof(IRepository<,>), typeof(RepositoryBase<,>));

            //AddIdentity()��ָΪϵͳ�ṩĬ�ϵ��û��ͽ�ɫ���͵ĵ������֤ϵͳ ,���������Ĵ�����ʾ
            services.AddIdentity<ApplicationUser, IdentityRole>().AddErrorDescriber<CustomIdentityErrorDescriber>().AddEntityFrameworkStores<AppDbContext>();
            

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.SignIn.RequireConfirmedEmail = true;
                //ͨ���Զ����CustomEmailConfirmation���������Ǿ���token���ƣ�������AddTokenProvider<CustomEmailConfirmationTokenProvider<ApplicationUser>>("CustomEmailConfirmation")������һ��
                options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            });
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                //�޸ĵ�¼��ַ��·��
                // options.LoginPath = new PathString("/Admin/Login");
                //�޸�ע����ַ��·��
                // options.LogoutPath = new PathString("/Admin/LogOut");
                //ͳһϵͳȫ�ֵ�Cookie����
                options.Cookie.Name = "CMS5CookieName";
                // ��¼�û�Cookie����Ч��
                //options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                //�Ƿ��Cookie���û�������ʱ��
                options.SlidingExpiration = true;

                options.LoginPath = "/Account/Login";
                //�޸ľܾ����ʵ�·�ɵ�ַ
                options.AccessDeniedPath = new PathString("/Admin/AccessDenied");


            });
        }

        // �˷���������ʱ���á�ʹ�ô˷�������HTTP����ܵ���
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //������ʾ�ѺõĴ���ҳ��
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                app.UseHsts();
            }
         
            #region //�м����������

            // app.Use(async (context, next) =>
            // {
            //     context.Response.ContentType = "text/plain; charset=utf-8"; //��ֹ����
            //     logger.LogInformation("MW1:��������");
            //     await next();
            //     logger.LogInformation("MW1:������Ӧ");
            // });
            //
            // app.Use(async (context, next) =>
            // {
            //     logger.LogInformation("MW2: ��������");
            //     await next();
            //     logger.LogInformation("MW2: ������Ӧ");
            // });
            //
            // app.Run(async (context) =>
            // {
            //     await context.Response.WriteAsync("MW3: ��������������Ӧ");
            //     logger.LogInformation("MW3: ��������������Ӧ");
            // });

            #endregion

            app.UseHttpsRedirection();

            #region //��abc.htmlָ��ΪĬ���ĵ�  ��Ӿ�̬�ļ��м��

            // ////��abc.htmlָ��ΪĬ���ĵ�
            // //DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
            // //defaultFilesOptions.DefaultFileNames.Clear();
            // //defaultFilesOptions.DefaultFileNames.Add("abc.html.html");
            // ////���Ĭ���ļ��м��
            // //app.UseDefaultFiles(defaultFilesOptions);
            // ////��Ӿ�̬�ļ��м��
            app.UseStaticFiles();
            //
            //
            // FileServerOptions fileServerOptions = new FileServerOptions();
            // fileServerOptions.DefaultFilesOptions.DefaultFileNames.Clear();
            // fileServerOptions.DefaultFilesOptions.DefaultFileNames.Add("abc.html");
            // app.UseFileServer(fileServerOptions);

            #endregion


            //�����֤�м��
            app.UseAuthentication();

            //
            app.UseRouting();

            //�����֤�м��
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
