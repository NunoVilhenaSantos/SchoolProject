using System.Text;
using System.Text.RegularExpressions;

namespace SchoolProject.Web.Data.Entities.School;

public static class XFilesRelations
{
    //
    // Global Properties for the windows forms
    // to store the data into files of class
    //
    private static Regex _myRegex1 =
        new(@"^([a-zA-Z0-9\s_\\.\-:])+(.csv|.txt)$");

    private static Regex _myRegex2 = new("^[a-zA-Z]+:$");

    #region Constants

    //  static string COURSES_FILENAME = XFiles.FilesFolder +"courses.csv";
    //  static string STUDENTS_FILENAME = XFiles.FilesFolder +"students.csv";
    //  static string CLASSES_FILENAME =XFiles.FilesFolder +"classes.csv";

    private static string COURSE_CLASSES_FILENAME =
        XFiles.FilesFolder + "course_classes.csv";

    private static string COURSE_STUDENTS_FILENAME =
        XFiles.FilesFolder + "course_students.csv";

    private static string STUDENTS_CLASSES_FILENAME =
        XFiles.FilesFolder + "students_classes.csv";

    private static string COURSE_TEACHERS_FILENAME =
        XFiles.FilesFolder + "course_teacher.csv";

    private static readonly string DICTIONARIES_FILENAME =
        XFiles.FilesFolder + "dictionaries.csv";

    private static string SchoolClassCourseFilePath =
        XFiles.FilesFolder + "SchoolClassCourse.csv";

    private static string StudentCourseFilePath =
        XFiles.FilesFolder + "StudentCourse.csv";

    private static string TeacherCourseFilePath =
        XFiles.FilesFolder + "TeacherCourse.csv";

    //private const string lineSeparator = ";";

    #endregion


    #region ManagingData

