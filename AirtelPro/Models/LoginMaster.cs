using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DG_Tool.Models
{
    public class LoginMaster
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public int IsActive { get; set; }
        public int Status { get; set; }
        public int IsDeleted { get; set; }
        public int FirstLogin { get; set; }
        public DateTime? LastPasswordChangeDate { get; set; }
    }
}
