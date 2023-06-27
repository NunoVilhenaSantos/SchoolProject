using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using SchoolProject.Web.Data.Entities.School;
using Serilog;

namespace SchoolProject.Web.Data.Entities.Courses;

public class CoursesFileHelper
{
    //
    // Global Properties for the windows forms
    // to store the data into files of class
    //

    #region Properties

    private static readonly string CoursesFilePath =
        XFiles.FilesFolder + "courses.csv";

    #endregion


    public static void WriteCoursesToFile(
        out bool Success, out string myString)
    {
        var logger = Log.ForContext(typeof(CoursesFileHelper));

        try
        {
            using (var fileStream =
                   new FileStream(CoursesFilePath,
                       FileMode.Create, FileAccess.Write))
            {
            }
        }
        catch (IOException ex)
        {
            myString = "Error accessing the file: "
                       + ex.Source + " | " + ex.Message;
            Success = false;
            logger.Error(ex, "Error accessing the file");
            return;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            myString = "Error accessing the file: "
                       + e.Source + " | " + e.Message;
            Success = false;
            logger.Error(e, "Error accessing the file");
            return;
        }

        var csvConfig =
            new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";"
            };

        using (var fileStream =
               new FileStream(CoursesFilePath,
                   FileMode.Create, FileAccess.Write))
        using (var streamWriter =
               new StreamWriter(fileStream, Encoding.UTF8))
        using (var csvWriter =
               new CsvWriter(streamWriter, csvConfig))
        {
            csvWriter.WriteRecords(Courses.CoursesList);
        }

        myString = "Operação realizada com sucesso";
        Success = true;
        logger.Information(
            "Courses successfully written to file");
    }


    public static List<Course> ReadCoursesFromFile(
        out bool Success, out string myString)
    {
        try
        {
            using (var fileStream =
                   new FileStream(CoursesFilePath, FileMode.OpenOrCreate,
                       FileAccess.Read))
            {
            }
        }
        catch (IOException ex)
        {
            myString = "Error accessing the file: " + ex.Source + " | " +
                       ex.Message;
            Success = false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            myString = "Error accessing the file: " + e.Source + " | " +
                       e.Message;
            Success = false;
        }

        var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";"
        };

        using (var fileStream =
               new FileStream(CoursesFilePath, FileMode.OpenOrCreate,
                   FileAccess.Read))
        using (var streamWriter = new StreamReader(fileStream))
        using (var csvReader = new CsvReader(streamWriter, csvConfig))
        {
            myString = "Operação realizada com sucesso";
            Success = true;

            return csvReader.GetRecords<Course>().ToList();
        }
    }
}