using System.ComponentModel.DataAnnotations;

namespace Library.PL.Areas.Dashboard.ViewModels
{
    public class RoleVM
    {
        public string? Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
