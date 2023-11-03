using System.Diagnostics;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.Disciplines;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Entities.Teachers;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Data.Repositories.Countries;
using SchoolProject.Web.Data.Repositories.Genders;
using SchoolProject.Web.Data.Repositories.Students;
using SchoolProject.Web.Data.Repositories.Teachers;
using SchoolProject.Web.Data.Seeders;
using SchoolProject.Web.Helpers.Users;
using SchoolProject.Web.Models.Account;
using SchoolProject.Web.Models.Countries;
using SchoolProject.Web.Models.Courses;
using SchoolProject.Web.Models.Disciplines;
using SchoolProject.Web.Models.Students;
using SchoolProject.Web.Models.Teachers;

namespace SchoolProject.Web.Helpers.ConverterModelClassOrClassModel;

/// <inheritdoc />
public class ConverterHelper : IConverterHelper
{
    // authenticatedUserInApp
    private readonly AuthenticatedUserInApp _authenticatedUserInApp;
    private readonly ICityRepository _cityRepository;

    // dataContext
    private readonly DataContextMySql _context;

    // repository
    private readonly ICountryRepository _countryRepository;

    private readonly IGenderRepository _genderRepository;

    private readonly IStudentRepository _studentRepository;
    private readonly ITeacherRepository _teacherRepository;

    // userHelper
    private readonly IUserHelper _userHelper;


    /// <summary>
    /// </summary>
    /// <param name="authenticatedUserInApp"></param>
    /// <param name="context"></param>
    /// <param name="userHelper"></param>
    /// <param name="countryRepository"></param>
    /// <param name="cityRepository"></param>
    /// <param name="studentRepository"></param>
    /// <param name="teacherRepository"></param>
    /// <param name="genderRepository"></param>
    public ConverterHelper(AuthenticatedUserInApp authenticatedUserInApp,
        DataContextMySql context, IUserHelper userHelper,
        ICountryRepository countryRepository, ICityRepository cityRepository,
        IStudentRepository studentRepository,
        ITeacherRepository teacherRepository,
        IGenderRepository genderRepository)
    {
        _authenticatedUserInApp = authenticatedUserInApp;
        _countryRepository = countryRepository;
        _studentRepository = studentRepository;
        _teacherRepository = teacherRepository;
        _genderRepository = genderRepository;
        _cityRepository = cityRepository;
        _userHelper = userHelper;
        _context = context;
    }


    // public Owner ToOwner(OwnerViewModel ownerViewModel,
    //     string? filePath, Guid fileStorageId, bool isNew)
    // {
    //     return new Owner
    //     {
    //         Id = isNew ? 0 : ownerViewModel.Id,
    //         Document = ownerViewModel.Document,
    //         FirstName = ownerViewModel.FirstName,
    //         LastName = ownerViewModel.LastName,
    //         ProfilePhotoUrl = filePath,
    //         ProfilePhotoId = fileStorageId,
    //         FixedPhone = ownerViewModel.FixedPhone,
    //         CellPhone = ownerViewModel.CellPhone,
    //         Address = ownerViewModel.Address,
    //         AppUser = ownerViewModel.AppUser
    //     };
    // }


    // public OwnerViewModel ToOwnerViewModel(Owner owner)
    // {
    //     return new OwnerViewModel
    //     {
    //         Id = owner.Id,
    //         Document = owner.Document,
    //         FirstName = owner.FirstName,
    //         LastName = owner.LastName,
    //         ProfilePhotoUrl = owner.ProfilePhotoUrl,
    //         ProfilePhotoId = owner.ProfilePhotoId,
    //         FixedPhone = owner.FixedPhone,
    //         CellPhone = owner.CellPhone,
    //         Address = owner.Address,
    //         AppUser = owner.AppUser
    //     };
    // }


