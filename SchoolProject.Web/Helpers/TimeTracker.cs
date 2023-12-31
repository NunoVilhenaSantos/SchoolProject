﻿using System.Diagnostics;

namespace SchoolProject.Web.Helpers;

public static class TimeTracker
{
    internal const string SeederTimerName = "SeederTimer";
    internal const string AppBuilderTimerName = "AppBuilderTimer";

    internal const string SeedDbDisciplinesName = "SeedDbCoursesTimer";
    internal const string SeedDbDisciplinesMPInfoName = "SCMPInfoTimer";

    private static readonly Dictionary<string, Stopwatch> Timers = new();


    // Method to create a new timer with the given name
    public static void CreateTimer(string timerName)
    {
        if (!Timers.ContainsKey(timerName))
            Timers[timerName] = new Stopwatch();
    }


    // Method to start a timer with the given name
    public static void StartTimer(string timerName)
    {
        if (Timers.TryGetValue(timerName, out var timer))
            timer.Start();
    }


    // Method to stop a timer with the given name
    public static void StopTimer(string timerName)
    {
        if (Timers.TryGetValue(timerName, out var timer))
            timer.Stop();
    }


    // Method to get the elapsed time of a timer with the given name
    public static TimeSpan GetElapsedTime(string timerName)
    {
        return Timers.TryGetValue(timerName, out var timer)
            ? timer.Elapsed
            : TimeSpan.Zero;
    }


    // Method to reset a timer with the given name
    public static void ResetTimer(string timerName)
    {
        if (Timers.TryGetValue(timerName, out var timer))
            timer.Reset();
    }


    // Method to check if a timer with the given name exists
    public static bool TimerExists(string timerName)
    {
        return Timers.ContainsKey(timerName);
    }


    // Method to print the elapsed time of a timer with the given name
    internal static void PrintTimerToConsole(string timerName)
    {
        if (!Timers.TryGetValue(timerName, out var timer)) return;

        Console.WriteLine(
            $"Elapsed time for timer {timerName}: {timer.Elapsed}");

        // Format and display the TimeSpan value.
        Console.WriteLine(
            "\nElapsed time for timer {timerName}: " +
            "horas, minutos, segundos, milesimos de segundos");

        var elapsedTime =
            $"{timer.Elapsed.Hours:00}:{timer.Elapsed.Minutes:00}:" +
            $"{timer.Elapsed.Seconds:00}.{timer.Elapsed.Milliseconds:00}";

        Console.WriteLine(
            $"Elapsed time for timer {timerName}: " + elapsedTime);
    }
}