﻿using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Twitter.WPF;

public static class Program
{
    [STAThread]
    public static void Main()
    {
        var app = new App();
        app.InitializeComponent();
        app.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        var hostBuilder = Host.CreateDefaultBuilder(args)
            .UseContentRoot(App.CurrentDirectory)
            .ConfigureAppConfiguration((host, cfg) =>
            {
                cfg.SetBasePath(App.CurrentDirectory);
                cfg.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            });

        hostBuilder.ConfigureServices(App.ConfigureServices);

        return hostBuilder;
    }
}