using System.ComponentModel.DataAnnotations;

namespace Project_DAW.Models
{
    public class Comentariu
    {
        [Key]
        public int Id { get; set; }

        public int Intrebare_ID { get; set; }

        public virtual Intrebare? Intrebare { get; set; }

        [Required(ErrorMessage = "Comentariul trebuie sa aiba continut!!!")]
        public string Continut { get; set; }


    }
}
