using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationSettings _applicationSettings;

        public HomeController(IOptions<ApplicationSettings> applicationSettings)
        {
            _applicationSettings = applicationSettings.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            ViewData["EnvironmentMessage"] = GetEnvironmentMessage();

            return View();
        }

        [Authorize]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
            //return StatusCode(200);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private string GetEnvironmentMessage()
        {
            var message = _applicationSettings.Messages.Environment;
            return message;
        }

    }
}
