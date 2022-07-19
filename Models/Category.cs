using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace la_mia_pizzeria_static.Models
{
    [Table("category")]
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Il campo è obbligatorio")]
        [StringLength(40, ErrorMessage = "Il nome non può avere più di 40 caratteri")]
        public string Name { get; set; }

        public List<Pizza> Pizzas { get; set; }


        public Category()
        {

        }
    }
}
