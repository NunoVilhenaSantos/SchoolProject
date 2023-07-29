using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Crypto.Generators;
using SchoolProject.Web.Data.Entities.ExtraEntities;
using SchoolProject.Web.Helpers.Users;
using Serilog.Core;


namespace SchoolProject.Web.Data.Seeders;

public class SeedDbUsers
{
    // Add a private field to hold the IUserHelper instance
    private static IUserHelper _userHelper;
    private static ILogger<SeedDbUsers> _logger;


    // Add a constructor to receive IUserHelper through dependency injection
    public static void Initialize(
        IUserHelper userHelper, ILogger<SeedDbUsers> logger
    )
    {
        _logger = logger;
        _userHelper = userHelper;
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

        Console.WriteLine(
            $"Seeding the user {firstName} {lastName} with the email {email}");

        await VerifyUserAsync(
            firstName, lastName,
            email,
            email,
            cellPhone, role,
            document,
            fullAddress, password
        );
    }


    internal static async Task<User> VerifyUserAsync(
        string firstName, string lastName,
        string userName,
        string email,
        string phoneNumber, string role,
        string document,
        string address,
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
        {
            throw new ArgumentException(
                "One or more required parameters are missing or empty.");
        }

        // Additional validation for email format and
        // phone number format can be performed here.

        // Validate email format
        if (!IsValidEmail(email))
        {
            throw new ArgumentException("Invalid email format.");
        }

        // Validate phone number format
        if (!IsValidPhoneNumber(phoneNumber))
        {
            throw new ArgumentException("Invalid phone number format.");
        }


        // Validate role
        var user = await _userHelper.GetUserByEmailAsync(email);


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
                    _logger.LogError(
                        $"{nameof(User)} {firstName} {lastName} " +
                        $"with email {email} and role {role}, " +
                        "could not create the user in Seeder, " +
                        $"because the role {role} is not valid.");

                    throw new InvalidOperationException(
                        $"The role {role} is not valid.");
            }

            // Hash the password before storing it
            // var hashedPassword = HashPassword(password);
            // var result =
            //     await _userHelper.AddUserAsync(newUser, hashedPassword);

            var result =
                await _userHelper.AddUserAsync(newUser, password);


            if (result != IdentityResult.Success)
            {
                _logger.LogError(
                    $"{nameof(User)} {firstName} {lastName} " +
                    $"with email {email} and role {role}, " +
                    "could not create the user in Seeder.");

                throw new InvalidOperationException(
                    "Could not create the user in Seeder.");
            }

            // Log the user creation
            _logger.LogInformation(
                $"User {firstName} {lastName} " +
                $"with email {email} and role {role} has been created.");

            return newUser;
        }
        else
        {
            // Log that the user already exists
            _logger.LogInformation(
                $"User with email {email} already exists.");
            return user;
        }
    }


    private static bool IsValidEmail(string email)
    {
        // Simple email format validation using regular expression
        // This is a basic example,
        // and a more comprehensive validation can be used in a real application.
        // The chosen regular expression may not cover all edge cases,
        // but it's a good starting point.

        const string emailPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$";

        return Regex.IsMatch(email, emailPattern);
    }

    private static bool IsValidPhoneNumber(string phoneNumber)
    {
        // Simple phone number format validation using regular expression
        // This is a basic example,
        // and the chosen regular expression may not cover all phone number formats.
        // Depending on the specific format required,
        // a more comprehensive regex pattern can be used.

        // const string phonePattern = @"^\+\d{9,13}$";
        const string phonePattern = @"^\d{9,13}$";

        return Regex.IsMatch(phoneNumber, phonePattern);
    }
}