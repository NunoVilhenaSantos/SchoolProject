using Microsoft.AspNetCore.Identity;
using SchoolProject.Web.Data.Entities;
using SchoolProject.Web.Helpers;

namespace SchoolProject.Web.Data.Seeders;

public static class SeedDbUsers
{
    private static IUserHelper _userHelper;


    internal static async Task AddUsers(
        string email, string address,
        string role, string password = "123456"
    )
    {
        var random = new Random();
        var userSplit = email.Split(
            '.', StringSplitOptions.RemoveEmptyEntries);

        var document = random.Next(100000, 999999999).ToString();
        var fixedPhone = random.Next(1000000, 99999999).ToString();
        var cellPhone = random.Next(1000000, 99999999).ToString();
        var addressFull = address + ", " + random.Next(1, 9999);

        await CheckUserAsync(
            userSplit[0], userSplit[1],
            email,
            email,
            cellPhone, role,
            document,
            addressFull, password
        );
    }


    internal static async Task<User> CheckUserAsync(
        string firstName, string lastName,
        string userName,
        string email,
        string phoneNumber, string role,
        string document, string address,
        string password = "123456")
    {
        var user = await _userHelper.GetUserByEmailAsync(email);

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
                        PhoneNumber = phoneNumber
                    },
                    "Teacher" => new User
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Address = address,
                        UserName = userName,
                        Email = email,
                        PhoneNumber = phoneNumber
                    },
                    "Functionary" => new User
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Address = address,
                        UserName = userName,
                        Email = email,
                        PhoneNumber = phoneNumber
                    },
                    "Admin" => new User
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Address = address,
                        UserName = userName,
                        Email = email,
                        PhoneNumber = phoneNumber
                    },
                    "SuperUser" => new User
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Address = address,
                        UserName = userName,
                        Email = email,
                        PhoneNumber = phoneNumber
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