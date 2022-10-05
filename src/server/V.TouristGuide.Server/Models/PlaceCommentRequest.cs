using System.ComponentModel.DataAnnotations;

namespace V.TouristGuide.Server.Models
{
    public class PlaceCommentRequest
    {
        public long PlaceId { get; set; }

        [Required]
        public string Comment { get; set; }
    }
}
