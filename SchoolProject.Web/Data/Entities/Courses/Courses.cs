using SchoolProject.Web.Data.Entities.School;

namespace SchoolProject.Web.Data.Entities.Courses;

public static class Courses
{
    #region Properties

    public static List<Course> CoursesList { get; set; } = new();
    public static readonly Dictionary<int, Course> CoursesDictionary = new();

    #endregion


    #region Methods

    /// <summary>
    ///     Adds a new course to the course list.
    /// </summary>
    /// <param name="id">The course's ID.</param>
    /// <param name="name">The course's name.</param>
    /// <param name="workLoad">The course's work load.</param>
    /// <param name="credits">The course's credits.</param>
    public static void AddCourse(int id, string name, int workLoad, int credits)
    {
        CoursesList.Add(
            new Course
            {
                //Id_Course = id,
                Name = name,
                WorkLoad = workLoad,
                Credits = credits
            }
        );
        // Add course to the database.
        SchoolDatabase.AddCourse(CoursesList[^1]);
        // Update the number of students for the new course.
        GetStudentsCount();
    }


    /// <summary>
    ///     Deletes a course from the course list.
    /// </summary>
    /// <param name="id">The ID of the course to be deleted.</param>
    /// <returns>
    ///     Returns a text informing whether
    ///     the operation was valid or not.
    /// </returns>
    public static string DeleteCourse(int id)
    {
        var course = CoursesList.FirstOrDefault(a => a.Id == id);

        if (course == null)
            // The course does not exist in the list.
            return "O curso não existe";

        // Remove the course from the course list.
        CoursesList.Remove(course);
        // The course was successfully deleted.
        return "O curso foi apagado";
    }


    /// <summary>
    ///     Edits an existing course from the course list.
    /// </summary>
    /// <param name="id">The ID of the course to be edited.</param>
    /// <param name="name">The new name for the course.</param>
    /// <param name="workLoad">The new work load for the course.</param>
    /// <returns>
    ///     Informs if the list is empty or the course doesn't exist,
    ///     or if the course was successfully edited.
    /// </returns>
    public static string EditCourse(
        int id, string name, int workLoad
    )
    {
        if (CoursesList.Count < 1)
            return "Lista está vazia"; // The course list is empty.

        var course = CoursesList.FirstOrDefault(a => a.Id == id);

        if (course == null)
            return "O curso não existe";

        CoursesList.FirstOrDefault(a => a.Id == id)!.Name = name;
        CoursesList.FirstOrDefault(a => a.Id == id)!.WorkLoad = workLoad;

        CoursesList.FirstOrDefault(a => a.Id == id)!.Name =
            name; // Update the course name.
        CoursesList.FirstOrDefault(a => a.Id == id)!.WorkLoad =
            workLoad; // Update the course work load.
        return
            "Curso alterado com sucesso"; // The course was successfully edited.
    }


    /// <summary>
    ///     Searches for a course in the course list by its name or work load.
    /// </summary>
    /// <param name="name">The name of the course to search for.</param>
    /// <param name="workLoad">The work load of the course to search for.</param>
    /// <returns>Returns a list of courses that match the search criteria.</returns>
    public static List<Course> ConsultCourse(
        string name, int workLoad
    )
    {
        var courses = CoursesList;

        if (!string.IsNullOrWhiteSpace(name))
            courses = CoursesList.Where(a => a.Name == name).ToList();

        if (!int.IsNegative(workLoad))
            courses = CoursesList.Where(
                a => a.WorkLoad == workLoad).ToList();

        return courses;
    }


    /// <summary>
    ///     Searches for a course in the course list by its name or work load.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name">The name of the course to search for.</param>
    /// <param name="workLoad">The work load of the course to search for.</param>
    /// <returns>Returns a list of courses that match the search criteria.</returns>
    public static List<Course> ConsultCourse(
        int id, string name, int workLoad
    )
    {
        var courses = CoursesList;

        if (!string.IsNullOrWhiteSpace(name))
            courses = courses.Where(c => c.Name == name).ToList();

        if (workLoad >= 0)
            courses = courses.Where(c => c.WorkLoad == workLoad).ToList();

        return courses;
    }