    public static bool SaveDictionariesToFile(out string myString)
    {
        //
        // constructor for storing info in files
        // with a try and catch
        // and also returning the messages
        //
        FileStream fileStream;
        try
        {
            fileStream =
                new FileStream(DICTIONARIES_FILENAME,
                    FileMode.Create,
                    FileAccess.ReadWrite);
            fileStream.Close();
        }
        catch (IOException ex)
        {
            myString = "Error accessing the file: " + ex.Source + " | " +
                       ex.Message;
            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            myString = "Error accessing the file: " + e.Source + " | " +
                       e.Message;
            return false;
        }

        // Sort the dictionaries by their keys
        var sortedCourses =
            Courses.Courses.CoursesDictionary
                .OrderBy(kvp => kvp.Key);
        var sortedSchoolClasses =
            SchoolClasses.SchoolClasses.SchoolClassesDictionary
                .OrderBy(kvp => kvp.Key);
        var sortedStudents =
            Students.Students.StudentsDictionary
                .OrderBy(kvp => kvp.Key);
        var sortedTeachers =
            Teachers.Teachers.TeachersDictionary
                .OrderBy(kvp => kvp.Key);

        // Order the dictionaries by their keys
        // var sortedCourses =
        //     SchoolDatabase.Courses
        //         .OrderBy(x => x.Key)
        //         .ToDictionary(x => x.Key, x => x.Value);
        //
        // var sortedStudents =
        //     SchoolDatabase.Students
        //         .OrderBy(x => x.Key)
        //         .ToDictionary(x => x.Key, x => x.Value);
        //
        // var sortedTeachers =
        //     SchoolDatabase.Teachers
        //         .OrderBy(x => x.Key)
        //         .ToDictionary(x => x.Key, x => x.Value);
        //
        // var sortedSchoolClasses =
        //     SchoolDatabase.SchoolClasses
        //         .OrderBy(x => x.Key)
        //         .ToDictionary(x => x.Key, x => x.Value);

        // var sortedCourseClasses =
        //     SchoolDatabase.CourseClasses
        //         .OrderBy(x => x.Key)
        //         .ToDictionary(x => x.Key, x => x.Value);
        //
        // var sortedCourseStudents =
        //     SchoolDatabase.CourseStudents
        //         .OrderBy(x => x.Key)
        //         .ToDictionary(x => x.Key, x => x.Value);
        //
        // var sortedCourseTeacher =
        //     SchoolDatabase.CourseTeacher
        //         .OrderBy(x => x.Key)
        //         .ToDictionary(x => x.Key, x => x.Value);
        //
        // var sortedStudentClass =
        //     SchoolDatabase.StudentClass
        //         .OrderBy(x => x.Key)
        //         .ToDictionary(x => x.Key, x => x.Value);


        var sortedCourseClasses =
            SchoolDatabase.CourseClasses
                .OrderBy(x => x.Key)
                .ToDictionary(x => x.Key,
                    x => x.Value
                        .OrderBy(y => y)
                        .ToList());

        var sortedCourseStudents =
            SchoolDatabase.CourseStudents
                .OrderBy(x => x.Key)
                .ToDictionary(x => x.Key,
                    x => x.Value
                        .OrderBy(y => y)
                        .ToList());

        var sortedCourseTeacher =
            SchoolDatabase.CourseTeacher
                .OrderBy(x => x.Key)
                .ToDictionary(x => x.Key,
                    x => x.Value
                        .OrderBy(y => y)
                        .ToList());

        var sortedStudentClass = SchoolDatabase.StudentClass
            .OrderBy(x => x.Key)
            .ToDictionary(x => x.Key,
                x => x.Value
                    .OrderBy(y => y)
                    .ToList());


        // ...

        using (fileStream =
                   new FileStream(DICTIONARIES_FILENAME,
                       FileMode.Create, FileAccess.ReadWrite))
            //using (StreamWriter streamWriter = new(fileStream, Encoding.UTF8))
        using (StreamWriter writer = new(fileStream, Encoding.UTF8))
        {
            writer.WriteLine("CourseClasses:");
            foreach (
                var kvp in
                sortedCourseClasses) //SchoolDatabase.CourseClasses
                writer.WriteLine(
                    $"{kvp.Key}{XFiles.Delimiter}" +
                    $"{string.Join(XFiles.Delimiter, kvp.Value)}");

            writer.WriteLine("CourseStudents:");
            foreach (
                var kvp in
                sortedCourseStudents) //SchoolDatabase.CourseStudents
                writer.WriteLine(
                    $"{kvp.Key}{XFiles.Delimiter}" +
                    $"{string.Join(XFiles.Delimiter, kvp.Value)}");

            writer.WriteLine("CourseTeacher:");
            foreach (
                var kvp in
                sortedCourseTeacher) //SchoolDatabase.CourseTeacher
                writer.WriteLine(
                    $"{kvp.Key}{XFiles.Delimiter}" +
                    $"{string.Join(XFiles.Delimiter, kvp.Value)}");

            writer.WriteLine("StudentClass:");
            foreach (
                var kvp in
                sortedStudentClass) //SchoolDatabase.StudentClass
                writer.WriteLine(
                    $"{kvp.Key}{XFiles.Delimiter}" +
                    $"{string.Join(XFiles.Delimiter, kvp.Value)}");

            writer.Close();
        }

        myString = "Operação realizada com sucesso";
        return true;
    }