    /// <summary>
    /// </summary>
    /// <param name="model"></param>
    /// <param name="filePath"></param>
    /// <param name="fileStorageId"></param>
    /// <param name="isNew"></param>
    /// <returns></returns>
    public Course ToCourse(CourseViewModel model, string? filePath,
        Guid fileStorageId, bool isNew)
    {
        return new Course
        {
            Id = isNew ? 0 : model.Id,
            IdGuid = isNew ? Guid.Empty : model.IdGuid,
            Code = model.Code,
            Acronym = model.Acronym,
            Name = model.Name,
            QnqLevel = model.QnqLevel,
            EqfLevel = model.EqfLevel,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            StartHour = model.StartHour,
            EndHour = model.EndHour,
            PriceForEmployed = model.PriceForEmployed,
            PriceForUnemployed = model.PriceForUnemployed,
            ProfilePhotoId = fileStorageId,
            WasDeleted = model.WasDeleted,

            CreatedBy = isNew
                ? _authenticatedUserInApp.GetAuthenticatedUser().Result
                : model.CreatedBy,
            CreatedAt = isNew
                ? DateTime.UtcNow
                : model.CreatedAt,
            UpdatedBy = isNew
                ? null
                : _authenticatedUserInApp.GetAuthenticatedUser().Result,
            UpdatedAt = isNew
                ? null
                : DateTime.UtcNow
        };
    }


    /// <inheritdoc />
    public CourseViewModel ToCourseViewModel(Course course)
    {
        return new CourseViewModel
        {
            Id = course.Id,
            IdGuid = course.IdGuid,
            Code = course.Code,
            Acronym = course.Acronym,
            Name = course.Name,
            QnqLevel = course.QnqLevel,
            EqfLevel = course.EqfLevel,
            StartDate = course.StartDate,
            EndDate = course.EndDate,
            StartHour = course.StartHour,
            EndHour = course.EndHour,
            PriceForEmployed = course.PriceForEmployed,
            PriceForUnemployed = course.PriceForUnemployed,
            ProfilePhotoId = course.ProfilePhotoId,
            CreatedBy = course.CreatedBy,
            CreatedAt = course.CreatedAt,
            WasDeleted = course.WasDeleted,
            UpdatedBy = course.UpdatedBy,
            UpdatedAt = course.UpdatedAt
        };
    }


    /// <inheritdoc />
    public Discipline ToDiscipline(DisciplinesViewModel model,
        string? filePath, Guid fileStorageId, bool isNew)
    {
        return new Discipline
        {
            Id = isNew ? 0 : model.Id,
            IdGuid = isNew ? Guid.Empty : model.IdGuid,
            Code = model.Code,
            Name = model.Name,
            Description = model.Description,
            Hours = model.Hours,
            CreditPoints = model.CreditPoints,
            ProfilePhotoId = model.ProfilePhotoId,
            Enrollments = model.Enrollments,
            WasDeleted = model.WasDeleted,

            CreatedBy = isNew
                ? _authenticatedUserInApp.GetAuthenticatedUser().Result
                : model.CreatedBy,
            CreatedAt = isNew
                ? DateTime.UtcNow
                : model.CreatedAt,
            UpdatedBy = isNew
                ? null
                : _authenticatedUserInApp.GetAuthenticatedUser().Result,
            UpdatedAt = isNew
                ? null
                : DateTime.UtcNow
        };
    }


    /// <inheritdoc />
    public DisciplinesViewModel ToDisciplineViewModel(Discipline discipline)
    {
        return new DisciplinesViewModel
        {
            Id = discipline.Id,
            IdGuid = discipline.IdGuid,
            Code = discipline.Code,
            Name = discipline.Name,
            Description = discipline.Description,
            Hours = discipline.Hours,
            CreditPoints = discipline.CreditPoints,
            ProfilePhotoId = discipline.ProfilePhotoId,
            Enrollments = discipline.Enrollments,
            WasDeleted = discipline.WasDeleted,
            CreatedBy = discipline.CreatedBy,
            CreatedAt = discipline.CreatedAt,
            UpdatedBy = discipline.UpdatedBy,
            UpdatedAt = discipline.UpdatedAt
        };
    }


