using BlazorApp2.Data.Models;
using Path = BlazorApp2.Data.Models.Path;

namespace BlazorApp2.Domain.PathAlgorithm;

public class BellmanFordAlgorithm: IPathAlgorithm
{
    public Dictionary<Guid, int> FindShortestPath(City startCity, City endCity, List<Path> edges)
        {
            Dictionary<Guid, int> distances = new Dictionary<Guid, int>();
            Dictionary<Guid, Guid> previousCities = new Dictionary<Guid, Guid>();

            var allCities = edges.SelectMany(edge => new[] { edge.StartCityId, edge.EndCityId }).Distinct();

            foreach (var city in allCities)
            {
                distances[city] = city == startCity.Id ? 0 : int.MaxValue;
            }

            for (int i = 0; i < allCities.Count() - 1; i++)
            {
                foreach (var edge in edges)
                {
                    var startCityId = edge.StartCityId;
                    var endCityId = edge.EndCityId;
                    var weight = edge.Weight;

                    if (distances[startCityId] != int.MaxValue && distances[startCityId] + weight < distances[endCityId])
                    {
                        distances[endCityId] = distances[startCityId] + weight;
                        previousCities[endCityId] = startCityId;
                    }

                    if (distances[endCityId] != int.MaxValue && distances[endCityId] + weight < distances[startCityId])
                    {
                        distances[startCityId] = distances[endCityId] + weight;
                        previousCities[startCityId] = endCityId;
                    }
                }
            }

            foreach (var edge in edges)
            {
                var startCityId = edge.StartCityId;
                var endCityId = edge.EndCityId;
                var weight = edge.Weight;

                if (distances[startCityId] != int.MaxValue && distances[startCityId] + weight < distances[endCityId])
                {
                    throw new Exception("Negative cycle detected. The graph contains a negative-weight cycle.");
                }

                if (distances[endCityId] != int.MaxValue && distances[endCityId] + weight < distances[startCityId])
                {
                    throw new Exception("Negative cycle detected. The graph contains a negative-weight cycle.");
                }
            }

            return distances;
        }
}