    /// <summary>
    ///     Searches for a course in the course list by its name or work load.
    /// </summary>
    /// <param name="selectedProperty">The property name to search by.</param>
    /// <param name="selectedValue">The value to search for.</param>
    /// <returns>A list of courses matching the search criteria.</returns>
    public static List<Course> ConsultCourses(
        string selectedProperty, object selectedValue)
    {
        // Get the property with the specified name.
        var property = typeof(Course).GetProperty(selectedProperty);

        // If the property doesn't exist, return an empty list.
        if (property == null) return new List<Course>();

        // Get the type of the property.
        var propertyType = property.PropertyType;

        // Convert the search value to the type of the property.
        object convertedValue;
        try
        {
            convertedValue = Convert.ChangeType(selectedValue, propertyType);
        }
        catch (InvalidCastException ex)
        {
            // Handle invalid cast exception
            Console.WriteLine($"Invalid cast: {ex.Message}");
            return new List<Course>();
        }
        catch (FormatException ex)
        {
            // Handle format exception
            Console.WriteLine($"Invalid format: {ex.Message}");
            return new List<Course>();
        }

        // Return a list of courses where the
        // property value matches the search value.
        return CoursesList
            .Where(c => property.GetValue(c)
                            ?.Equals(convertedValue) ==
                        true)
            .ToList();
    }


    /// <summary>
    ///     Gets the index of the last course in the course list.
    /// </summary>
    /// <returns>
    ///     The index of the last course in the course list,
    ///     or -1 if the list is empty.
    /// </returns>
    public static int GetLastIndex()
    {
        // Get the last course in the course list, if it exists.
        var lastCourse = CoursesList.LastOrDefault();

        // If the list is empty, return -1.
        if (lastCourse == null) return -1;

        // Otherwise, return the index of the last course.
        return lastCourse.Id;
    }


    /// <summary>
    ///     Returns the last ID of a course in the course list,
    ///     or -1 if the list is empty.
    /// </summary>
    /// <returns>The last ID of a course or -1 if the list is empty.</returns>
    public static int GetLastId()
    {
        // Get the last course in the course list or null if the list is empty
        var lastCourse = CoursesList.LastOrDefault();

        // If the last course exists, return its ID;
        // otherwise, get the last index in the list
        return lastCourse?.Id ?? GetLastIndex();
        /*
        return lastCourse != null
            ? lastCourse.ID
            : GetLastIndex();
        */
        // handle the case where the collection is empty
        // return StudentsList[^1].IdStudent;
        // return GetLastIndex();
    }

    /// <summary>
    ///     Gets the full name of a course based on its ID.
    /// </summary>
    /// <param name="id">The ID of the course.</param>
    /// <returns>
    ///     The full name of the course or an error message
    ///     if it doesn't exist or the course list is empty.
    /// </returns>
    public static string GetFullName(int id)
    {
        // If the course list is empty, return an error message
        if (CoursesList.Count < 1)
            return "A lista está vazia";

        // Find the course with the given ID or null if it doesn't exist
        var course = CoursesList.FirstOrDefault(a => a.Id == id);

        // If the course doesn't exist, return an error message
        if (course == null)
            return "O curso não existe!";

        // Return the full name of the course
        return $"{course.Id,5} | {course.Name} {course.Credits}";
    }


    /// <summary>
    ///     Returns a string containing the full information for a given course,
    ///     including its name, credits, workload,
    ///     and the number of students enrolled in it.
    /// </summary>
    /// <param name="id">The ID of the course.</param>
    /// <returns>
    ///     A string containing the full
    ///     information for the course.
    /// </returns>
    public static string GetFullInfo(int id)
    {
        if (CoursesList.Count < 1)
            return "A lista está vazia";

        // Find the course with the specified ID in the course list
        var course = CoursesList.FirstOrDefault(a => a.Id == id);

        if (course == null)
            return "A turma não existe!";

        // Return a string containing the course name, credits,
        // workload, and number of students enrolled
        return
            $"{GetFullName(id)} | {course.WorkLoad} - {course.StudentsCount}";
    }

    /// <summary>
    ///     Calculates the number of students enrolled in each course and
    ///     updates the StudentsCount property of
    ///     each course in the course list accordingly.
    /// </summary>
    /// <returns>
    ///     A string indicating that the
    ///     calculations have been executed.
    /// </returns>
    public static string GetStudentsCount()
    {
        if (CoursesList.Count < 1)
            return "A lista está vazia";

        // Calculate the number of students enrolled in each course
        foreach (var course in CoursesList)
            course.StudentsCount =
                Enrollments.Enrollments.ListEnrollments?
                    .Where(x => x.Id == course.Id)
                    .Distinct()
                    .Count() ?? 0;

        // Return a string indicating that the calculations have been executed
        return "Cálculos executados.";
    }

    #endregion
}