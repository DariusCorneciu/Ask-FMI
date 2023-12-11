using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_DAW.Models
{
    public class Intrebare
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Intrebarea trebuie sa fie adaugata!!!!")]
        public string Continut { get; set; }
        public DateTime Date { get; set; }
        
        public int? SubCategorieId { get; set; }
        public bool IsOpen { get; set; }
        public virtual SubCategorie? SubCategorie { get; set; }

        public virtual ICollection<Comentariu>? Comentarii { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? SubCateg { get; set; }
        public string? UserId { get; set; }

        public virtual ApplicationUser? User { get; set; }


        public virtual Raspuns? Raspuns { get; set; }
       
    }
}
