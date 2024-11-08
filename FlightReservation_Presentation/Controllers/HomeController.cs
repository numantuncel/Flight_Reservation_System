using FlightReservation_Presentation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FlightReservation_Presentation.Controllers
{
    public class HomeController : Controller
    {
        // admin
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }

        // user
        [Authorize(Roles = "User,Admin")]
        public IActionResult UserUI()
        {
            return View();
        }
    }
}
