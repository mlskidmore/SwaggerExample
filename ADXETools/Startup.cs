using ADXETools.FalconRequests;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Examples;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Net.Http;
using WebApiContrib.Core.Formatter.PlainText;

namespace ADXETools
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        private readonly IEnvironmentConfiguration envConfig;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            envConfig = new EnvironmentConfiguration(Configuration);
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddOptions();
            services.AddLogging();

            MvcOptions mvcOptions = new MvcOptions
            {
                AllowBindingHeaderValuesToNonStringModelTypes = true,

            };
            services.AddMvc(config =>
            {
                // Add XML and Text Content Negotiation
                config.RespectBrowserAcceptHeader = true;
                //config.InputFormatters.Clear();
                //config.OutputFormatters.Clear();
                config.InputFormatters.Add(new Formatters.XmlSerializerInputFormatter());
                config.OutputFormatters.Add(new Formatters.XmlSerializerOutputFormatter());
            })
            //.AddXmlSerializerFormatters()
            .AddPlainTextFormatters()
            .AddJsonOptions(opt => opt.SerializerSettings.ContractResolver = new DefaultContractResolver()); // fixes XML deserialization

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "ADXE Tools API", Version = "v1" });
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "ADXETools.xml");
                c.IncludeXmlComments(xmlPath);
                c.OperationFilter<ExamplesOperationFilter>();
            });

            services.TryAddSingleton<IEnvironmentConfiguration>(new EnvironmentConfiguration(Configuration));
            services.TryAddSingleton<HttpClient>();
            services.TryAddSingleton<IFalconPort, FalconPort>();
            services.TryAddSingleton<ADXECertificateValidationHandler>();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            try
            {
                JsonConvert.DefaultSettings = () => new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }

                app.UseSwagger(c =>
                {
                    c.PreSerializeFilters.Add((swaggerDoc, httpReq) => swaggerDoc.Host = httpReq.Host.Value);
                });
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("../swagger/v1/swagger.json", "ADXE Tools API");
                });

                app.UseMvcWithDefaultRoute();
                app.UseMvc();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
