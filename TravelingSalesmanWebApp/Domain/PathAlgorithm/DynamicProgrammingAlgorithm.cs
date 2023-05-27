using Path = TravelingSalesmanWebApp.Data.Models.Path;

namespace TravelingSalesmanWebApp.Domain.PathAlgorithm;


/// <summary>
/// Реализация алгоритма динамического программирования
/// Использует внутри себя алгоритм дейкстры, но почему-то косячит при каких-то из расчётов
/// </summary>
public class DynamicProgrammingAlgorithm : IPathAlgorithm
{
    private Dictionary<Guid, List<Tuple<Guid, double>>> _graph = new();

    public void UpdatePaths(List<Path> paths)
    {
        // Build a dictionary mapping each city to a list of its adjacent cities and the weight of the edge between them
        _graph = new Dictionary<Guid, List<Tuple<Guid, double>>>();
        foreach (var path in paths)
        {
            if (!_graph.ContainsKey(path.CityIds[0]))
            {
                _graph[path.CityIds[0]] = new List<Tuple<Guid, double>>();
            }

            if (!_graph.ContainsKey(path.CityIds[1]))
            {
                _graph[path.CityIds[1]] = new List<Tuple<Guid, double>>();
            }

            _graph[path.CityIds[0]].Add(new Tuple<Guid, double>(path.CityIds[1], path.Weight));
            _graph[path.CityIds[1]].Add(new Tuple<Guid, double>(path.CityIds[0], path.Weight));
        }
    }

    public List<Guid> FindShortestPath(Guid startCity, Guid endCity)
    {
        // Initialize a dictionary for storing the shortest distances from the start city to each city
        var shortestDistances = new Dictionary<Guid, double>();
        foreach (var city in _graph.Keys)
        {
            shortestDistances[city] = (city == startCity) ? 0.0 : double.MaxValue;
        }

        // Define a priority queue for vertices that we haven't explored yet
        var queue = new SortedSet<Tuple<double, Guid>>(
            Comparer<Tuple<double, Guid>>.Create((a, b) => a.Item1.CompareTo(b.Item1)));
        queue.Add(new Tuple<double, Guid>(0, startCity));

        // Visit each vertex in the priority queue and update the shortest distances to its neighbors
        while (queue.Any())
        {
            var currentVertex = queue.First().Item2;
            var currentDistance = queue.First().Item1;
            queue.Remove(queue.First());

            if (currentDistance > shortestDistances[currentVertex]) continue;

            foreach (var neighbor in _graph[currentVertex])
            {
                var distanceToNeighbor = currentDistance + neighbor.Item2;
                if (distanceToNeighbor < shortestDistances[neighbor.Item1])
                {
                    shortestDistances[neighbor.Item1] = distanceToNeighbor;
                    queue.Add(new Tuple<double, Guid>(distanceToNeighbor, neighbor.Item1));
                }
            }
        }

        // If there is no path from the start city to the end city, return null
        if (!shortestDistances.ContainsKey(endCity) || Math.Abs(shortestDistances[endCity] - double.MaxValue) < 1e-9)
        {
            return new List<Guid>();
        }

        // Build the shortest path from the start city to the end city by backtracking through the predecessor dictionary
        var shortestPath = new List<Guid>();
        var currentCity = endCity;
        while (currentCity != startCity)
        {
            shortestPath.Insert(0, currentCity);
            foreach (var neighbor in _graph[currentCity])
            {
                if (Math.Abs(shortestDistances[currentCity] - shortestDistances[neighbor.Item1] - neighbor.Item2) <
                    1e-9)
                {
                    currentCity = neighbor.Item1;
                    break;
                }
            }
        }

        shortestPath.Insert(0, startCity);
        return shortestPath;
    }
}