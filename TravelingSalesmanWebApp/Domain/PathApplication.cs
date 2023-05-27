using TravelingSalesmanWebApp.Data;
using TravelingSalesmanWebApp.Data.Models;
using TravelingSalesmanWebApp.Domain.PathAlgorithm;
using Path = TravelingSalesmanWebApp.Data.Models.Path;

namespace TravelingSalesmanWebApp.Domain;

public interface IPathApplication
{
    ShortestPathModel GetShortestPath(Guid startId, Guid endId);
}

public class PathApplication:IPathApplication
{
    private readonly ApplicationDBContext _context;

    private readonly IPathAlgorithm _pathAlgorithm;
    
    public PathApplication(ApplicationDBContext context)
    {
        _context = context;
        _pathAlgorithm = new GreedyAlgorithm();
    }
    public ShortestPathModel GetShortestPath(Guid startId, Guid endId)
    {
        var allPaths = _context.Paths.ToList();
        
        var shortestPath = _pathAlgorithm.FindShortestPath(startId, endId, allPaths);
        var cities = shortestPath.Select(GetCityById).ToArray();
        
        var paths = new List<Path>();

        for (int i = 0; i < cities.Length-1; i++)
        {
            var pathBetweenCities = GetPath(cities[i].Id, cities[i + 1].Id);
            paths.Add(pathBetweenCities);
        }
        
        return new ShortestPathModel(cities, paths.ToArray());
    }

    private City GetCityById(Guid Id)
    {
        var endCity = _context.Cities.First(city => city.Id == Id);
        return endCity;
    }
    
    private Path GetPath(Guid startId, Guid endId)
    {
        var paths = _context.Paths.ToArray();
        return paths.First(path => path.CityIds.Contains(startId) && path.CityIds.Contains(endId));
    }
}