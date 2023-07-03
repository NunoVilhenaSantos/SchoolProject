using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.Entities;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Entities.Teachers;
using SchoolProject.Web.Helpers;

namespace SchoolProject.Web.Data.Seeders;

public static class SeedDbPersons
{
    private static Random _random;
    private static IUserHelper _userHelper;
    private static DataContextMssql _dataContextMssql;


    internal static async Task AddingData()
    {
        var user = await _userHelper.GetUserByEmailAsync(
            "nuno.santos.26288@formandos.cinel.pt");


        if (!_dataContextMssql.Students.Any())
        {
            await AddStudent(
                "Juan", "Zuluaga",
                "Calle Luna", user);
            await AddStudent(
                "Joaquim", "Alvenaria",
                "Calle Sol", user);
            await AddStudent(
                "Alberto", "Domingues",
                "Calle Luna", user);
            await AddStudent(
                "Mariana", "Alvarez",
                "Calle Sol", user);
            await AddStudent(
                "Lucia", "Liu",
                "Calle Luna", user);
            await AddStudent(
                "Renee", "Arriaga",
                "Calle Sol", user);
            await AddStudent(
                "Marcia", "Zuluaga",
                "Calle Luna", user);
            await AddStudent(
                "Ernesto", "Guevara",
                "Calle Sol", user);
            await AddStudent(
                "El", "Che",
                "Calle Luna", user);
            await AddStudent(
                "Claudia", "Arroz",
                "Calle Sol", user);
        }

        user = await _userHelper.GetUserByEmailAsync(
            "nuno.santos.26288@formandos.cinel.pt");


        if (!_dataContextMssql.Teachers.Any())
        {
            await AddTeacher(
                "Roberto", "Rossellini",
                "Calle Luna", user);
            await AddTeacher(
                "Giuseppe", "Tornatore",
                "Calle Luna", user);
            await AddTeacher(
                "Federico", "Fellini",
                "Rimini", user);
            await AddTeacher(
                "Ingrid", "Bergman",
                "Calle Sol", user);
            await AddTeacher(
                "Gina", "Lollobrigida",
                "Calle Sol", user);
            await AddTeacher(
                "Isabella", "Rossellini",
                "Calle Luna", user);
            await AddTeacher(
                "Monica", "Bellucci",
                "Calle Sol", user);
            await AddTeacher(
                "Giovanna", "Ralli",
                "Calle Luna", user);
            await AddTeacher(
                "Valeria", "Golino",
                "Calle Sol", user);
            await AddTeacher(
                "Sophia", "Loren",
                "Calle Luna", user);
            await AddTeacher(
                "Claudia", "Cardinale",
                "Calle Sol", user);
        }
    }


    internal static async Task AddStudent(
        string firstName, string lastName, string address, User user)
    {
        var document = _random.Next(100000, 999999999).ToString();
        var fixedPhone = _random.Next(1000000, 99999999).ToString();
        var cellPhone = _random.Next(1000000, 99999999).ToString();
        var addressFull = address + ", " + _random.Next(1, 9999);

        _dataContextMssql.Students.Add(new Student
            {
                //Document = document,
                FirstName = firstName,
                LastName = lastName,
                //FixedPhone = fixedPhone,
                //CellPhone = cellPhone,
                Address = addressFull,
                User = await SeedDbUsers.CheckUserAsync(
                    firstName, lastName,
                    $"{firstName}.{lastName}@rouba_a_descarada.com",
                    $"{firstName}.{lastName}@rouba_a_descarada.com",
                    $"{cellPhone}", "Owner",
                    document, addressFull
                )
            }
        );
    }

    internal static async Task AddTeacher(
        string firstName, string lastName, string address, User user)
    {
        var document = _random.Next(100000, 999999999).ToString();
        var fixedPhone = _random.Next(1000000, 99999999).ToString();
        var cellPhone = _random.Next(1000000, 99999999).ToString();
        var addressFull = address + ", " + _random.Next(1, 9999);

        _dataContextMssql.Teachers.Add(new Teacher
            {
                // Document = document,
                FirstName = firstName,
                LastName = lastName,
                // FixedPhone = fixedPhone,
                // CellPhone = cellPhone,
                Address = addressFull,
                User = await SeedDbUsers.CheckUserAsync(
                    firstName, lastName,
                    $"{firstName}.{lastName}@rouba_a_descarada.com",
                    $"{firstName}.{lastName}@rouba_a_descarada.com",
                    $"{cellPhone}", "Lessee",
                    document, addressFull
                )
            }
        );
    }
}