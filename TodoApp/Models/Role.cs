using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApp.Models
{
    public class Role
    {
        public int Id { get; set; }

        public string RoleName { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}