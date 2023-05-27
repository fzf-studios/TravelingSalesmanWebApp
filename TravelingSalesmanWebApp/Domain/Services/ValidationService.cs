using System.Text.RegularExpressions;

namespace TravelingSalesmanWebApp.Data.Services;

public interface IValidationService
{
    public bool IsValidCityName(string cityName, out string reason);
}

public partial class ValidationService : IValidationService
{
    public bool IsValidCityName(string cityName, out string reason)
    {
        reason = string.Empty;
        if (string.IsNullOrEmpty(cityName))
        {
            reason = "Пустая строка";
            return false;
        }

        if (cityName.Any(char.IsDigit))
        {
            reason = "Название города не может содержать цифр";
            return false;
        }

        return true;
    }
}