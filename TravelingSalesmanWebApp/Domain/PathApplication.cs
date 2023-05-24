using System.Data.SqlTypes;
using BlazorApp2.Data;
using BlazorApp2.Data.Models;
using BlazorApp2.Domain.PathAlgorithm;

namespace BlazorApp2.Domain;

public interface IPathApplication
{
    Dictionary<City, int> GetShortestPath(Guid startId, Guid endId);
}

public class PathApplication:IPathApplication
{
    private readonly ApplicationDBContext _context;

    private readonly IPathAlgorithm _pathAlgorithm;
    
    public PathApplication(ApplicationDBContext context)
    {
        _context = context;
        _pathAlgorithm = new GreedyAlgorithm(); //BellmanFordAlgorithm();
    }
    public Dictionary<City, int> GetShortestPath(Guid startId, Guid endId)
    {
        var startCity = GetCityById(startId);
        var endCity = GetCityById(endId);
        var shortestPath = _pathAlgorithm.FindShortestPath(startCity, endCity, _context.Paths.ToList());
        return shortestPath
            .ToDictionary(pair => GetCityById(pair.Key), pair => pair.Value);
    }

    private City GetCityById(Guid Id)
    {
        var endCity = _context.Cities.First(city => city.Id == Id);
        return endCity;
    }
}