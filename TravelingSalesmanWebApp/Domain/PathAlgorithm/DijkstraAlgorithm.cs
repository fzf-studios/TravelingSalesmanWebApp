using BlazorApp2.Data.Models;
using Path = BlazorApp2.Data.Models.Path;

namespace BlazorApp2.Domain.PathAlgorithm;

public class DijkstraAlgorithm : IPathAlgorithm
    {
        public Dictionary<Guid, int> FindShortestPath(City startCity, City endCity, List<Path> edges)
        {
            Dictionary<Guid, int> distances = new Dictionary<Guid, int>();
            Dictionary<Guid, Guid> previousCities = new Dictionary<Guid, Guid>();
            HashSet<Guid> visitedCities = new HashSet<Guid>();

            distances = edges
                .SelectMany(edge => new[] { edge.StartCityId, edge.EndCityId })
                .Distinct()
                .ToDictionary(id => id, _ => Int32.MaxValue);
            distances[startCity.Id] = 0;

            while (true)
            {
                var currentCity = GetNextCity(distances, visitedCities);
                if (currentCity == Guid.Empty || currentCity == endCity.Id)
                {
                    break;
                }

                visitedCities.Add(currentCity);

                foreach (var edge in edges.Where(e => e.StartCityId == currentCity))
                {
                    var neighborCity = edge.EndCityId;
                    var totalDistance = (long)distances[currentCity] + edge.Weight;

                    if (totalDistance < distances[neighborCity])
                    {
                        distances[neighborCity] = (int)totalDistance;
                        previousCities[neighborCity] = currentCity;
                    }
                }
            }

            return distances;
        }

        private Guid GetNextCity(Dictionary<Guid, int> distances, HashSet<Guid> visitedCities)
        {
            var minDistance = int.MaxValue;
            Guid nextCity = Guid.Empty;

            foreach (var kvp in distances)
            {
                var city = kvp.Key;
                var distance = kvp.Value;

                if (distance < minDistance && !visitedCities.Contains(city))
                {
                    minDistance = distance;
                    nextCity = city;
                }
            }

            return nextCity;
        }
    }