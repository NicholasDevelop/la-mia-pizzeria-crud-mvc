using la_mia_pizzeria_static.Data;
using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace la_mia_pizzeria_static.Controllers
{
    public class PizzasController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            using (PizzeriaContext context = new PizzeriaContext())
            {
                IQueryable<Pizza> pizzaList = context.Pizzas.Include(p => p.Category).Include("Ingredient");
                //List<Pizza> pizzaList = (List<Pizza>)new PizzeriaContext().Pizzas.Include(p => p.Category);
                return View("index", pizzaList.ToList());
            }
        }

        [HttpGet]
        public IActionResult Detail(int id)
        {
            using(PizzeriaContext context = new PizzeriaContext())
            {
                Pizza current = context.Pizzas.Where(pizza => pizza.Id == id).Include("Category").Include("Ingredient").FirstOrDefault();
                if(current == null)
                {
                    return NotFound($"Il post con id {id} non è stato trovato!");
                }
                else
                {
                    return View("Detail", current);
                }
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PizzaCategories data)
        {
            if (!ModelState.IsValid)
            {
                using (PizzeriaContext context = new PizzeriaContext())
                {
                    List<Category> categories = context.Categories.ToList();
                    data.Categories = categories;

                    List<SelectListItem> ingredientList = new List<SelectListItem>();
                    List<Ingredient> ingredients = context.Ingredients.ToList();

                    foreach (Ingredient i in ingredients)
                    {
                        ingredientList.Add(new SelectListItem() { Text = i.Name, Value = i.Id.ToString() });
                    }
                    data.Ingredients = ingredientList;

                    return View("Create", data);
                }
            }

            using (PizzeriaContext context = new PizzeriaContext())
            {
                Pizza pizzaToCreate = new Pizza();
                pizzaToCreate.Name = data.Pizza.Name;
                pizzaToCreate.Description = data.Pizza.Description;
                pizzaToCreate.Img = data.Pizza.Img;
                pizzaToCreate.Price = data.Pizza.Price;
                pizzaToCreate.CategoryId = data.Pizza.CategoryId;

                pizzaToCreate.Ingredients = new List<Ingredient>();

                //se non è stato selezionato nessun tag, la proprietà sarà null e genererà errore
                if (data.SelectedIngredients != null)
                {
                    foreach (string selectedIngredientId in data.SelectedIngredients)
                    {
                        int selectedIntIngredientId = int.Parse(selectedIngredientId);
                        Ingredient ingredient = context.Ingredients.Where(i => i.Id == selectedIntIngredientId).FirstOrDefault();

                        pizzaToCreate.Ingredients.Add(ingredient);
                    }
                }

                context.Ingredients.Add(pizzaToCreate);
                context.SaveChanges();

                return RedirectToAction("Index");
            }
        }



        [HttpGet]
        public IActionResult Create()
        {
            using (PizzeriaContext context = new PizzeriaContext())
            {
                List<Category> categories = context.Categories.ToList();

                PizzaCategories model = new PizzaCategories();

                model.Categories = categories;
                model.Pizza = new Pizza();

                List<SelectListItem> ingredientList = new List<SelectListItem>();
                List<Ingredient> ingredients = context.Ingredients.ToList();

                foreach(Ingredient i in ingredients)
                {
                    ingredientList.Add(new SelectListItem() { Text = i.Name, Value = i.Id.ToString() });
                }

                model.Ingredients = ingredientList;

                return View("Create", model);
            }
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
                PizzaCategories model = new PizzaCategories();
                model.Pizza = pizza;
                model.Categories = context.Categories.ToList();

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, PizzaCategories data)
        {
            using(PizzeriaContext context = new PizzeriaContext())
            {
                if (!ModelState.IsValid)
                {
                    data.Categories = context.Categories.ToList();
                    return View(data);
                }

                Pizza editPizza = context.Pizzas.Where(p => p.Id == id).FirstOrDefault();
                  
                if(editPizza == null)
                {
                    return NotFound();
                }

                editPizza.Name = data.Pizza.Name;
                editPizza.Description = data.Pizza.Description;
                editPizza.Price = data.Pizza.Price;
                editPizza.Img = data.Pizza.Img;
                editPizza.CategoryId = data.Pizza.CategoryId;

                context.SaveChanges();

                return RedirectToAction("index");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            using(PizzeriaContext context = new PizzeriaContext())
            {
                Pizza removePizza = context.Pizzas.Where(p => p.Id == id).FirstOrDefault();

                if (removePizza == null)
                {
                    return NotFound();
                }

                context.Pizzas.Remove(removePizza);
                context.SaveChanges();
                return RedirectToAction("index");

            }
        }
    }
}
