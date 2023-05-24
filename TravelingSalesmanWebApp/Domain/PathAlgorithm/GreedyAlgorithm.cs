using BlazorApp2.Data.Models;
using Path = BlazorApp2.Data.Models.Path;

namespace BlazorApp2.Domain.PathAlgorithm;

public class GreedyAlgorithm:IPathAlgorithm
{
    public Dictionary<Guid, int> FindShortestPath(City startCity, City endCity, List<Path> edges)
    {
        throw new NotImplementedException();
    }
}