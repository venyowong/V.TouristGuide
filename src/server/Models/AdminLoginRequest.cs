using System.ComponentModel.DataAnnotations;

namespace V.TouristGuide.Server.Models
{
    public class AdminLoginRequest
    {
        [Required]
        public string Pwd { get; set; }
    }
}
