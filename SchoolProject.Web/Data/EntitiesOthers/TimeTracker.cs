namespace SchoolProject.Web.Data.EntitiesOthers;

public static class TimeTracker
{
    public static TimeSpan RunSeedingElapsedSeconds { get; set; }


    public static TimeSpan
        SeedDbCoursesListAddingDataElapsedSeconds { get; set; }


    public static TimeSpan
        SeedDbCoursesListMergeAndPrintCourseInfoElapsedSeconds { get; set; }


    public static TimeSpan StartProgramElapsedTime { get; set; }
}