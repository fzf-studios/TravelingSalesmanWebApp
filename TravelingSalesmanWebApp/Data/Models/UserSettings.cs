namespace TravelingSalesmanWebApp.Data.Models;

public class UserSettings
{
    public bool IsDarkModeEnabled { get; set; }
    public bool IsDrawerOpened { get; set; }
    public bool IsAutoSaveEnabled { get; set; }
    public bool IsCityIdHidden { get; set; } = true;
    public string LastEnteredCityName { get; set; } = string.Empty;
}