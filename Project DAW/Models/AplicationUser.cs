using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;


namespace Project_DAW.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Comentariu>? Comentarii { get; set; }

        public virtual ICollection<Intrebare>? Intrebari { get; set; }
        public byte[] ?ProfilePicture { get; set; }
        public string ?ProfileType { get; set; }

        public byte[]? BackroundPicture {  get; set; }
        public string? BackroundType { get; set; }

        // atribute suplimentare adaugate pentru user
        public string? FirstName { get; set; }
        public DateTime JoinDate {  get; set; }
        public bool Moderator { get; set; }
        
         /// <summary>
         ///  rolurile suplimentare ale aplicatiei
         /// </summary>
        public bool Admitere { get; set; }
        public bool Licenta {  get; set; }
        public bool Master {  get; set; }

        public DateTime? BanTime { get; set; }

        public string? LastName { get; set; }

        public  string ProfileImage()
        {
            if(ProfilePicture != null && ProfileType !=null)
            {
                var ImagineBaza64 = Convert.ToBase64String(ProfilePicture);
                string Imaginesrc = $"data:{ProfileType};base64,{ImagineBaza64}";

                return Imaginesrc;

            }
            else { return "/images/default.jpg"; }
        }
        public string BackroundImage()
        {
            if (BackroundPicture != null && BackroundType != null)
            {
                var ImagineBaza64 = Convert.ToBase64String(BackroundPicture);
                string Imaginesrc = $"data:{BackroundType};base64,{ImagineBaza64}";

                return Imaginesrc;

            }
            else { return "/images/defaultb.jpg"; }

        }



        

    }
}