    public static bool LoadDictionariesFromFile(out string myString)
    {
        //
        // constructor for storing info in files
        // with a try and catch
        // and also returning the messages
        //
        FileStream fileStream;
        try
        {
            fileStream =
                new FileStream(DICTIONARIES_FILENAME,
                    FileMode.OpenOrCreate,
                    FileAccess.ReadWrite);
            fileStream.Close();
        }
        catch (IOException ex)
        {
            myString = "Error accessing the file: " + ex.Source + " | " +
                       ex.Message;
            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            myString = "Error accessing the file: " + e.Source + " | " +
                       e.Message;
            return false;
        }

        using (fileStream =
                   new FileStream(DICTIONARIES_FILENAME,
                       FileMode.OpenOrCreate, FileAccess.ReadWrite))
        using (var reader =
               new StreamReader(fileStream, Encoding.UTF8))
        {
            string currentDictionary = null;

            SchoolDatabase.UpdateDictionaries();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                // Define a regular expression to match letters followed by a colon
                //var regex = MyRegex();

                // Get the index of the colon
                var colonIndex = line.IndexOf(':');

                // Get the substring before the colon
                var letters =
                    line.Substring(0, colonIndex + 1);

                // Trim leading and trailing whitespace
                letters = letters.Trim();

                if (letters.EndsWith(":"))
                {
                    currentDictionary = letters.TrimEnd(':');
                    continue;
                }

                var values =
                    line.Trim('"')
                        .Split(';', StringSplitOptions.RemoveEmptyEntries)
                        .Select(int.Parse)
                        .ToArray();

                switch (currentDictionary)
                {
                    case "CourseClasses":
                        SchoolDatabase.CourseClasses[values[0]] =
                            new HashSet<int>(values.Skip(1));
                        SchoolDatabase.AssignCoursesToClass(
                            new HashSet<int>(values.Skip(1)),
                            values[0]);
                        break;

                    case "CourseStudents":
                        SchoolDatabase.CourseStudents[values[0]] =
                            new HashSet<int>(values.Skip(1));
                        SchoolDatabase.EnrollStudentInCourses(
                            new HashSet<int>(values.Skip(1)),
                            values[0]);
                        break;

                    case "CourseTeacher":
                        SchoolDatabase.CourseTeacher[values[0]] =
                            new HashSet<int>(values.Skip(1));
                        SchoolDatabase.AssignTeacherToCourses(
                            new HashSet<int>(values.Skip(1)),
                            values[0]);
                        break;

                    case "StudentClass":
                        SchoolDatabase.StudentClass[values[0]] =
                            new HashSet<int>(values.Skip(1));
                        SchoolDatabase.AssignCoursesToClass(
                            new HashSet<int>(values.Skip(1)),
                            values[0]);
                        break;

                    default:
                        throw new Exception(
                            $"Invalid dictionary identifier " +
                            $"'{currentDictionary}'");
                }
            }

            reader.Close();
        }

