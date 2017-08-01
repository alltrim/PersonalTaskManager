namespace PersonalTaskManager.DataModels
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class TasksContext : DbContext
    {
        
        public TasksContext()
            : base("name=TasksContext")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Task> Tasks { get; set; }

    }

}