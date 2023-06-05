namespace TravelingSalesmanWebApp.Data.Models;

public class Path : BaseModel
{
    public Guid StartCityId { get; set; }
    public Guid EndCityId { get; set; }
    public int Weight { get; set; }
    public List<Guid> CityIds => new() { StartCityId, EndCityId };
}