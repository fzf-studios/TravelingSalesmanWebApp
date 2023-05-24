using TravelingSalesmanWebApp.Data.Models;
using Path = TravelingSalesmanWebApp.Data.Models.Path;

namespace TravelingSalesmanWebApp.Domain.PathAlgorithm;

public interface IPathAlgorithm
{
    Dictionary<Guid, int> FindShortestPath(Guid startCity, Guid endCity, List<Path> edges);
}