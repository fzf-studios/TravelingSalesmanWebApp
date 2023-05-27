namespace TravelingSalesmanWebApp.Data.Models;

public struct ShortestPathModel
{
    public City[] Cities { get; private set; }
    public Path[] Paths { get; private set; }

    public ShortestPathModel(City[] cities, Path[] paths)
    {
        Cities = cities;
        Paths = paths;
    }
}