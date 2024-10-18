using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.PL.Areas.Dashboard.ViewModels
{
    public class ChangeRoleVM
    {
        public string Id { get; set; }
        public string? Email { get; set; } = null!;

        public string? SelectedRole { get; set; } = null!;
        public List<SelectListItem>? selectLists { get; set; }

    }
}
