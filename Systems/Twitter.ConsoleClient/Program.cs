using Microsoft.AspNetCore.SignalR.Client;
using Twitter.ConsoleClient.TokenHelper;

namespace Twitter.ConsoleClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            const string defaultUserName = "ak12345";
            string password = "1234567";
            const string serverUrl = "https://localhost:5001/connect/token";
            const string chatUrl = "https://localhost:5000/chat";

            var userName = defaultUserName;
            if (args?.Length > 0)
            {
                userName = args[0];
                password = args[1];
            }

            Console.WriteLine("Getting token...");
            var token = await TokenLoader.RequestToken(userName, password, serverUrl);

            if (string.IsNullOrWhiteSpace(token?.AccessToken))
            {
                Console.WriteLine("Request token error. Exit.");
                return;
            }

            Console.WriteLine("Creating connection...");
            var connection = new HubConnectionBuilder()
                .WithUrl(chatUrl, options =>
                {
                    // custom Token Provider if you needed
                    // options.AccessTokenProvider =

                    options.Headers.Add("Authorization", $"Bearer {token.AccessToken}");
                })
                .WithAutomaticReconnect()
                .Build();

            Console.WriteLine("Subscribe to actions...");

            #region subscriptions

            connection.On<IEnumerable<string>>("UpdateUsersAsync", users =>
            {
                Console.WriteLine("--------------------------------");
                var enumerable = users as string[] ?? users.ToArray();
                Console.WriteLine($"Total users: {enumerable.Length}");
                foreach (var user in enumerable)
                {
                    Console.WriteLine($"{user}");
                }
            });

            connection.On<string, string>("SendMessageAsync", (user, message) =>
             {
                 Console.WriteLine("--------------------------------");
                 Console.WriteLine($"{user} says {message}");
             });

            #endregion

            try
            {
                connection.StartAsync().GetAwaiter().GetResult();
                var messageHelper = new MessageHelper();

                while (true)
                {

                    var key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Spacebar)
                    {
                        await connection.SendAsync("SendMessageAsync", userName, messageHelper.GetRandom());
                    }

                    if (key.Key == ConsoleKey.Enter)
                    {
                        return;
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}
