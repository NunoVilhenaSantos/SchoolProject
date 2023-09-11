using SchoolProject.Web.Data.Entities.Disciplines;

namespace SchoolProject.Web.Models.Disciplines;

public class DisciplinesPagesViewModel
{
    public required IQueryable<Discipline> Records { get; set; }


    public required int PageNumber { get; set; }


    public required int PageSize { get; set; }


    public required int TotalCount { get; set; }
}