    /// <summary>
    /// </summary>
    /// <param name="model"></param>
    /// <param name="filePath"></param>
    /// <param name="fileStorageId"></param>
    /// <param name="isNew"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Student ToStudent(StudentViewModel model, string? filePath,
        Guid fileStorageId, bool isNew)
    {
        var city = _countryRepository.GetCityAsync(model.CityId)
            .FirstOrDefault();
        if (city == null) return null;


        var gender = _genderRepository.GetByIdAsync(model.GenderId)
            .FirstOrDefault();
        if (gender == null) return null;


        var countryOfNationality = _countryRepository
            .GetCountryWithCitiesAsync(model.CountryOfNationalityId)
            .FirstOrDefault();
        if (countryOfNationality == null) return null;


        var birthplace = _countryRepository
            .GetCountryWithCitiesAsync(model.BirthplaceId)
            .FirstOrDefault();
        if (birthplace == null) return null;


        var appUser = _userHelper.GetUserByEmailAsync(model.Email).Result;
        if (appUser == null) return null;


        return new Student
        {
            Id = isNew ? 0 : model.Id,
            IdGuid = isNew ? Guid.Empty : model.IdGuid,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Address = model.Address,
            PostalCode = model.PostalCode,

            CountryId = model.CountryId,
            CityId = city.Id,
            City = city,

            MobilePhone = model.MobilePhone,
            Email = model.Email,
            Active = model.Active,

            GenderId = gender.Id,
            Gender = gender,

            DateOfBirth = model.DateOfBirth,
            IdentificationNumber = model.IdentificationNumber,
            IdentificationType = model.IdentificationType,
            ExpirationDateIdentificationNumber =
                model.ExpirationDateIdentificationNumber,
            TaxIdentificationNumber = model.TaxIdentificationNumber,

            CountryOfNationalityId = countryOfNationality.Id,
            CountryOfNationality = countryOfNationality,

            BirthplaceId = birthplace.Id,
            Birthplace = birthplace,

            EnrollDate = model.EnrollDate,

            UserId = appUser.Id,
            AppUser = appUser,

            ImageFile = null,
            ProfilePhotoId = model.ProfilePhotoId,

            CourseStudents = _studentRepository.GetStudentById(model.Id)
                .FirstOrDefault()?.CourseStudents,
            Enrollments = _studentRepository.GetStudentById(model.Id)
                .FirstOrDefault()?.Enrollments,

            WasDeleted = model.WasDeleted,

            CreatedBy = isNew
                ? _authenticatedUserInApp.GetAuthenticatedUser().Result
                : model.CreatedBy,
            CreatedAt = isNew
                ? DateTime.UtcNow
                : model.CreatedAt,
            UpdatedBy = isNew
                ? null
                : _authenticatedUserInApp.GetAuthenticatedUser().Result,
            UpdatedAt = isNew
                ? null
                : DateTime.UtcNow
        };
    }


    /// <summary>
    /// </summary>
    /// <param name="student"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public StudentViewModel ToStudentViewModel(Student student)
    {
        throw new NotImplementedException();
    }


    /// <summary>
    /// </summary>
    /// <param name="model"></param>
    /// <param name="filePath"></param>
    /// <param name="fileStorageId"></param>
    /// <param name="isNew"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Teacher ToTeacher(TeacherViewModel model, string? filePath,
        Guid fileStorageId, bool isNew)
    {
        throw new NotImplementedException();
    }


    /// <summary>
    /// </summary>
    /// <param name="teacher"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public TeacherViewModel ToTeacherViewModel(Teacher teacher)
    {
        throw new NotImplementedException();
    }


    public AppUser AddUser(
        string firstName, string lastName, string address,
        string email, string cellPhone, string role,
        string password = SeedDb.DefaultPassword)
    {
        var city =
            _context.Cities
                .Include(city => city.Country)
                .FirstOrDefault(e => e.Name == "Faro");
        Debug.Assert(city != null, nameof(city) + " != null");

        var country = city.Country;
        Debug.Assert(country != null, nameof(country) + " != null");


        var user = new AppUser
        {
            FirstName = firstName,
            LastName = lastName,
            Address = address,
            Email = email,
            PhoneNumber = cellPhone,
            UserName = email,
            WasDeleted = false,
            ProfilePhotoId = default
            // Country = country,
            // City = city,
            // CityId = city.Id,
            // SubscriptionId = subscription.Id,
            // Subscription = subscription,
        };

        _context.Users.Add(user);
        _userHelper.AddUserAsync(user, password).Wait();
        _userHelper.AddUserToRoleAsync(user, role).Wait();
        _context.SaveChanges();

        // var result = _context.Users.FirstOrDefault(appUser);

        // Estou gerando o token e já confirmando para este AppUser
        var token = _userHelper.GenerateEmailConfirmationTokenAsync(user)
            .Result;
        _userHelper.ConfirmEmailAsync(user, token).Wait();

        var result = _context.Users
            .Where(e => e.UserName == user.UserName)
            .AsEnumerable()
            .FirstOrDefault();

        return result ?? _context.Users.FirstOrDefault(user);
    }


    /// <inheritdoc />
    public Teacher ToTeacherFromUser(AppUser user)
    {
        var random = new Random();
        var city = _cityRepository.GetCityAsync(1).FirstOrDefault();
        var gender = _genderRepository.GetByIdAsync(1).FirstOrDefault();

        var countryOfNationality = _countryRepository
            .GetCountryWithCitiesAsync(5).FirstOrDefault();

        var birthplace = _countryRepository
            .GetCountryWithCitiesAsync(4).FirstOrDefault();

        return new Teacher
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Address = user.Address ?? string.Empty,
            PostalCode = random.Next(1000, 9999) + "-" + random.Next(000, 999),
            MobilePhone = user.PhoneNumber ?? string.Empty,
            Email = user.Email ?? string.Empty,
            Active = true,
            DateOfBirth = DateTime.Today.AddYears(-18),
            IdentificationNumber =
                random.Next(100_000_000, 999_999_999).ToString(),
            IdentificationType = "C/C",
            ExpirationDateIdentificationNumber = DateTime.Today.AddYears(10),
            TaxIdentificationNumber =
                random.Next(100_000_000, 999_999_999).ToString(),

            EnrollDate = DateTime.Now,
            ProfilePhotoId = default,

            CountryId = city.CountryId,
            City = city,
            CityId = city.Id,
            Gender = gender,
            CountryOfNationality = countryOfNationality,
            Birthplace = birthplace,
            AppUser = user,
            CreatedBy = _authenticatedUserInApp.GetAuthenticatedUser().Result
        };
    }


    /// <inheritdoc />
    public Student ToStudentFromUser(AppUser user)
    {
        var random = new Random();
        var city = _cityRepository.GetCityAsync(1).FirstOrDefault();
        var gender = _genderRepository.GetByIdAsync(1).FirstOrDefault();

        var countryOfNationality = _countryRepository
            .GetCountryWithCitiesAsync(5).FirstOrDefault();

        var birthplace = _countryRepository
            .GetCountryWithCitiesAsync(4).FirstOrDefault();

        return new Student
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Address = user.Address ?? string.Empty,
            PostalCode = random.Next(1000, 9999) + "-" + random.Next(000, 999),
            MobilePhone = user.PhoneNumber ?? string.Empty,
            Email = user.Email ?? string.Empty,
            Active = true,
            DateOfBirth = DateTime.Today.AddYears(-18),
            IdentificationNumber =
                random.Next(100_000_000, 999_999_999).ToString(),
            IdentificationType = "C/C",
            ExpirationDateIdentificationNumber = DateTime.Today.AddYears(10),
            TaxIdentificationNumber =
                random.Next(100_000_000, 999_999_999).ToString(),

            EnrollDate = DateTime.Now,
            ProfilePhotoId = default,

            CountryId = city.CountryId,
            City = city,
            CityId = city.Id,
            Gender = gender,
            CountryOfNationality = countryOfNationality,
            Birthplace = birthplace,
            AppUser = user,
            CreatedBy = _authenticatedUserInApp.GetAuthenticatedUser().Result
        };
    }


    /// <summary>
    /// </summary>
    /// <param name="model"></param>
    /// <param name="isNew"></param>
    /// <returns></returns>
    public AppUser ViewModelToUser(AppUserViewModel model, bool isNew)
    {
        return new AppUser
        {
            Id = isNew ? string.Empty : model.Id,
            UserName = model.Email,
            Email = model.Email,
            EmailConfirmed = model.EmailConfirmed,
            PhoneNumber = model.PhoneNumber,
            FirstName = model.FirstName,
            LastName = model.LastName,
            // Country = null,
            // City = model.City,
            // CityId = model.CityId,
            // Subscription = model.Subscription,
            // SubscriptionId = model.SubscriptionId,
            WasDeleted = false,
            ProfilePhotoId = default
        };
    }


    /// <summary>
    /// </summary>
    /// <param name="model"></param>
    /// <param name="appUser"></param>
    /// <param name="isNew"></param>
    /// <returns></returns>
    public AppUser ViewModelToUser(
        AppUserViewModel model, AppUser appUser, bool isNew)
    {
        return new AppUser
        {
            Id = isNew ? string.Empty : model.Id,
            UserName = model.Email,
            Email = model.Email,
            EmailConfirmed = model.EmailConfirmed,
            PhoneNumber = model.PhoneNumber,
            FirstName = model.FirstName,
            LastName = model.LastName,
            // Country = null,
            // City = model.City,
            // CityId = model.CityId,
            // Subscription = model.Subscription,
            // SubscriptionId = model.SubscriptionId,
            WasDeleted = false,
            ProfilePhotoId = default
        };
    }


    /// <summary>
    /// </summary>
    /// <param name="appUser"></param>
    /// <param name="role"></param>
    /// <returns></returns>
    public AppUserViewModel UserToViewModel(AppUser appUser, string role)
    {
        return new AppUserViewModel
        {
            Id = appUser.Id,
            Email = appUser.Email,
            EmailConfirmed = appUser.EmailConfirmed,
            PhoneNumber = appUser.PhoneNumber,
            FirstName = appUser.FirstName,
            LastName = appUser.LastName,
            HasPhoto = appUser.ProfilePhotoId != Guid.Empty,
            ProfilePhotoId = appUser.ProfilePhotoId,
            Role = role,
            WasDeleted = false
            // SubscriptionId = appUser.SubscriptionId,
            // Subscription = appUser.Subscription,
            // City = appUser.City,
            // CityId = appUser.CityId,
        };
    }


    /// <summary>
    /// </summary>
    /// <param name="model"></param>
    /// <param name="profilePhotoId"></param>
    /// <param name="isNew"></param>
    /// <returns></returns>
    public Country ToCountry(
        CountryViewModel model, Guid profilePhotoId, bool isNew)
    {
        return new Country
        {
            Id = isNew ? 0 : model.Id,
            Cities = model.Cities,
            Name = model.Name,
            Nationality = model.Nationality,
            ProfilePhotoId = profilePhotoId,

            CreatedBy = isNew
                ? _authenticatedUserInApp.GetAuthenticatedUser().Result
                : model.CreatedBy,
            CreatedAt = isNew
                ? DateTime.UtcNow
                : model.CreatedAt,
            UpdatedBy = isNew
                ? null
                : _authenticatedUserInApp.GetAuthenticatedUser().Result,
            UpdatedAt = isNew
                ? null
                : DateTime.UtcNow
        };
    }

    /// <summary>
    /// </summary>
    /// <param name="country"></param>
    /// <returns></returns>
    public CountryViewModel ToCountryViewModel(Country country)
    {
        return new CountryViewModel
        {
            Id = country.Id,
            Name = country.Name,
            CountryId = country.Id,
            CountryName = country.Name,

            NationalityId = country.Nationality.Id,
            NationalityName = country.Nationality.Name,
            Nationality = country.Nationality,

            Cities = country.Cities,
            ProfilePhotoId = country.ProfilePhotoId,

            WasDeleted = country.WasDeleted,
            CreatedBy = country.CreatedBy,
            CreatedAt = country.CreatedAt,
            UpdatedBy = country.UpdatedBy,
            UpdatedAt = country.UpdatedAt
        };
    }
}