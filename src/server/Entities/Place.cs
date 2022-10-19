using System.ComponentModel.DataAnnotations.Schema;

namespace V.TouristGuide.Server.Entities
{
    public class Place : BaseEntity
    {
        public string Name { get; set; }

        /// <summary>
        /// 封面
        /// </summary>
        public string Cover { get; set; }

        /// <summary>
        /// 开放时间
        /// </summary>
        [Column("open_time")]
        public string OpenTime { get; set; }

        public string Address { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public decimal Lng { get; set; }

        /// <summary>
        /// 维度
        /// </summary>
        public decimal Lat { get; set; }

        public string Tel { get; set; }

        /// <summary>
        /// 0 美食 1 景点
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 能见度(m)
        /// <para>超出该距离地图上不可见</para>
        /// </summary>
        public double Visibility { get; set; }

        /// <summary>
        /// 百度地图区域
        /// </summary>
        public string Region { get; set; }
    }
}
