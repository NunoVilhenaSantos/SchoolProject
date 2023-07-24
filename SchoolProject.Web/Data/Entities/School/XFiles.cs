using System.Reflection;
using System.Text;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.Enrollments;
using SchoolProject.Web.Data.Entities.ExtraTables;
using SchoolProject.Web.Data.Entities.SchoolClasses;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Entities.Teachers;
using SchoolProject.Web.Helpers;

namespace SchoolProject.Web.Data.Entities.School;

public static class XFiles
{
    //
    // Global Properties for the windows forms
    // to store the data into files of class
    //


    private static readonly IUserHelper _userHelper;

    // public static Logger Logger1 =
    //     new LoggerConfiguration().CreateLogger();
    //
    // public static Logger Logger =
    //     new LoggerConfiguration().WriteTo.Console().CreateLogger();

    internal const string Delimiter = ";";

    private static readonly string ProjectFolder =
        Directory.GetCurrentDirectory();
    //private static string ProjectFolder =
    //    "C:\\Users\\nunov\\Downloads\\Projeto\\";

    internal static string FilesFolder = ProjectFolder + "\\XFiles\\";

    private static readonly string
        CoursesFile = FilesFolder + "CoursesFile.csv";

    private static readonly string SchoolClassesFile =
        FilesFolder + "SchoolClassesFile.csv";

    private static readonly string StudentsFile =
        FilesFolder + "StudentsFile.csv";

    private static readonly string EnrollmentsFile =
        FilesFolder + "EnrollmentsFile.csv";

    private static readonly string TeachersFile =
        FilesFolder + "TeachersFile.csv";

    private static string SchoolDictionariesFilePath =
        FilesFolder + "SchoolDictionaries.csv";

    private static string SchoolDictionariesExtensoCsv =
        FilesFolder + "SchoolDictionariesExtenso.csv";

    public static string SchoolProjectLoggerFile =
        FilesFolder + "SchoolProjectLoggerFile.txt";
}