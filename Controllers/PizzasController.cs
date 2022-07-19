using la_mia_pizzeria_static.Data;
using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Mvc;

namespace la_mia_pizzeria_static.Controllers
{
    public class PizzasController : Controller
    {
        public IActionResult Index()
        {
            using (PizzeriaContext context = new PizzeriaContext())
            {
                List<Pizza> pizzaList = context.Pizzas.ToList();
                return View(pizzaList);
            }
        }

        public IActionResult Detail(int id)
        {
            using(PizzeriaContext context = new PizzeriaContext())
            {
                Pizza current = context.Pizzas.Where(pizza => pizza.Id == id).FirstOrDefault();
                if(current == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(current);
                }
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Pizza pizza)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", pizza);
            }

            using(PizzeriaContext context = new PizzeriaContext())
            {
                context.Pizzas.Add(pizza);
                context.SaveChanges();
            }

            return RedirectToAction("Index");
        }



        [HttpGet]
        public IActionResult Create()
        {
            return View("Create");
        }
        
    }
}
