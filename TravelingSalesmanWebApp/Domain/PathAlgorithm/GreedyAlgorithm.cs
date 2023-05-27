using TravelingSalesmanWebApp.Data.Models;
using Path = TravelingSalesmanWebApp.Data.Models.Path;

namespace TravelingSalesmanWebApp.Domain.PathAlgorithm;

public class GreedyAlgorithm : IPathAlgorithm
{
    public List<Guid> FindShortestPath(Guid startCity, Guid endCity, List<Path> edges)
    {
        var path = new List<Guid>();
        var distances = new Dictionary<Guid, int>();
        var parents = new Dictionary<Guid, Guid>();
        var visited = new HashSet<Guid>();
        var queue = new PriorityQueue<Guid, int>();
        queue.Enqueue(startCity, 0);
        distances[startCity] = 0;
        parents[startCity] = Guid.Empty;
        while (queue.Count > 0)
        {
            var currentCity = queue.Dequeue();
            if (currentCity == endCity)
            {
                break;
            }
            if (visited.Contains(currentCity))
            {
                continue;
            }
            visited.Add(currentCity);
            var currentCityDistance = distances[currentCity];
            var currentCityEdges = edges.Where(e => e.CityIds.Contains(currentCity));
            foreach (var edge in currentCityEdges)
            {
                var otherCity = edge.CityIds.First(c => c != currentCity);
                var otherCityDistance = currentCityDistance + edge.Weight;
                if (!distances.ContainsKey(otherCity) || otherCityDistance < distances[otherCity])
                {
                    distances[otherCity] = otherCityDistance;
                    parents[otherCity] = currentCity;
                    queue.Enqueue(otherCity, otherCityDistance);
                }
            }
        }
        var current = endCity;
        while (current != Guid.Empty)
        {
            path.Add(current);
            current = parents[current];
        }
        path.Reverse();
        return path;
    }
}