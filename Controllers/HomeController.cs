using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace la_mia_pizzeria_static.Controllers
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

    //public class PizzaController : Controller
    //{
    //    public class Pizza
    //    {
    //        public string Name { get; set; }
    //        public string Description { get; set; }
    //        public string Img { get; set; }
    //        public double Price { get; set; }

    //        public Pizza(string name, string description, string img, double price)
    //        {
    //            Name = name;
    //            Description = description;
    //            Img = img;
    //            Price = price;
    //        }
    //    }
    //}
}