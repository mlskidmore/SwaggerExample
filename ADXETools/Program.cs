using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace SwaggerExample
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            try
            {
                Console.Title = "SwaggerExample";
                BuildWebHost(args).Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((context, builder) =>
        {
            builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            builder.AddJsonFile($"appsettings.{context.HostingEnvironment}.json", optional: true);
            builder.AddEnvironmentVariables();
            if (context.HostingEnvironment.IsDevelopment())
            {
                builder.AddUserSecrets("SwaggerExample");
            }
        })
        .UseStartup<Startup>()
        .Build();
    }
}