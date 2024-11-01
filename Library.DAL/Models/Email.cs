using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DAL.Models
{
    public class Email
    {
        public string Receiver { get; set; } 
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
