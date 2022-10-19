using V.TouristGuide.Server.Entities;

namespace V.TouristGuide.Server.Models
{
    public class PlaceDetail
    {
        public Place Place { get; set; }

        public List<PlaceAddition> Pictures { get; set; }

        public List<PlaceAddition> Articles { get; set; }

        public List<PlaceAddition> Videos { get; set; }

        public List<PlaceAddition> Comments { get; set; }

        /// <summary>
        /// 收藏数
        /// </summary>
        public int Stars { get; set; }

        /// <summary>
        /// 当前用户已收藏
        /// </summary>
        public bool HasStar { get; set; }

        /// <summary>
        /// 用户评论
        /// </summary>
        public List<Comment> CommentOperations { get; set; }
    }
}
