using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace TestSwagger.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            //modified by wangdong from dev 1
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "API",
                    Description = "api文档",
                    TermsOfService = "None"
                });
                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "TestSwagger.WebApi.xml");
                var xmlPathByModel = Path.Combine(basePath, "TestSwagger.Model.xml");
                options.IncludeXmlComments(xmlPathByModel);
                //true表示生成控制器描述，包含true的IncludeXmlComments重载应放在最后，或者两句都使用true
                options.IncludeXmlComments(xmlPath, true);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseStaticFiles();//启用默认文件夹wwwroot
            app.UseSwagger();
            app.UseSwaggerUI(action =>
            {
                action.ShowExtensions();
                action.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Docs");
            });
        }
    }
}
