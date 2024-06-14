using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Domain.Entities
{
    public class Tasks
    {
        [Key] public int TaskId { get; set; }
        public string TaskName { get; set; }
     
        public bool Ststus { get; set; }
        public User User { get; set; }// مفتاح خارجي يشير إلى معرف المستخدم الذي ينفذ المهمة
        
    }
}

