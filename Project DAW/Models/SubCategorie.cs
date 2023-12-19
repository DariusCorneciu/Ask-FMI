using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public virtual ICollection<Intrebare> Intrebari { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem> Categ { get; internal set; }
    }
}
