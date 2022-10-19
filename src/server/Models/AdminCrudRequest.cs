namespace V.TouristGuide.Server.Models
{
    public class AdminCrudRequest
    {
        public int Page { get; set; }

        public int PerPage { get; set; }

        public string OrderBy { get; set; }

        /// <summary>
        /// asc 为升序，desc 为降序
        /// </summary>
        public string OrderDir { get; set; }
    }
}
