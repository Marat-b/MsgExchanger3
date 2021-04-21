using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MsgExchanger3.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualBasic.CompilerServices;
using MsgExchanger3.Classes;

namespace MsgExchanger3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            string ipAddress = GetIpAddress();
            ViewBag.IpAddress = ipAddress;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private string GetIpAddress()
        {
            var IpAddress = Dns.GetHostEntry(Dns.GetHostName())
                .AddressList
                .First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                .ToString();

            return IpAddress;
        }
    }
}
