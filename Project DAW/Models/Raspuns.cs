using System.ComponentModel.DataAnnotations;

namespace Project_DAW.Models
{
    public class Raspuns
    {
        [Key]
        public int Id { get; set; }
        public int IntrebareId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        [Required(ErrorMessage ="Nu poti sa dai un raspuns gol!")]
        public string Text { get; set; }
        public virtual Intrebare? Intrebare { get; set; }

    }
}
