using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DAL.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Img { get; set; }
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime? MemberDate { get; set; }
        public DateTime? ExpiryDate { get; set; }

        public bool IsDeleted { get; set; }
    }
}
