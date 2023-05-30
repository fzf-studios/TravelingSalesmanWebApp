using Path = TravelingSalesmanWebApp.Data.Models.Path;

namespace TravelingSalesmanWebApp.Domain.PathAlgorithm;

/// <summary>
/// Реализация алгоритма ветвей и границ
/// Выглядит иначе, но работает похоже на алгоритм Дейкстры
/// (считает правильно, но через рекурсию и я не уверен, что это реально алгоритм ветвей и границ)
/// Из-за рекурсии при первом расчёте может пролагать на пару секунд
/// </summary>
public class BranchAndBoundAlgorithm : IPathAlgorithm
{
    private Dictionary<Guid, List<Tuple<Guid, double>>> _graph;
    private List<Guid> _shortestPath;
    private double _shortestDistance;

    public List<Guid> FindShortestPath(Guid startCity, Guid endCity)
    {
        _shortestPath = new List<Guid>();
        _shortestDistance = double.MaxValue;

        var visited = new HashSet<Guid>();
        visited.Add(startCity);

        BranchAndBound(startCity, endCity, visited, 0, new List<Guid>() { startCity });

        return _shortestPath;
    }

    private void BranchAndBound(Guid currentCity, Guid endCity, HashSet<Guid> visited, double currentDistance, List<Guid> currentPath)
    {
        if (currentCity == endCity)
        {
            if (currentDistance < _shortestDistance)
            {
                _shortestDistance = currentDistance;
                _shortestPath = new List<Guid>(currentPath);
            }
            return;
        }

        foreach (var neighbor in _graph[currentCity])
        {
            if (!visited.Contains(neighbor.Item1))
            {
                double potentialDistance = currentDistance + neighbor.Item2;

                if (potentialDistance < _shortestDistance)
                {
                    visited.Add(neighbor.Item1);
                    currentPath.Add(neighbor.Item1);

                    BranchAndBound(neighbor.Item1, endCity, visited, potentialDistance, currentPath);

                    visited.Remove(neighbor.Item1);
                    currentPath.RemoveAt(currentPath.Count - 1);
                }
            }
        }
    }

    public void UpdatePaths(List<Path> paths)
    {
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
}