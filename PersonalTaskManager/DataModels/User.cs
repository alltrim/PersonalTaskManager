using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PersonalTaskManager.DataModels
{
    public class User
    {
        public int UserId { get; set; }
        [MaxLength(100), Required]
        public string UserName { get; set; }
        [MaxLength(100), Required]
        public string Password { get; set; }

        public List<Task> UserTasks { get; set; }

    }
}