using DgpaMapWebApi.Dtos;
using DgpaMapWebApi.Models;

namespace DgpaMapWebApi.Dto
{
    public class FeatureCollection
    {
        public string Type { get; set; } = "FeatureCollection";
        public List<Feature> Features { get; set; } = new();
    }


    public class Feature
    {
        public string Type { get; set; } = "Feature";
        public Geometry Geometry { get; set; }
        public Guid Id { get; set; }
        public JobDto Properties { get; set; }
    }

    public class Geometry
    {
        public string Type { get; set; } = "Point";
        public decimal?[] Coordinates { get; set; }
    }
}
