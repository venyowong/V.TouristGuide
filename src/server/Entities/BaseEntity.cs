using System.ComponentModel.DataAnnotations.Schema;

namespace V.TouristGuide.Server.Entities
{
    public class BaseEntity
    {
        [Column("id")]
        public long Id { get; set; }

        [Column("is_valid")]
        public bool IsValid { get; set; }

        [Column("create_time")]
        public DateTime CreateTime { get; set; }

        [Column("update_time")]
        public DateTime UpdateTime { get; set; }
    }
}
