using System.ComponentModel.DataAnnotations.Schema;

namespace V.TouristGuide.Server.Entities
{
    public class PlaceAddition : BaseEntity
    {
        [Column("place_id")]
        public long PlaceId { get; set; }

        /// <summary>
        /// 类型(菜单、景点照片、文章)
        /// </summary>
        public string Type { get; set; }

        public string Title { get; set; }

        public string Img { get; set; }

        public string Url { get; set; }

        public string Desc { get; set; }

        /// <summary>
        /// 素材来源
        /// </summary>
        public string Source { get; set; }

        [Column("source_url")]
        public string SourceUrl { get; set; }
    }
}
