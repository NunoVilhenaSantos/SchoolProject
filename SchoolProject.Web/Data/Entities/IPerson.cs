namespace SchoolProject.Web.Data.Entities
{
    public interface IPerson: IEntity
    {
        

            string Document { get; set; }
            string FirstName { get; set; }
            string LastName { get; set; }
            string? ProfilePhotoUrl { get; set; }
            string? ProfilePhotoFullUrl { get; }
            Guid ProfilePhotoId { get; set; }
            string ProfilePhotoIdUrl { get; }
            string? FixedPhone { get; set; }
            string? CellPhone { get; set; }
            string? Address { get; set; }
            string FullName { get; }
            User? User { get; set; }
        


    }
}
