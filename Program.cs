using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using WorkLearnProject3;

class Program
{
    static void Main(string[] args)
    {
        WebHost.CreateDefaultBuilder().UseStartup<Startup>().Build().Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder().UseStartup<Startup>();
}