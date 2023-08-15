using Microsoft.AspNetCore.Identity;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Helpers.Users;
using System.Text.RegularExpressions;

namespace SchoolProject.Web.Data.Seeders;

public class SeedDbUsers
{
    // Add a private field to hold the IUserHelper instance
    private static IUserHelper _userHelper;
    private static ILogger<SeedDbUsers> _logger;
    private static DataContextMySql _dataContextInUse;

    public SeedDbUsers(
        IUserHelper userHelper, ILogger<SeedDbUsers> logger
    )
    {
        _logger = logger;
        _userHelper = userHelper;
    }


    // Add a constructor to receive IUserHelper through dependency injection
    public static void Initialize(
        IUserHelper userHelper,
        ILogger<SeedDbUsers> logger, DataContextMySql dataContextInUse)
    {
        _logger = logger;
        _userHelper = userHelper;
        _dataContextInUse = dataContextInUse;
    }


    internal static async Task AddUsers(
        string firstName, string lastName,
        string email,
        string address,
        string role, string password = "Passw0rd"
    )
    {
        var random = new Random();
        var userSplit = email.Split(
            '.', StringSplitOptions.RemoveEmptyEntries);

        var document =
            random.Next(100_000_000, 999_999_999).ToString();
        var fixedPhone =
            random.Next(100_000_000, 999_999_999).ToString();
        var cellPhone =
            random.Next(100_000_000, 999_999_999).ToString();

        var fullAddress =
            address + ", " + random.Next(1, 9_999);

        var city =
            await _dataContextInUse.Cities
                .FirstOrDefaultAsync(c => c.Name == "Havana");
        var country =
            await _dataContextInUse.Countries
                .FirstOrDefaultAsync(c => c.Name == "Cuba");

        Console.WriteLine(
            $"Seeding the user {firstName} {lastName} with the email {email}");

        await VerifyUserAsync(
            firstName, lastName,
            email,
            email,
            cellPhone, role,
            document,
            fullAddress,
            city, country, password
        );
    }


    internal static async Task<User> VerifyUserAsync(
        string firstName, string lastName,
        string userName,
        string email,
        string phoneNumber, string role,
        string document,
        string address,
        City city, Country country,
        string password = "Passw0rd")
    {
        // Input validation
        if (string.IsNullOrWhiteSpace(firstName) ||
            string.IsNullOrWhiteSpace(lastName) ||
            string.IsNullOrWhiteSpace(userName) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(phoneNumber) ||
            string.IsNullOrWhiteSpace(role) ||
            string.IsNullOrWhiteSpace(address))
            throw new ArgumentException(
                "One or more required parameters are missing or empty.");


        // Validate email format
        if (!IsValidEmail(email))
            throw new ArgumentException("Invalid email format.");

        // Validate phone number format
        if (!IsValidPhoneNumber(phoneNumber))
            throw new ArgumentException("Invalid phone number format.");


        // Validate role
        var user = await _userHelper.GetUserByEmailAsync(email);
        string message;

        if (user == null)
        {
            // Create a new user with common properties set outside the switch
            var newUser = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Address = address,
                UserName = userName,
                Email = email,
                PhoneNumber = phoneNumber,
                WasDeleted = false
                // City = city,
                // CityId = city.Id,
                // Country = country,
                // CountryId = country.Id,
                // NationalityId = country.Nationality.Id,
            };

            // Set role-specific properties inside the switch
            switch (role)
            {
                case "SuperUser":
                case "Admin":
                case "Functionary":
                case "Student":
                case "Teacher":
                case "Parent":
                case "User":
                    // Add any role-specific properties here
                    break;

                default:
                    message = $"{nameof(User)} {firstName} {lastName} " +
                              $"with email {email} and role {role}, " +
                              "could not create the user in Seeder, " +
                              $"because the role {role} is not valid.";

                    Console.WriteLine(message);
                    _logger.LogError(message);
                    throw new InvalidOperationException(message);
            }


            // ------------------------------------------------------------ //

            // Create the user
            var result =
                await _userHelper.AddUserAsync(newUser, password);

            // Check if the user was created successfully
            if (result != IdentityResult.Success)
            {
                message = $"{nameof(User)} {firstName} {lastName} " +
                          $"with email {email} and role {role}, " +
                          "could not create the user in Seeder.";

                Console.WriteLine(message);
                _logger.LogError(message);
                throw new InvalidOperationException(message);
            }


            // ------------------------------------------------------------ //

            // Add the user to the role
            await _userHelper.AddUserToRoleAsync(newUser, role);

            // Check if the user was added to the role successfully
            var isInRole =
                await _userHelper.IsUserInRoleAsync(newUser, role);

            // Check if the user was added to the role successfully
            if (!isInRole)
            {
                message = $"{nameof(User)} {firstName} {lastName} " +
                          $"with email {email} and role {role}, " +
                          "could not create the user in Seeder.";

                Console.WriteLine(message);
                _logger.LogError(message);
                throw new InvalidOperationException(message);
            }


            // Log the user creation
            message = $"User {firstName} {lastName} " +
                      $"with email {email} and role {role} has been created.";


            Console.WriteLine(message);
            _logger.LogInformation(message);

            return newUser;
        }

        message = $"{nameof(User)} {firstName} {lastName} " +
                  $"with email {email} and role {role}, " +
                  "already exists.";

        Console.WriteLine(message);
        _logger.LogInformation(message);

        return user;
    }


    private static bool IsValidEmail(string email)
    {
        // Simple email format validation using regular expression
        // The chosen regular expression may not cover all edge cases,
        // but it's a good starting point.

        const string emailPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$";

        return Regex.IsMatch(email, emailPattern);
    }

    private static bool IsValidPhoneNumber(string phoneNumber)
    {
        // Simple phone number format validation using regular expression
        // Depending on the specific format required,
        // a more comprehensive regex pattern can be used.

        // const string phonePattern = @"^\+\d{9,13}$";
        const string phonePattern = @"^\d{9,13}$";

        return Regex.IsMatch(phoneNumber, phonePattern);
    }
}