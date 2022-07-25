using la_mia_pizzeria_static.ValidationAttributes;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace la_mia_pizzeria_static.Models
{
    [Table("pizza")]
    [Authorize]
    public class Pizza
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Il campo è obbligatorio")]
        [StringLength(40, ErrorMessage = "Il nome non può avere più di 40 caratteri")]
        public string Name { get; set; }

        [StringLength(250, ErrorMessage = "Il nome non può avere più di 250 caratteri")]
        //[FiveWordsValidationAttribute]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Il campo è obbligatorio")]
        [StringLength(250, ErrorMessage = "Il nome non può avere più di 250 caratteri")]
        [Url]
        public string Img { get; set; }

        [Required(ErrorMessage = "Il campo è obbligatorio")]
        [Range(0.1,50,ErrorMessage = "Il costo della pizza deve essere compreso tra 0.1 e 50 euro")]
        public double Price { get; set; }


        //**CAMPI CHIAVE ESTERNA**
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public List<Ingredient>? Ingredients { get; set; }


        public Pizza()
        {

        }

        public Pizza(string name, string description, string img, double price)
        {
            Name = name;
            Description = description;
            Img = img;
            Price = price;
        }

    }
}
