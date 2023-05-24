using BlazorApp2.Data.Models;
using Path = BlazorApp2.Data.Models.Path;

namespace BlazorApp2.Domain.PathAlgorithm;

public class GreedyAlgorithm : IPathAlgorithm
{
    public Dictionary<Guid, int> FindShortestPath(City startCity, City endCity, List<Path> edges)
    {
        var distances = new Dictionary<Guid, int>();
        var previous = new Dictionary<Guid, Guid>();
        var cities = new List<City>();

        foreach (var edge in edges)
        {
            if (!distances.ContainsKey(edge.StartCityId))
            {
                distances.Add(edge.StartCityId, int.MaxValue);
            }

            if (!distances.ContainsKey(edge.EndCityId))
            {
                distances.Add(edge.EndCityId, int.MaxValue);
            }
        }

        distances[startCity.Id] = 0;

        for (var i = 0; i < cities.Count - 1; i++)
        {
            foreach (var edge in edges)
            {
                var newDistance = distances[edge.StartCityId] + edge.Weight;

                if (newDistance < distances[edge.EndCityId])
                {
                    distances[edge.EndCityId] = newDistance;
                    previous[edge.EndCityId] = edge.StartCityId;
                }
            }
        }

        foreach (var edge in edges)
        {
            if (distances[edge.StartCityId] + edge.Weight < distances[edge.EndCityId])
            {
                throw new Exception("Graph contains a negative-weight cycle");
            }
        }

        return distances;
    }
}