using System.ComponentModel.DataAnnotations;

namespace Project_DAW.Models
{
    public class Comentariu
    {
        [Key]
        public int Id { get; set; }

        public string? UserId { get; set; }
        public int? IntrebareId { get; set; }

        public virtual Intrebare? Intrebare { get; set; }
        public virtual ApplicationUser? User { get; set; }
        [Required(ErrorMessage = "Comentariul trebuie sa aiba continut!!!")]
        public string Continut { get; set; }
        public DateTime Date { get; set; }


    }
}
