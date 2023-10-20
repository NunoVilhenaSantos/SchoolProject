using Newtonsoft.Json;

namespace SchoolProject.Web.Helpers;

public class ProcessList
{
    [JsonProperty("listOfProcesses")]
    // Static list to store information about each thread
    public static List<string> ListOfProcesses { get; } = new();
}