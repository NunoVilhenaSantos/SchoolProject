using System.Text.Json;
using SchoolProject.Web.Data.Entities.Countries;

namespace SchoolProject.Web.Data.Servives;

public static class CountryApi
{
    private const string ApiUrl = "https://restcountries.com/v3.1/all";


    private static async Task<List<City>?> GetAllCountries()
    {
        try
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(ApiUrl);

                if (!response.IsSuccessStatusCode)
                    return null;

                var json = await response.Content.ReadAsStringAsync();
                var countryData =
                    JsonSerializer.Deserialize<List<City>>(json);

                return countryData;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"Failed to retrieve country data: {ex.Message}");
            return null;
        }
    }
}