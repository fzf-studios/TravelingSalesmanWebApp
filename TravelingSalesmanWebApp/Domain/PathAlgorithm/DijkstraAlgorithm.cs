using Path = TravelingSalesmanWebApp.Data.Models.Path;

namespace TravelingSalesmanWebApp.Domain.PathAlgorithm;

/// <summary>
/// Реализация алгоритма Дейкстры 2
/// Немного отличается от первой, но по сути работает так же
/// </summary>
public class DijkstraAlgorithm : IPathAlgorithm
{
    private List<Path> edges = new();

    public List<Guid> FindShortestPath(Guid startCity, Guid endCity)
    {
        var path = new List<Guid>();
        var distances = new Dictionary<Guid, int>();
        var parents = new Dictionary<Guid, Guid>();
        var visited = new HashSet<Guid>();
        var queue = new PriorityQueue<Guid, int>();
        foreach (var edge in edges)
        {
            foreach (var city in edge.CityIds)
            {
                if (city == startCity)
                {
                    distances[city] = 0;
                }
                else
                {
                    distances[city] = int.MaxValue;
                }
            }
        }

        queue.Enqueue(startCity, 0);
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
            var currentCityEdges = edges.Where(e => e.CityIds.Contains(currentCity));
            foreach (var edge in currentCityEdges)
            {
                var otherCity = edge.CityIds.First(c => c != currentCity);
                var otherCityDistance = distances[currentCity] + edge.Weight;
                if (otherCityDistance < distances[otherCity])
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
            if (!parents.ContainsKey(current))
                break;

            current = parents[current];
        }

        path.Reverse();
        return path;
    }

    public void UpdatePaths(List<Path> paths)
    {
        edges = paths;
    }
}