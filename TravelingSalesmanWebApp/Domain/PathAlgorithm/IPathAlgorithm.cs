using BlazorApp2.Data.Models;
using Path = BlazorApp2.Data.Models.Path;

namespace BlazorApp2.Domain.PathAlgorithm;

public interface IPathAlgorithm
{
    Dictionary<Guid, int> FindShortestPath(City startCity, City endCity, List<Path> edges);
}