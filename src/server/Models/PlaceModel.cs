using V.TouristGuide.Server.Entities;

namespace V.TouristGuide.Server.Models
{
    public class PlaceModel : Place
    {
        public double Distance { get; set; }

        public PlaceModel() { }

        public PlaceModel(Place p)
        {
            this.Id= p.Id;
            this.Name = p.Name;
            this.Cover = p.Cover;
            this.OpenTime = p.OpenTime;
            this.Address = p.Address;
            this.Lng = p.Lng;
            this.Lat = p.Lat;
            this.Tel = p.Tel;
            this.Type = p.Type;
            this.Visibility = p.Visibility;
            this.CreateTime = p.CreateTime;
            this.UpdateTime = p.UpdateTime;
            this.Region = p.Region;
        }
    }
}
