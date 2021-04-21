using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using MsgExchanger3.Models;

namespace MsgExchanger3.Classes
{
    public class MsgHub: Hub
    {
        public async Task SendMessage(string userName, string message)
        {
            if (Clients != null)
            {
                string name = Context.User.Identity.Name;
#if DEBUG
                Debug.WriteLine($"name={name}");
#endif
                await Clients.All.SendAsync("ReceiveMessage",userName,  message);
            }
        }

        public async Task SendMetrics(Metrics metrics)
        {
            await Clients.Others.SendAsync("ReceiveMetrics", metrics);
        }

        public async Task SendInterval(int interval)
        {
            await Clients.Others.SendAsync("ReceiveInterval", interval);
        }

        public async Task GetInterval()
        {
            await Clients.Others.SendAsync("GetInterval");
        }
    }
}
