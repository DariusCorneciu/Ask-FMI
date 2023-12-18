using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace Project_DAW.Models
{
    public class Imagine
    {
        [Key]
        public int Id { get; set; }
        public string? UserId { get; set; }
        public long Size { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public byte[] ImageData { get; set; }

        public string Usage {  get; set; }
        public virtual ApplicationUser ?User { get; set; }
    }
}
