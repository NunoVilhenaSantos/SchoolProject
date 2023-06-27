using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using SchoolProject.Web.Data.Entities.School;
using Serilog;

namespace SchoolProject.Web.Data.Entities.Students;

public class StudentsFileHelper
{
    //
    // Global Properties for the windows forms
    // to store the data into files of class
    //

    #region Properties

    private static readonly string StudentsFilePath =
        XFiles.FilesFolder + "students.csv";

    #endregion


    // public static void WriteStudentsToFile(
    //     out bool Success, out string myString)
    // {
    //     try
    //     {
    //         using (var fileStream =
    //                new FileStream(StudentsFilePath, FileMode.Create,
    //                    FileAccess.Write
    //                ))
    //         {
    //         }
    //     }
    //     catch (IOException ex)
    //     {
    //         myString = "Error accessing the file: " + ex.Source + " | " +
    //                    ex.Message;
    //         Success = false;
    //     }
    //     catch (Exception e)
    //     {
    //         Console.WriteLine(e.Message);
    //         myString = "Error accessing the file: " + e.Source + " | " +
    //                    e.Message;
    //         Success = false;
    //     }
    //
    //     var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
    //     {
    //         Delimiter = ";"
    //     };
    //
    //     using (var fileStream =
    //            new FileStream(StudentsFilePath, FileMode.Create,
    //                FileAccess.Write))
    //     using (var streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
    //     using (var csvWriter = new CsvWriter(streamWriter, csvConfig))
    //     {
    //         csvWriter.WriteRecords(Students.StudentsList);
    //
    //         myString = "Operação realizada com sucesso";
    //         Success = true;
    //     }
    // }


    public static void WriteStudentsToFile(out bool Success,
        out string myString)
    {
        try
        {
            //Serilog.Log.Logger.Information("Creating file stream");
            Log.Logger.Information(
                "Creating file stream");
            using (var fileStream =
                   new FileStream(StudentsFilePath,
                       FileMode.Create, FileAccess.Write))
            {
                Log.Logger.Information(
                    "File stream created successfully");
            }
        }
        catch (IOException ex)
        {
            Log.Logger.Error(
                ex,
                "Error accessing file: {Message}",
                ex.Message);
            myString = "Error accessing the file: " + ex.Source + " | " +
                       ex.Message;
            Success = false;
            return;
        }
        catch (Exception e)
        {
            Log.Logger.Error(e,
                "Unexpected error accessing file: {Message}",
                e.Message);
            myString = "Error accessing the file: " + e.Source + " | " +
                       e.Message;
            Success = false;
            return;
        }

        var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";"
        };

        try
        {
            Log.Logger.Information(
                "Creating file writer");
            using (var fileStream = new FileStream(StudentsFilePath,
                       FileMode.Create, FileAccess.Write))
            using (var streamWriter =
                   new StreamWriter(fileStream, Encoding.UTF8))
            using (var csvWriter = new CsvWriter(streamWriter, csvConfig))
            {
                Log.Logger.Information(
                    "Writing records to file");
                csvWriter.WriteRecords(Students.StudentsList);
            }

            myString = "Operação realizada com sucesso";
            Success = true;
            Log.Logger.Information(
                "Records written successfully");
        }
        catch (Exception ex)
        {
            Log.Logger.Error(
                ex,
                "Unexpected error writing records to file:" +
                " {Message}",
                ex.Message);
            myString = "Error writing records to file: " + ex.Source + " | " +
                       ex.Message;
            Success = false;
        }
    }


    public static List<Student> ReadStudentsFromFile(
        out bool Success, out string myString)
    {
        try
        {
            using (var fileStream =
                   new FileStream(StudentsFilePath, FileMode.OpenOrCreate,
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
               new FileStream(StudentsFilePath, FileMode.OpenOrCreate,
                   FileAccess.Read))
        using (var streamReader = new StreamReader(fileStream))
        using (var csvReader = new CsvReader(streamReader, csvConfig))
        {
            myString = "Operação realizada com sucesso";
            Success = true;

            return csvReader.GetRecords<Student>().ToList();
        }
    }
}