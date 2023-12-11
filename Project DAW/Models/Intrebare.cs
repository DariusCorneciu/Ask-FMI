using System.ComponentModel.DataAnnotations;

namespace Project_DAW.Models
{
    public class Intrebare
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Intrebarea trebuie sa fie adaugata!!!!")]
        public string Continut { get; set; }
        public DateTime Date { get; set; }

        public bool IsOpen { get; set; }
        public virtual Raspuns? Raspuns { get; set; }
    }
}
