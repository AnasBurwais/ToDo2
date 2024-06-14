using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Domain.Entities
{
    public class User
    {
        
        public int UserId { get; set; }

        [MaxLength(50)]
        public string UserName { get; set; }
        [MaxLength(10)]
        public string Password { get; set; }
        
        public string Email { get; set; }

        public List<Tasks> Task { get; set; }


    }
}
