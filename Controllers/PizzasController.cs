using la_mia_pizzeria_static.Data;
using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace la_mia_pizzeria_static.Controllers
{
    [Authorize]
    public class PizzasController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            using (PizzeriaContext context = new PizzeriaContext())
            {
                IQueryable<Pizza> pizzaList = context.Pizzas.Include(p => p.Category).Include(i => i.Ingredients);
                //List<Pizza> pizzaList = (List<Pizza>)new PizzeriaContext().Pizzas.Include(p => p.Category);
                return View("index", pizzaList.ToList());
            }
        }

        [HttpGet]
        public IActionResult Detail(int id)
        {
            using(PizzeriaContext context = new PizzeriaContext())
            {
                Pizza current = context.Pizzas.Where(pizza => pizza.Id == id).Include("Category").Include(i => i.Ingredients).FirstOrDefault();
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

                    data.Ingredients = RetriveIngredientListItem();

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

                context.Pizzas.Add(pizzaToCreate);
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

                model.Ingredients = RetriveIngredientListItem();

                return View("Create", model);
            }
        }


        [HttpGet]
        public IActionResult Update(int id)
        {
            using (PizzeriaContext context = new PizzeriaContext())
            {
                Pizza pizzaEdit = context.Pizzas.Where(p => p.Id == id).Include("Category").Include(i => i.Ingredients).FirstOrDefault();
                //Pizza pizza = (from p in context.Pizzas where p.Id == id select p).FirstOrDefault();
                if (pizzaEdit == null)
                {
                    return NotFound();
                }
                List<Category> categories = context.Categories.ToList();
                PizzaCategories model = new PizzaCategories();
                model.Pizza = pizzaEdit;
                model.Categories = categories;
                model.SelectedIngredients = new List<string>();

                foreach(Ingredient i in pizzaEdit.Ingredients)
                {
                    model.SelectedIngredients.Add(i.Id.ToString());
                }

                model.Ingredients = RetriveIngredientListItem();

                return View(model);
            }
        }
            

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, PizzaCategories data)
        {
            if (!ModelState.IsValid)
            {
                using (PizzeriaContext context = new PizzeriaContext())
                {
                    List<Category> categories = context.Categories.ToList();
                    data.Categories = categories;

                    data.Ingredients = RetriveIngredientListItem();

                    return View("Update", data);
                }
            }

            using (PizzeriaContext context = new PizzeriaContext())
            {
                Pizza pizzaToEdit = context.Pizzas.Where(pizza => pizza.Id == id).Include("Category").Include("Ingredients").FirstOrDefault();

                if(pizzaToEdit != null)
                {
                    //aggiorniamo i campi con i nuovi valori
                    pizzaToEdit.Name = data.Pizza.Name;
                    pizzaToEdit.Description = data.Pizza.Description;
                    pizzaToEdit.Img = data.Pizza.Img;
                    pizzaToEdit.Price = data.Pizza.Price;
                    pizzaToEdit.CategoryId = data.Pizza.CategoryId;

                    pizzaToEdit.Ingredients.Clear();

                    if (data.SelectedIngredients != null)
                    {
                        foreach (string selectedIngredientId in data.SelectedIngredients)
                        {
                            int selectedIntIngredientId = int.Parse(selectedIngredientId);
                            Ingredient ingredient = context.Ingredients.Where(i => i.Id == selectedIntIngredientId).FirstOrDefault();

                            pizzaToEdit.Ingredients.Add(ingredient);
                        }
                    }

                    context.SaveChanges();
                }

                return RedirectToAction("Index");
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



        private static List<SelectListItem> RetriveIngredientListItem()
        {
            using (PizzeriaContext context = new PizzeriaContext())
            {
                List<SelectListItem> ingredientList = new List<SelectListItem>();
                List<Ingredient> ingredients = context.Ingredients.ToList();

                foreach (Ingredient i in ingredients)
                {
                    ingredientList.Add(new SelectListItem() { Text = i.Name, Value = i.Id.ToString() });
                }

                return ingredientList;
            }
        }
    }
}
