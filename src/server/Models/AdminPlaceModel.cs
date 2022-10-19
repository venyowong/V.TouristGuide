namespace V.TouristGuide.Server.Models
{
    public class AdminPlaceModel
    {
        public string Name { get; set; }

        /// <summary>
        /// 封面
        /// </summary>
        public string Cover { get; set; }

        /// <summary>
        /// 开放时间
        /// </summary>
        public string OpenTime { get; set; }

        public AdminAddress Address { get; set; }

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

        public class AdminAddress
        {
            public string City { get; set; }

            public string Address { get; set; }

            public decimal Lat { get; set; }

            public decimal Lng { get; set; }
        }
    }
}
