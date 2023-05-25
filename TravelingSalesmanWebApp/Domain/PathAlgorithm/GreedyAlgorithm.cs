using Path = TravelingSalesmanWebApp.Data.Models.Path;

namespace TravelingSalesmanWebApp.Domain.PathAlgorithm;

public class GreedyAlgorithm : IPathAlgorithm
{
    public Dictionary<Guid, int> FindShortestPath(Guid startCityId, Guid endCityId, List<Path> edges)
    {
        var distances = new Dictionary<Guid, int>();
        var previous = new Dictionary<Guid, Guid>();

        foreach (var edge in edges)
        {
            distances.TryAdd(edge.StartCityId, int.MaxValue);
            distances.TryAdd(edge.EndCityId, int.MaxValue);
        }

        distances[startCityId] = 0;

        foreach (var edge in edges)
        {
            var newDistance = distances[edge.StartCityId] + edge.Weight;

            if (newDistance < distances[edge.EndCityId])
            {
                distances[edge.EndCityId] = newDistance;
                previous[edge.EndCityId] = edge.StartCityId;
            }
        }

        return distances.ToDictionary(x => x.Key, x => x.Value);
    }

}