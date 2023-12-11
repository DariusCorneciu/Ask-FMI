using System.ComponentModel.DataAnnotations;

namespace Project_DAW.Models
{
    public class Categorie
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Titlul categoriei este necesar!!!")]
        public string Name { get; set; }
        public virtual ICollection<SubCategorie> SubCategorii { get; set; }
        
    }
}
