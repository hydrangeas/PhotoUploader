using System;
using System.Linq;
using CVPTest.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CVPTest
{
    public class Startup
    {
        private readonly IHostingEnvironment env;
        private readonly IConfiguration config;

        public Startup(IHostingEnvironment _env, IConfiguration _config)
        {
            env = _env;
            config = _config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<JobDatabase>(options =>
            {
#if DEBUG
                var connectionString = config.GetConnectionString("JobDatabase");
                options.UseSqlite(connectionString);
#else
                // 与えられる情報が誤っているらしい
                // refs:https://social.msdn.microsoft.com/Forums/en-US/7e577b74-bbc8-41ea-a5e4-075b0eaa8622/aspnet-core-mvc-and-mysql-in-app?forum=windowsazurewebsitespreview
                // 変換元: Database=localdb;Data Source=127.0.0.1:PPPPP;User Id=azure;Password=XXXXX
                // 変換後: server=127.0.0.1;userid=azure;password=XXXXX;database=localdb;Port=PPPPP

                // ポート番号とパスワードだけ動的に取る（それ以外は固定決め打ち）

                var connectionString = Environment.GetEnvironmentVariable("MYSQLCONNSTR_localdb");
                var dictionary = connectionString.Split(';')
                                                 .Select(x => x.Split('='))
                                                 .ToDictionary(x => x[0], x => x[1]);
                connectionString = $"server=127.0.0.1;userid=azure;password={dictionary["Password"]};database=localdb;Port={dictionary["Data Source"].Split(':')[1]}";
                options.UseMySql(connectionString);
#endif
            });

            services.AddApplicationInsightsTelemetry(config);
            services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
