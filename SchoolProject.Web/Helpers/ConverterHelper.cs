using SchoolProject.Web.Data.Entities;

namespace SchoolProject.Web.Helpers;

public class ConverterHelper : IConverterHelper
{
    public Owner ToOwner(OwnerViewModel ownerViewModel,
        string? filePath, Guid fileStorageId, bool isNew)
    {
        return new Owner
        {
            Id = isNew ? 0 : ownerViewModel.Id,
            Document = ownerViewModel.Document,
            FirstName = ownerViewModel.FirstName,
            LastName = ownerViewModel.LastName,
            ProfilePhotoUrl = filePath,
            ProfilePhotoId = fileStorageId,
            FixedPhone = ownerViewModel.FixedPhone,
            CellPhone = ownerViewModel.CellPhone,
            Address = ownerViewModel.Address,
            User = ownerViewModel.User
        };
    }


    public OwnerViewModel ToOwnerViewModel(Owner owner)
    {
        return new OwnerViewModel
        {
            Id = owner.Id,
            Document = owner.Document,
            FirstName = owner.FirstName,
            LastName = owner.LastName,
            ProfilePhotoUrl = owner.ProfilePhotoUrl,
            ProfilePhotoId = owner.ProfilePhotoId,
            FixedPhone = owner.FixedPhone,
            CellPhone = owner.CellPhone,
            Address = owner.Address,
            User = owner.User
        };
    }


    public Lessee ToLessee(LesseeViewModel lesseeViewModel,
        string? filePath, Guid fileStorageId, bool isNew)
    {
        return new Lessee
        {
            Id = isNew ? 0 : lesseeViewModel.Id,
            Document = lesseeViewModel.Document,
            FirstName = lesseeViewModel.FirstName,
            LastName = lesseeViewModel.LastName,
            ProfilePhotoUrl = filePath,
            ProfilePhotoId = fileStorageId,
            FixedPhone = lesseeViewModel.FixedPhone,
            CellPhone = lesseeViewModel.CellPhone,
            Address = lesseeViewModel.Address,
            User = lesseeViewModel.User
        };
    }


    public LesseeViewModel ToLesseeViewModel(Lessee lessee)
    {
        return new LesseeViewModel
        {
            Id = lessee.Id,
            Document = lessee.Document,
            FirstName = lessee.FirstName,
            LastName = lessee.LastName,
            ProfilePhotoUrl = lessee.ProfilePhotoUrl,
            ProfilePhotoId = lessee.ProfilePhotoId,
            FixedPhone = lessee.FixedPhone,
            CellPhone = lessee.CellPhone,
            Address = lessee.Address,
            User = lessee.User
        };
    }
}