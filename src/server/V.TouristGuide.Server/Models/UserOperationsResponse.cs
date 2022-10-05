namespace V.TouristGuide.Server.Models
{
    public class UserOperationsResponse
    {
        public List<Operation> Collections { get; set; }

        public List<Operation> Comments { get; set; }

        public class Operation
        {
            public long Id { get; set; }

            public long PlaceId { get; set; }

            public string Name { get; set; }

            public string Cover { get; set; }

            public string Remark { get; set; }

            public string UpdateTime { get; set; }
        }
    }
}
