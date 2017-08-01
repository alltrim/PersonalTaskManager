using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PersonalTaskManager.DataModels
{
    public class Task
    {
        public int TaskId { get; set; }
        [MaxLength(100), Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public DateTime LastUpdate { get; set; }

        [Required]
        public User Owner { get; set; }

    }
}