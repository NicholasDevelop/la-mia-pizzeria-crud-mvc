using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace la_mia_pizzeria_static.Models
{
    [Table("ingredient")]
    public class Ingredient
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Il campo è obbligatorio")]
        [StringLength(25, ErrorMessage = "Il nome non può avere più di 25 caratteri")]
        public string Name { get; set; }

        //**CAMPI CHIAVE ESTERNA**
        public List<Pizza> Pizzas { get; set; }

        public Ingredient()
        {

        }

        public Ingredient(string name)
        {
            Name = name;
        }
    }
}
