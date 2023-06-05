namespace TravelingSalesmanWebApp.Data.Models;

public struct ShortestPathModel
{
    public static readonly ShortestPathModel Empty = new();
    public City[] Cities { get; private set; }
    public Path[] Paths { get; private set; }

    public bool IsEmpty => Cities == null 
                           && Paths == null;
    
    public ShortestPathModel(City[] cities, Path[] paths)
    {
        Cities = cities;
        Paths = paths;
    }
}