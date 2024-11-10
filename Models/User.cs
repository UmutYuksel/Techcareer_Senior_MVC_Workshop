using System.ComponentModel.DataAnnotations;

namespace EfCore.Models
{
    public class User
{
    [Key]
    public Guid UserId { get; set; }  // UserId'yi Guid olarak bırakıyoruz.

    [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
    [Display(Name = "Kullanıcı Adı")]
    public string? Name { get; set; }

    public ICollection<Duty> Duties { get; set; } = new List<Duty>();
}

}