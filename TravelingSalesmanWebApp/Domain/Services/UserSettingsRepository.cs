using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MudBlazor;
using TravelingSalesmanWebApp.Data.Models;

namespace TravelingSalesmanWebApp.Domain.Services;

public interface IUserSettingsRepository
{
    public bool DarkMode { get; set; }
    bool DrawerOpened { get; set; }
    bool AutoSaveEnabled { get; set; }
    bool CityIdHidden { get; set; }
    string LastEnteredCityName { get; set; }
    public Task LoadLastStateAsync();
}

public class UserSettingsRepository : IUserSettingsRepository
{
    private readonly ProtectedLocalStorage _protectedLocalStorage;
    private static readonly string UserSettingsKey = "UserSettings";
    private UserSettings _settings;

    public UserSettingsRepository(ProtectedLocalStorage protectedLocalStorage)
    {
        _protectedLocalStorage = protectedLocalStorage;
        _settings = new();
    }

    public bool DarkMode
    {
        get => _settings.IsDarkModeEnabled;
        set
        {
            _settings.IsDarkModeEnabled = value;
            SaveLastStateAsync().AndForget();
        }
    }

    public bool DrawerOpened
    {
        get => _settings.IsDrawerOpened;
        set
        {
            _settings.IsDrawerOpened = value;
            SaveLastStateAsync().AndForget();
        }
    }
    
    public bool AutoSaveEnabled
    {
        get => _settings.IsAutoSaveEnabled;
        set
        {
            _settings.IsAutoSaveEnabled = value;
            SaveLastStateAsync().AndForget();
        }
    }
    
    public bool CityIdHidden
    {
        get => _settings.IsCityIdHidden;
        set
        {
            _settings.IsCityIdHidden = value;
            SaveLastStateAsync().AndForget();
        }
    }
    
    public string LastEnteredCityName
    {
        get => _settings.LastEnteredCityName;
        set
        {
            _settings.LastEnteredCityName = value;
            SaveLastStateAsync().AndForget();
        }
    }

    public async Task LoadLastStateAsync()
    {
        var result = await _protectedLocalStorage.GetAsync<UserSettings>(UserSettingsKey);
        
        _settings = result.Success
            ? result.Value?? new ()
            : new();
    }

    private async Task SaveLastStateAsync()
    {
        await _protectedLocalStorage.SetAsync(UserSettingsKey, _settings);
    }
}