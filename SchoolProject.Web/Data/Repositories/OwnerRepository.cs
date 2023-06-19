using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.Entities;
using SchoolProject.Web.Data.Repositories.Interfaces;

namespace SchoolProject.Web.Data.Repositories;

public class OwnerRepository : GenericRepository<Owner>, IOwnerRepository
{
    public OwnerRepository(DataContext dataContext) : base(dataContext)
    {
    }
}