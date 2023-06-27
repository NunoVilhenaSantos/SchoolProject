using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using SchoolProject.Web.Data.Entities.School;
using Serilog;

namespace SchoolProject.Web.Data.Entities.Enrollments;

public class EnrollmentsFileHelper
{
    //
    // Global Properties for the windows forms
    // to store the data into files of class
    //

    #region Properties

    private static readonly string EnrollmentsFilePath =
        XFiles.FilesFolder + "enrollments.csv";

    #endregion


    public static void WriteEnrollmentsToFile(
        out bool success, out string message)
    {
        try
        {
            using (var fileStream = new FileStream(EnrollmentsFilePath,
                       FileMode.Create, FileAccess.Write))
            {
                // Log info message
                Log.Information(
                    "Enrollments file opened for writing");
            }
        }
        catch (IOException ex)
        {
            // Log error message
            Log.Error(ex,
                "Error accessing the file: {ErrorMessage}",
                ex.Message);
            message = $"Error accessing the file: {ex.Message}";
            success = false;
            return;
        }
        catch (Exception e)
        {
            // Log error message
            Log.Error(e,
                "Error accessing the file: {ErrorMessage}",
                e.Message);
            message = $"Error accessing the file: {e.Message}";
            success = false;
            return;
        }

        var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";"
        };

        try
        {
            using (var fileStream =
                   new FileStream(EnrollmentsFilePath,
                       FileMode.Create, FileAccess.Write))
            using (var streamWriter =
                   new StreamWriter(fileStream, Encoding.UTF8))
            using (var csvWriter = new CsvWriter(streamWriter, csvConfig))
            {
                csvWriter.WriteRecords(Enrollments.ListEnrollments);
            }

            // Log info message
            Log.Information(
                "Enrollments successfully written to file");
            message = "Operação realizada com sucesso";
            success = true;
        }
        catch (IOException ex)
        {
            // Log error message
            Log.Error(ex,
                "Error writing to the file: {ErrorMessage}",
                ex.Message);
            message = $"Error writing to the file: {ex.Message}";
            success = false;
        }
        catch (Exception e)
        {
            // Log error message
            Log.Error(e,
                "Error writing to the file: {ErrorMessage}",
                e.Message);
            message = $"Error writing to the file: {e.Message}";
            success = false;
        }
    }


    public static List<Enrollment> ReadEnrollmentsFromFile(
        out bool Success, out string myString)
    {
        try
        {
            using (var fileStream =
                   new FileStream(EnrollmentsFilePath, FileMode.OpenOrCreate,
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
               new FileStream(EnrollmentsFilePath, FileMode.OpenOrCreate,
                   FileAccess.Read))
        using (var streamReader = new StreamReader(fileStream))
        using (var csvReader = new CsvReader(streamReader, csvConfig))
        {
            myString = "Operação realizada com sucesso";
            Success = true;

            return csvReader.GetRecords<Enrollment>().ToList();
        }
    }
}