        myString = "Operação realizada com sucesso";
        return true;
    }

    //
    //
    //     // private void LoadData()
    //     // {
    //     //     LoadCourseClasses();
    //     //     LoadCourseStudents();
    //     //     LoadCourseTeacher();
    //     //     LoadStudentClass();
    //     // }
    //     //
    //     // private void SaveData()
    //     // {
    //     //     SaveCourseClasses();
    //     //     SaveCourseStudents();
    //     //     SaveCourseTeacher();
    //     //     SaveStudentClass();
    //     // }
    //
    //     #endregion
    //
    //
    //     // private void LoadCourseClasses()
    //     // {
    //     //     using (var reader = new StreamReader(COURSE_CLASSES_FILENAME))
    //     //     using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
    //     //     {
    //     //         // Read the courses from the CSV file
    //     //         var courses = csv.GetRecords<Course>().ToList();
    //     //         foreach (var course in courses)
    //     //         {
    //     //             Courses[course.IdCourse] = course;
    //     //         }
    //     //
    //     //         // Read the school classes from the CSV file
    //     //         var schoolClasses = csv.GetRecords<SchoolClass>().ToList();
    //     //         foreach (var schoolClass in schoolClasses)
    //     //         {
    //     //             SchoolClasses[schoolClass.IdSchoolClass] = schoolClass;
    //     //         }
    //     //
    //     //         // Read the school class courses from the CSV file
    //     //         var courseClassList = csv.GetRecords<CourseClass>().ToList();
    //     //         foreach (var courseClass in courseClassList)
    //     //         {
    //     //             if (!CourseClasses.ContainsKey(courseClass.SchoolClassId))
    //     //             {
    //     //                 CourseClasses[courseClass.SchoolClassId] =
    //     //                     new HashSet<int>();
    //     //             }
    //     //
    //     //             CourseClasses[courseClass.SchoolClassId]
    //     //                 .Add(courseClass.CourseId);
    //     //         }
    //     //     }
    //     // }
    //
    //     //
    //     // private void SaveCourseClasses()
    //     // {
    //     //     using (var writer = new StreamWriter(COURSE_CLASSES_FILENAME))
    //     //     using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
    //     //     {
    //     //         // Write the courses to the CSV file
    //     //         csv.WriteRecords(Courses.Values);
    //     //
    //     //         // Write the school classes to the CSV file
    //     //         csv.WriteRecords(SchoolClasses.Values);
    //     //
    //     //         // Write the school class courses to the CSV file
    //     //         foreach (var kvp in CourseClasses)
    //     //         foreach (var courseId in kvp.Value)
    //     //         {
    //     //             csv.WriteRecord(new
    //     //             {
    //     //                 SchoolClassId = kvp.Key,
    //     //                 CourseId = courseId
    //     //             });
    //     //             csv.NextRecord();
    //     //         }
    //     //     }
    //     // }
    //
    //     // private void LoadCourseStudents()
    //     // {
    //     //     using (var reader = new StreamReader(COURSE_STUDENTS_FILENAME))
    //     //     using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
    //     //     {
    //     //         // public static Dictionary<int, HashSet<int>> CourseClasses = new();
    //     //         // public static Dictionary<int, HashSet<int>> CourseStudents = new();
    //     //         // public static Dictionary<int, HashSet<int>> StudentClass = new();
    //     //         // public static Dictionary<int, HashSet<int>> CourseTeacher = new();
    //     //
    //     //         csv.Configuration.RegisterClassMap<CourseStudentMap>();
    //     //         var records = csv.GetRecords<CourseStudents>();
    //     //         foreach (var record in records)
    //     //         {
    //     //             if (!CourseStudents.ContainsKey(record.CourseId))
    //     //             {
    //     //                 CourseStudents.Add(record.CourseId, new HashSet<int>());
    //     //             }
    //     //
    //     //             CourseStudents[record.CourseId].Add(record.StudentId);
    //     //         }
    //     //     }
    //     // }
    //
    //     // private void SaveCourseStudents()
    //     // {
    //     //     using (var writer = new StreamWriter(COURSE_STUDENTS_FILENAME))
    //     //     using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
    //     //     {
    //     //         // Write the courses to the CSV file
    //     //         csv.WriteRecords(Courses.Values);
    //     //
    //     //         // Write the students to the CSV file
    //     //         csv.WriteRecords(Students.Values);
    //     //
    //     //
    //     //         // Write the student courses to the CSV file
    //     //         foreach (var kvp in CourseStudents)
    //     //         foreach (var courseId in kvp.Value)
    //     //         {
    //     //             csv.WriteRecord(new
    //     //             {
    //     //                 StudentId = kvp.Key,
    //     //                 CourseId = courseId
    //     //             });
    //     //             csv.NextRecord();
    //     //         }
    //     //
    //     //         Write the student courses to the CSV file
    //     //         foreach (var kvp in CourseStudents)
    //     //         foreach (var studentId in kvp.Value)
    //     //         {
    //     //             csv.WriteRecord(new
    //     //             {
    //     //                 StudentId = studentId,
    //     //                 CourseId = kvp.Key
    //     //             });
    //     //             csv.NextRecord();
    //     //         }
    //     //     }
    //     // }
    //     //
    //     // private void LoadStudentClass()
    //     // {
    //     //     using (var reader = new StreamReader(STUDENTS_CLASSES_FILENAME))
    //     //     using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
    //     //     {
    //     //         csv.Configuration.RegisterClassMap<CourseStudentMap>();
    //     //         var records = csv.GetRecords<CourseStudents>();
    //     //         foreach (var record in records)
    //     //         {
    //     //             if (!CourseStudents.ContainsKey(record.CourseId))
    //     //             {
    //     //                 CourseStudents.Add(record.CourseId, new HashSet<int>());
    //     //             }
    //     //
    //     //             CourseStudents[record.CourseId].Add(record.StudentId);
    //     //         }
    //     //     }
    //     // }
    //
    //     // private void SaveStudentClass()
    //     // {
    //     //     using (var writer = new StreamWriter(STUDENTS_CLASSES_FILENAME))
    //     //     using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
    //     //     {
    //     //         // Write the school classes to the CSV file
    //     //         csv.WriteRecords(SchoolClasses.Values);
    //     //
    //     //         // Write the students to the CSV file
    //     //         csv.WriteRecords(Students.Values);
    //     //
    //     //         // Write the school class student to the CSV file
    //     //         foreach (var kvp in StudentClass)
    //     //         foreach (var studentId in kvp.Value)
    //     //         {
    //     //             csv.WriteRecord(new
    //     //             {
    //     //                 SchoolClassId = kvp.Key,
    //     //                 StudentId = studentId
    //     //             });
    //     //             csv.NextRecord();
    //     //         }
    //     //     }
    //     // }
    //
    //     // private void LoadCourseTeacher()
    //     // {
    //     //     using (var reader = new StreamReader(COURSE_TEACHERS_FILENAME))
    //     //     using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
    //     //     {
    //     //         // csv.Configuration.RegisterClassMap<CourseTeacherMap>();
    //     //         // var records = csv.GetRecords<CourseTeacher>();
    //     //         foreach (var record in records)
    //     //         {
    //     //             if (!CourseTeacher.ContainsKey(record.StudentId))
    //     //             {
    //     //                 StudentClass.Add(record.StudentId, new HashSet<int>());
    //     //             }
    //     //
    //     //             StudentClass[record.StudentId].Add(record.ClassId);
    //     //         }
    //     //     }
    //     // }
    //
    //     //
    //     // private void LoadCourseTeacher()
    //     // {
    //     //     using (var fileStream = new FileStream(COURSE_TEACHERS_FILENAME,
    //     //                FileMode.Open, FileAccess.Read))
    //     //     using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
    //     //     using (var csv =
    //     //            new CsvReader(streamReader, CultureInfo.InvariantCulture))
    //     //     {
    //     //         csv.Context.RegisterClassMap<CourseTeacherMap>();
    //     //         var records = csv.GetRecords<CourseTeacher>().ToList();
    //     //
    //     //         foreach (var record in records)
    //     //         {
    //     //             if (!CourseTeacher.ContainsKey(record.TeacherId))
    //     //             {
    //     //                 CourseTeacher[record.TeacherId] = new HashSet<int>();
    //     //             }
    //     //
    //     //             CourseTeacher[record.TeacherId].Add(record.CourseId);
    //     //         }
    //     //     }
    //     // }
    //     //
    //     // private void SaveCourseTeacher()
    //     // {
    //     //     using (var writer = new StreamWriter(COURSE_TEACHERS_FILENAME))
    //     //     using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
    //     //     {
    //     //         // Write the courses to the CSV file
    //     //         csv.WriteRecords(Courses.Values);
    //     //
    //     //         // Write the teachers to the CSV file
    //     //         csv.WriteRecords(Teachers.Values);
    //     //
    //     //         // Write the teacher courses to the CSV file
    //     //         foreach (var kvp in CourseTeacher)
    //     //         foreach (var courseId in kvp.Value)
    //     //         {
    //     //             csv.WriteRecord(new
    //     //             {
    //     //                 TeacherId = kvp.Key,
    //     //                 CourseId = courseId
    //     //             });
    //     //             csv.NextRecord();
    //     //         }
    //     //     }
    //     // }
    //
    //     #region SaveToCsv
    //
    //     public static bool LoadFromCsv(string filePath, out string myString)
    //     {
    //         //
    //         // constructor for the reading files
    //         // with a try and catch
    //         // and also returning the messages
    //         //
    //
    //         //FileStream fileStream;
    //         try
    //         {
    //             using (var fileStream = new FileStream(
    //                        filePath,
    //                        FileMode.OpenOrCreate,
    //                        FileAccess.Read))
    //             {
    //             }
    //         }
    //         catch (IOException ex)
    //         {
    //             myString = "Error accessing the file: " +
    //                        ex.Source + " | " + ex.Message;
    //             return false;
    //         }
    //         catch (Exception e)
    //         {
    //             Console.WriteLine(e.Message);
    //             myString = "Error accessing the file: " +
    //                        e.Source + " | " + e.Message;
    //             return false;
    //         }
    //
    //         var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
    //         {
    //             Delimiter = ";"
    //         };
    //
    //         //  const string COURSE_CLASSES_FILENAME = "course_classes.csv";
    //         //  const string COURSE_STUDENTS_FILENAME = "course_students.csv";
    //         //  const string STUDENTS_CLASSES_FILENAME = "students_classes.csv";
    //         //  const string COURSE_TEACHERS_FILENAME = "course_teacher.csv";
    //
    //         // using (fileStream = new FileStream(
    //         //            filePath,
    //         //            FileMode.OpenOrCreate, FileAccess.Read))
    //         // using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
    //         // using (var csv = new CsvReader(streamReader, csvConfig))
    //         // {
    //         //     // public static Dictionary<int, HashSet<int>> CourseClasses = new();
    //         //     // public static Dictionary<int, HashSet<int>> CourseStudents = new();
    //         //     // public static Dictionary<int, HashSet<int>> StudentClass = new();
    //         //     // public static Dictionary<int, HashSet<int>> CourseTeacher = new();
    //         //
    //         //     Courses =
    //         //         csv.GetRecords<Course>()
    //         //             .ToDictionary(c => c.IdCourse);
    //         //
    //         //     SchoolClasses =
    //         //         csv.GetRecords<SchoolClass>()
    //         //             .ToDictionary(
    //         //                 sc => sc.IdSchoolClass);
    //         //
    //         //     Students =
    //         //         csv.GetRecords<Student>()
    //         //             .ToDictionary(s => s.IdStudent);
    //         //
    //         //     Teachers =
    //         //         csv.GetRecords<Teacher>()
    //         //             .ToDictionary(t => t.TeacherId);
    //         // }
    //
    //         //  const string STUDENTS_CLASSES_FILENAME = "students_classes.csv";
    //         //  const string COURSE_CLASSES_FILENAME = "course_classes.csv";
    //         //  const string COURSE_STUDENTS_FILENAME = "course_students.csv";
    //         //  const string COURSE_TEACHERS_FILENAME = "course_teacher.csv";
    //
    //         const string COURSE_FILENAME = "courses.csv";
    //         const string SCHOOLCLASS_FILENAME = "schoolclasses.csv";
    //         const string STUDENT_FILENAME = "students.csv";
    //         const string TEACHER_FILENAME = "teachers.csv";
    //
    //         using (var fileStream = new FileStream(COURSE_FILENAME,
    //                    FileMode.OpenOrCreate, FileAccess.Read))
    //         using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
    //         using (var csv = new CsvReader(streamReader, csvConfig))
    //         {
    //             Courses = csv.GetRecords<Course>().ToDictionary(c => c.IdCourse);
    //         }
    //
    //         using (var fileStream = new FileStream(SCHOOLCLASS_FILENAME,
    //                    FileMode.OpenOrCreate, FileAccess.Read))
    //         using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
    //         using (var csv = new CsvReader(streamReader, csvConfig))
    //         {
    //             SchoolClasses = csv.GetRecords<SchoolClass>()
    //                 .ToDictionary(sc => sc.IdSchoolClass);
    //         }
    //
    //         using (var fileStream = new FileStream(STUDENT_FILENAME,
    //                    FileMode.OpenOrCreate, FileAccess.Read))
    //         using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
    //         using (var csv = new CsvReader(streamReader, csvConfig))
    //         {
    //             Students = csv.GetRecords<Student>().ToDictionary(s => s.IdStudent);
    //         }
    //
    //         using (var fileStream = new FileStream(TEACHER_FILENAME,
    //                    FileMode.OpenOrCreate, FileAccess.Read))
    //         using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
    //         using (var csv = new CsvReader(streamReader, csvConfig))
    //         {
    //             Teachers = csv.GetRecords<Teacher>().ToDictionary(t => t.TeacherId);
    //         }
    //
    //
    //         myString = "Operação realizada com sucesso";
    //         return true;
    //     }
    //
    //     public static bool SaveToCsvExtenso(string filePath, out string myString)
    //     {
    //         //
    //         // constructor for the reading files
    //         // with a try and catch
    //         // and also returning the messages
    //         //
    //
    //         FileStream fileStream;
    //         try
    //         {
    //             using (fileStream = new FileStream(
    //                        filePath,
    //                        FileMode.Create,
    //                        FileAccess.Write))
    //             {
    //             }
    //         }
    //         catch (IOException ex)
    //         {
    //             myString = "Error accessing the file: " +
    //                        ex.Source + " | " + ex.Message;
    //             return false;
    //         }
    //         catch (Exception e)
    //         {
    //             Console.WriteLine(e.Message);
    //             myString = "Error accessing the file: " +
    //                        e.Source + " | " + e.Message;
    //             return false;
    //         }
    //
    //         var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
    //         {
    //             Delimiter = ";"
    //         };
    //
    //         // const string COURSE_CLASSES_FILENAME = "course_classes.csv";
    //         // const string COURSE_STUDENTS_FILENAME = "course_students.csv";
    //         // const string STUDENTS_CLASSES_FILENAME = "students_classes.csv";
    //         // const string COURSE_TEACHERS_FILENAME = "course_teacher.csv";
    //
    //         // using (fileStream =
    //         //            new FileStream(filePath, FileMode.Create, FileAccess.Write))
    //         // using (var writer = new StreamWriter(fileStream, Encoding.UTF8))
    //         // using (var csv = new CsvWriter(writer, csvConfig))
    //         // {
    //         //     // Write the courses to the CSV file
    //         //     csv.WriteRecords(Courses.Values);
    //         //
    //         //     // Write the school classes to the CSV file
    //         //     csv.WriteRecords(SchoolClasses.Values);
    //         //
    //         //     // Write the students to the CSV file
    //         //     csv.WriteRecords(Students.Values);
    //         //
    //         //     // Write the teachers to the CSV file
    //         //     csv.WriteRecords(Teachers.Values);
    //         //
    //         //     // Write the school class student to the CSV file
    //         //     foreach (var kvp in StudentClass)
    //         //     foreach (var studentId in kvp.Value)
    //         //     {
    //         //         csv.WriteRecord(new
    //         //         {
    //         //             SchoolClassId = kvp.Key,
    //         //             StudentId = studentId
    //         //         });
    //         //         csv.NextRecord();
    //         //     }
    //         //
    //         //     // Write the school class courses to the CSV file
    //         //     foreach (var kvp in CourseClasses)
    //         //     foreach (var courseId in kvp.Value)
    //         //     {
    //         //         csv.WriteRecord(new
    //         //         {
    //         //             SchoolClassId = kvp.Key,
    //         //             CourseId = courseId
    //         //         });
    //         //         csv.NextRecord();
    //         //     }
    //         //
    //         //     // Write the student courses to the CSV file
    //         //     foreach (var kvp in CourseStudents)
    //         //     foreach (var courseId in kvp.Value)
    //         //     {
    //         //         csv.WriteRecord(new
    //         //         {
    //         //             StudentId = kvp.Key,
    //         //             CourseId = courseId
    //         //         });
    //         //         csv.NextRecord();
    //         //     }
    //         //
    //         //     // Write the teacher courses to the CSV file
    //         //     foreach (var kvp in CourseTeacher)
    //         //     foreach (var courseId in kvp.Value)
    //         //     {
    //         //         csv.WriteRecord(new
    //         //         {
    //         //             TeacherId = kvp.Key,
    //         //             CourseId = courseId
    //         //         });
    //         //         csv.NextRecord();
    //         //     }
    //         // }
    //
    //         // private const string COURSE_CLASSES_FILENAME = "course_classes.csv";
    //         using (fileStream =
    //                    new FileStream(COURSE_CLASSES_FILENAME,
    //                        FileMode.Create,
    //                        FileAccess.Write))
    //         using (var writer = new StreamWriter(fileStream, Encoding.UTF8))
    //         using (var csv = new CsvWriter(writer, csvConfig))
    //         {
    //             // Write the courses to the CSV file
    //             csv.WriteRecords(Courses.Values);
    //
    //             // Write the school classes to the CSV file
    //             csv.WriteRecords(SchoolClasses.Values);
    //
    //             // Write the school class courses to the CSV file
    //             foreach (var kvp in CourseClasses)
    //             foreach (var courseId in kvp.Value)
    //             {
    //                 csv.WriteRecord(new
    //                 {
    //                     SchoolClassId = kvp.Key,
    //                     CourseId = courseId
    //                 });
    //                 csv.NextRecord();
    //             }
    //         }
    //
    //         // private const string COURSE_STUDENTS_FILENAME = "course_students.csv";
    //         using (fileStream =
    //                    new FileStream(COURSE_STUDENTS_FILENAME,
    //                        FileMode.Create, FileAccess.Write))
    //         using (var writer = new StreamWriter(fileStream, Encoding.UTF8))
    //         using (var csv = new CsvWriter(writer, csvConfig))
    //         {
    //             // Write the courses to the CSV file
    //             csv.WriteRecords(Courses.Values);
    //
    //             // Write the students to the CSV file
    //             csv.WriteRecords(Students.Values);
    //
    //
    //             // Write the student courses to the CSV file
    //             foreach (var kvp in CourseStudents)
    //             foreach (var courseId in kvp.Value)
    //             {
    //                 csv.WriteRecord(new
    //                 {
    //                     StudentId = kvp.Key,
    //                     CourseId = courseId
    //                 });
    //                 csv.NextRecord();
    //             }
    //         }
    //
    //         // private const string STUDENTS_CLASSES_FILENAME = "students_classes.csv";
    //         using (fileStream =
    //                    new FileStream(STUDENTS_CLASSES_FILENAME,
    //                        FileMode.Create,
    //                        FileAccess.Write))
    //         using (var writer = new StreamWriter(fileStream, Encoding.UTF8))
    //         using (var csv = new CsvWriter(writer, csvConfig))
    //         {
    //             // Write the school classes to the CSV file
    //             csv.WriteRecords(SchoolClasses.Values);
    //
    //             // Write the students to the CSV file
    //             csv.WriteRecords(Students.Values);
    //
    //             // Write the school class student to the CSV file
    //             foreach (var kvp in StudentClass)
    //             foreach (var studentId in kvp.Value)
    //             {
    //                 csv.WriteRecord(new
    //                 {
    //                     SchoolClassId = kvp.Key,
    //                     StudentId = studentId
    //                 });
    //                 csv.NextRecord();
    //             }
    //         }
    //
    //
    //         // private const string COURSE_TEACHERS_FILENAME = "course_teacher.csv";
    //         using (fileStream =
    //                    new FileStream(COURSE_TEACHERS_FILENAME,
    //                        FileMode.Create,
    //                        FileAccess.Write))
    //         using (var writer = new StreamWriter(fileStream, Encoding.UTF8))
    //         using (var csv = new CsvWriter(writer, csvConfig))
    //         {
    //             // Write the courses to the CSV file
    //             csv.WriteRecords(Courses.Values);
    //
    //             // Write the teachers to the CSV file
    //             csv.WriteRecords(Teachers.Values);
    //
    //             // Write the teacher courses to the CSV file
    //             foreach (var kvp in CourseTeacher)
    //             foreach (var courseId in kvp.Value)
    //             {
    //                 csv.WriteRecord(new
    //                 {
    //                     TeacherId = kvp.Key,
    //                     CourseId = courseId
    //                 });
    //                 csv.NextRecord();
    //             }
    //         }
    //
    //
    //         myString = "Operação realizada com sucesso";
    //         return true;
    //     }
    //

    #endregion
}