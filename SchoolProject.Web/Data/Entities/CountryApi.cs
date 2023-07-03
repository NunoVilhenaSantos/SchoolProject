using System.Text.Json;

namespace SchoolProject.Web.Data.Entities;

public static class CountryApi
{
    private const string ApiUrl = "https://restcountries.com/v3.1/all";
    public static List<CountryData>? countryList { get; set; }


    private static async Task<List<CountryData>?> GetAllCountries()
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
        countryList = await GetAllCountries();

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


    public static async Task PopulateCountriesEnum()
    {
        //var countryNames = CountryData;

        if (countryList != null)
            foreach (var country in countryList)
            {
                Countries countryEnum;
                if (Enum.TryParse(country.Name, out countryEnum))
                {
                    // Enum value successfully parsed from country name
                    // Use countryEnum as needed
                }
                // Handle parsing failure
            }
        else
            Console.WriteLine("Failed to retrieve the list of country names.");
    }
}