using System.ComponentModel.DataAnnotations;

namespace Project_DAW.Models
{
    public class SubCategorie
    {
        [Key]
        public int Id { get; set; }

        public int CategorieId { get; set;}


        [Required(ErrorMessage = "Numele Categoriei este necesar")]
        public string Title { get; set; }
        public virtual Categorie? Categorie { get; set; }

    }
}
