using TravelingSalesmanWebApp.Data;
using TravelingSalesmanWebApp.Data.Models;
using TravelingSalesmanWebApp.Domain.PathAlgorithm;
using TravelingSalesmanWebApp.Domain.PathAlgorithm.Enum;
using TravelingSalesmanWebApp.Domain.Services;
using Path = TravelingSalesmanWebApp.Data.Models.Path;

namespace TravelingSalesmanWebApp.Domain;

public interface IPathApplication
{
    ShortestPathModel GetShortestPath(Guid startId, Guid endId);
    void UpdateData();
}

public class PathApplication : IPathApplication, IDisposable
{
    private readonly ApplicationDBContext _context;
    private readonly IUserSettingsRepository _userSettingsRepository;

    private IPathAlgorithm _selectedPathAlgorithm;

    /// <summary>
    /// Словарь алгоритмов, где ключ - тип алгоритма, а значение - сама реализация
    /// </summary>
    private readonly Dictionary<AlgorithmType, IPathAlgorithm> _pathAlgorithms = new()
    {
        { AlgorithmType.Greedy, new RealGreedyAlgorithm() },
        { AlgorithmType.Dijkstra, new GreedyAlgorithm() }, //TODO: fix, GreedyAlgorithm and DejkstraAlgorithm are same
        { AlgorithmType.DynamicProgramming, new DynamicProgrammingAlgorithm() },
        { AlgorithmType.BranchAndBound, new BranchAndBoundAlgorithm() }
    };

    public PathApplication(ApplicationDBContext context, IUserSettingsRepository userSettingsRepository)
    {
        _context = context;
        _userSettingsRepository = userSettingsRepository;
        _userSettingsRepository.OnStateChanged += OnUserSettingsChanged;
        _selectedPathAlgorithm = _pathAlgorithms[_userSettingsRepository.LastSelectedAlgorithm];
    }

    /// <summary>
    /// Обновление путей у алгоритма при изменении выбранного алгоритма, либо при входе на страницу
    /// Необходим для метода динамического программирования, так как он считает пути при заранее (наверное)
    /// </summary>
    public void UpdateData()
    {
        var allPaths = _context.Paths.ToList();
        _selectedPathAlgorithm.UpdatePaths(allPaths);
    }

    public ShortestPathModel GetShortestPath(Guid startId, Guid endId)
    {
        var shortestPath = _selectedPathAlgorithm.FindShortestPath(startId, endId);
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
    
    private void OnUserSettingsChanged()
    {
        _selectedPathAlgorithm = _pathAlgorithms[_userSettingsRepository.LastSelectedAlgorithm];
        UpdateData();
    }
    
    public void Dispose()
    {
        _userSettingsRepository.OnStateChanged -= OnUserSettingsChanged;
    }
}