using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EfCore.Models;

namespace EfCore.Models
{
    public class Duty
    {
        [Key]
        public int DutyId { get; set; }

        [Required(ErrorMessage = "Görev başlığı zorunludur.")]
        [Display(Name = "Görev Başlığı")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Görev açıklaması zorunludur.")]
        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Kullanıcı atanması zorunludur.")]
        [Display(Name = "Kullanıcı")]
        public Guid UserId { get; set; }  // Guid olarak değiştirdik

        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}