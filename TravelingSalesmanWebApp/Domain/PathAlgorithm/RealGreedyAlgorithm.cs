using Path = TravelingSalesmanWebApp.Data.Models.Path;

namespace TravelingSalesmanWebApp.Domain.PathAlgorithm;

/// <summary>
/// Реализация жадного алгоритма
/// Ищет по минимальному пути на каждом конкретном шаге и поэтому почти всегда будет выдавать, что не нашёл город
/// Вполне возможно, что будет выдавать оптимум, если у нас каждый город будет связан с каждым другим
/// </summary>
public class RealGreedyAlgorithm : IPathAlgorithm
{
    private List<Path> edges = new();

    public List<Guid> FindShortestPath(Guid startCity, Guid endCity)
    {
        var path = new List<Guid>();
        var currentCity = startCity;
        path.Add(currentCity);
        while (currentCity != endCity)
        {
            var currentCityEdges = edges.Where(e => e.CityIds.Contains(currentCity));
            var shortestEdge = currentCityEdges.MinBy(e => e.Weight);

            if (shortestEdge == null)
                return new List<Guid>();

            var otherCity = shortestEdge.CityIds.First(c => c != currentCity);

            if (path.Contains(otherCity))
                return new List<Guid>();

            path.Add(otherCity);
            currentCity = otherCity;
        }

        return path;
    }

    public void UpdatePaths(List<Path> paths)
    {
        edges = paths;
    }
}