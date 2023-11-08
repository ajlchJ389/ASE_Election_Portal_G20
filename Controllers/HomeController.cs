﻿using ASE_Election_Portal_G20.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ASE_Election_Portal_G20.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ElectionPortalG20Context electionPortal;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            this.electionPortal = electionPortal;
        }

        public IActionResult Index()
        {
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
    }
}