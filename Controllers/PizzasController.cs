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


        [HttpGet]
        public IActionResult Update(int id)
        {
            using(PizzeriaContext context = new PizzeriaContext())
            {
                Pizza pizza = context.Pizzas.Where(p => p.Id == id).FirstOrDefault();
                //Pizza pizza = (from p in context.Pizzas where p.Id == id select p).FirstOrDefault();
                if(pizza == null)
                {
                    return NotFound();
                }

                return View(pizza);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, Pizza pizza)
        {
            if (!ModelState.IsValid)
            {
                return View(pizza);
            }

            using(PizzeriaContext context = new PizzeriaContext())
            {
                Pizza editPizza = context.Pizzas.Where(p => p.Id == id).FirstOrDefault();

                if(editPizza == null)
                {
                    return NotFound();
                }

                editPizza.Name = pizza.Name;
                editPizza.Description = pizza.Description;
                editPizza.Price = pizza.Price;
                editPizza.Img = pizza.Img;

                context.SaveChanges();

                return RedirectToAction("index");
            }
        }
    }
}
