using System.ComponentModel.DataAnnotations.Schema;

namespace V.TouristGuide.Server.Entities
{
    public class Operation : BaseEntity
    {
        [Column("user_id")]
        public long UserId { get; set; }

        [Column("place_id")]
        public long PlaceId { get; set; }

        /// <summary>
        /// 0 收藏 1 评论
        /// </summary>
        public int Type { get; set; }

        public string Remark { get; set; }
    }
}
