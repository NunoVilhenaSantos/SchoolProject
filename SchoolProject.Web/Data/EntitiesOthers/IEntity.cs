using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolProject.Web.Data.Entities.Users;

namespace SchoolProject.Web.Data.EntitiesOthers;

/// <summary>
///
/// </summary>
public interface IEntity
{
    /// <summary>
    ///
    /// </summary>
    [Column(Order = 0)]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    ///
    /// </summary>
    [Column(Order = 1)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid IdGuid { get; set; }


    /// <summary>
    ///
    /// </summary>
    [Column(Order = 20)]
    [Required]
    [DisplayName("Was Deleted?")]
    public bool WasDeleted { get; set; }


    /// <summary>
    ///
    /// </summary>
    [Required]
    [Column(Order = 21)]
    [DisplayName("Created At")]
    [DataType(DataType.Date)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; }


    /// <summary>
    ///
    /// </summary>
    [Required]
    [Column(Order = 22)]
    [DisplayName("Created By")]
    public User CreatedBy { get; set; }


    // [Required]
    /// <summary>
    ///
    /// </summary>
    [Column(Order = 23)]
    [DisplayName("Updated At")]
    [DataType(DataType.Date)]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime? UpdatedAt { get; set; }


    /// <summary>
    ///
    /// </summary>
    [Column(Order = 24)]
    [Required]
    [DisplayName("Updated By")]
    [DataType(DataType.Date)]
    public User? UpdatedBy { get; set; }
}