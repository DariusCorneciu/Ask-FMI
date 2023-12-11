using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;


// PASUL 1 - useri si roluri 

namespace Project_DAW.Models
{
    public class ApplicationUser : IdentityUser
    {
        // un user poate posta mai multe comentarii
        public virtual ICollection<Comentariu>? Comentarii { get; set; }

        // un user poate posta mai multe articole
        public virtual ICollection<Intrebare>? Intrebari { get; set; }

        
        // atribute suplimentare adaugate pentru user
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        // variabila in care vom retine rolurile existente in baza de date
        // pentru popularea unui dropdown list
        [NotMapped]
        public IEnumerable<SelectListItem>? AllRoles { get; set; }

    }
}