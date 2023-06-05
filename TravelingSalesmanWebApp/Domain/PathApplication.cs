using TravelingSalesmanWebApp.Data;
using TravelingSalesmanWebApp.Data.Models;
using TravelingSalesmanWebApp.Domain.PathAlgorithm;
using TravelingSalesmanWebApp.Domain.Services;
using Path = TravelingSalesmanWebApp.Data.Models.Path;

namespace TravelingSalesmanWebApp.Domain;

public interface IPathApplication
{
    ShortestPathModel GetShortestPath(Guid startId, Guid endId);
    void UpdateData();
}

public class PathApplication : IPathApplication
{
    private readonly ApplicationDBContext _context;
    private readonly IUserSettingsRepository _userSettingsRepository;

    private IPathAlgorithm _pathAlgorithm;
    

    public PathApplication(ApplicationDBContext context, IPathAlgorithm pathAlgorithm)
    {
        _context = context;
        _pathAlgorithm = pathAlgorithm;
    }

    /// <summary>
    /// Обновление путей у алгоритма при изменении выбранного алгоритма, либо при входе на страницу
    /// Необходим для метода динамического программирования, так как он считает пути при заранее (наверное)
    /// </summary>
    public void UpdateData()
    {
        var allPaths = _context.Paths.ToList();
        _pathAlgorithm.UpdatePaths(allPaths);
    }

    public ShortestPathModel GetShortestPath(Guid startId, Guid endId)
    {
        var shortestPath = _pathAlgorithm.FindShortestPath(startId, endId);
        if (shortestPath.Count == 0)
            return ShortestPathModel.Empty;
        
        var cities = shortestPath.Select(GetCityById).ToArray();

        var paths = new List<Path>();

        for (int i = 0; i < cities.Length - 1; i++)
        {
            var pathBetweenCities = GetPath(cities[i].Id, cities[i + 1].Id);
            paths.Add(pathBetweenCities);
        }

        return new ShortestPathModel(cities, paths.ToArray());
    }

    private City GetCityById(Guid Id)
    {
        var endCity = _context.Cities.First(city => city.Id == Id);
        return endCity;
    }

    private Path GetPath(Guid startId, Guid endId)
    {
        var paths = _context.Paths.ToArray();
        return paths.First(path => path.CityIds.Contains(startId) && path.CityIds.Contains(endId));
    }
    
}