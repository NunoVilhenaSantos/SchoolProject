using System.Text.Json;


namespace SchoolProject.Web.Data.Entities;

public class CountryData
{
    public string Name { get; set; }
    public bool Independent { get; set; }
}

public static class CountryApi
{
    private const string ApiUrl = "https://restcountries.com/v3.1/all";


    public static async Task<List<CountryData>?> GetAllCountries()
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
                    JsonSerializer.Deserialize<List<CountryData>>(json);

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


    public static async Task EnumerateCountries()
    {
        var countryList = await GetAllCountries();

        if (countryList != null)
            foreach (var country in countryList)
            {
                Console.WriteLine($"Name: {country.Name}");
                Console.WriteLine(
                    $"Independent: {(country.Independent ? "Yes" : "No")}");
                Console.WriteLine();
            }
        else
            Console.WriteLine("Failed to retrieve the list of countries.");
    }
}