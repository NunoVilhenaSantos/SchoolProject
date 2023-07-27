using Microsoft.AspNetCore.Identity;
using SchoolProject.Web.Data.Entities.ExtraEntities;
using SchoolProject.Web.Helpers.Users;

namespace SchoolProject.Web.Data.Seeders;

public static class SeedDbUsers
{
    // Add a private field to hold the IUserHelper instance
    private static IUserHelper _userHelper;

    // Add a constructor to receive IUserHelper through dependency injection
    public static void Initialize(IUserHelper userHelper)
    {
        _userHelper = userHelper;
    }


    internal static async Task AddUsers(
        string firstName, string lastName,
        string email,
        string address,
        string role, string password = "123456"
    )
    {
        var random = new Random();
        var userSplit = email.Split(
            '.', StringSplitOptions.RemoveEmptyEntries);

        var document = random.Next(100000, 999999999).ToString();
        var fixedPhone = random.Next(1000000, 99999999).ToString();
        var cellPhone = random.Next(1000000, 99999999).ToString();
        var fullAddress = address + ", " + random.Next(1, 9999);

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
        string password = "123456")
    {
        var user = await _userHelper.GetUserByEmailAsync(email);

        Console.WriteLine(
            $"Seeding the user {firstName} {lastName} " +
            $"with the email {email} and the role {role}.");

        switch (user)
        {
            case null:
            {
                user = role switch
                {
                    "Student" => new User
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Address = address,
                        UserName = userName,
                        Email = email,
                        PhoneNumber = phoneNumber,
                        WasDeleted = false
                    },
                    "Teacher" => new User
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Address = address,
                        UserName = userName,
                        Email = email,
                        PhoneNumber = phoneNumber,
                        WasDeleted = false
                    },
                    "Functionary" => new User
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Address = address,
                        UserName = userName,
                        Email = email,
                        PhoneNumber = phoneNumber,
                        WasDeleted = false
                    },
                    "Admin" => new User
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Address = address,
                        UserName = userName,
                        Email = email,
                        PhoneNumber = phoneNumber,
                        WasDeleted = false
                    },
                    "SuperUser" => new User
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Address = address,
                        UserName = userName,
                        Email = email,
                        PhoneNumber = phoneNumber,
                        WasDeleted = false
                    },
                    _ => throw new InvalidOperationException(
                        "The role is not valid")
                };

                var result = await _userHelper.AddUserAsync(user, password);

                if (result != IdentityResult.Success)
                    throw new InvalidOperationException(
                        "Could not create the user in Seeder");
                break;
            }
        }

        return user;
    }
}