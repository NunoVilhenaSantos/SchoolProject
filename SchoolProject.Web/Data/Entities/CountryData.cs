using System.Collections;
using Microsoft.Build.Framework;

namespace SchoolProject.Web.Data.Entities;

public class CountryData
{
    [Required] public string Name { get; set; }

    [Required] public bool Independent { get; set; }

    public class CountryDataCollection : List<CountryData>
    {
    }
}