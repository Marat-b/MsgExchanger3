using System;
using Microsoft.AspNetCore.SignalR.Client;

namespace MessagerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:40407/msg")
                .Build();

            System.Timers.Timer aTimer = new System.Timers.Timer();
            
            connection.StartAsync().Wait();
            connection.InvokeAsync("GetInterval");
            connection.On("ReceiveInterval",
                (int interval) =>
                {
                    aTimer.Interval = interval;
                    Console.WriteLine($"\nIntervel={interval.ToString()}"); });

            aTimer.Elapsed += (sender, args) => KeepAliveElapsed(sender, connection);
            aTimer.Interval = 2000;
            aTimer.Enabled = true;
            Console.ReadKey();
        }

        public static void KeepAliveElapsed(object sender, HubConnection connection)
        {
            
            var hardware = new Hardware(new Metrics());
            var metrics = hardware.GetMetrics();
            if (metrics == null)
            {
                 metrics = new Metrics();
            }
            connection.InvokeAsync("SendMetrics", metrics);
        }
        
    }
}
