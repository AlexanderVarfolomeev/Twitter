using System;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Twitter.WPF.Services.AccountService;
using Twitter.WPF.Services.AccountService.Models;
using Twitter.WPF.Services.CommentsService;
using Twitter.WPF.Services.CommentsService.Models;
using Twitter.WPF.Services.TweetsService;
using Twitter.WPF.Services.TweetsService.Models;
using Twitter.WPF.Services.UserDialogService;
using Twitter.WPF.ViewModels;
using Path = System.IO.Path;

namespace Twitter.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static bool IsDesignMode { get; private set; } = true;
        private static IHost? _host;
        public static IHost Host => _host ??= Program.CreateHostBuilder(Environment.GetCommandLineArgs()).Build();
        protected override async void OnStartup(StartupEventArgs e)
        {
            IsDesignMode = false;   
            var host = Host;
            base.OnStartup(e);
            await host.StartAsync().ConfigureAwait(false);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            var host = Host;
            await host.StopAsync();
            host.Dispose();
            _host = null;
        }

        public static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
        {
            services.AddScoped<MainViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<AddTweetViewModel>();
            services.AddTransient<AddCommentViewModel>();
            
            services.AddScoped<IUserDialogService, UserDialogService>();
            services.AddSingleton<HttpClient>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ITweetsService, TweetService>();
            services.AddScoped<ICommentsService, CommentsService>();
        }

        public static string CurrentDirectory => IsDesignMode ? Path.GetDirectoryName(GetSourceCodePath()) : Environment.CurrentDirectory;

        private static string GetSourceCodePath([CallerFilePath] string path = null) => path;
    }
}