using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PersonalTaskManager.DataModels;
using System.Web.Security;

namespace PersonalTaskManager.Controllers
{
    public class TasksController : ApiController
    {
        // GET api/tasks
        [Authorize]
        public IEnumerable<object> Get()
        {
            List<object> tasks = new List<object>();

            if (User.Identity.IsAuthenticated)
            {
                string userName = User.Identity.Name;
                return getTasksFor(userName);
            }

            return new object[] { };

        }

        // GET api/tasks/5
        public string Get(int id)
        {
            return "";
        }

        // POST api/tasks
        [Authorize]
        public IEnumerable<object> Post([FromBody]Task value)
        {
            if (User.Identity.IsAuthenticated)
            {
                string userName = User.Identity.Name;

                using (TasksContext context = new TasksContext())
                {
                    value.Owner = context.Users.Where(user => user.UserName == userName).First();
                    value.LastUpdate = DateTime.Now;

                    if (value.TaskId == 0)
                    {
                        context.Tasks.Add(new Task {
                            Title = value.Title,
                            Content = value.Content,
                            Owner = value.Owner,
                            LastUpdate = value.LastUpdate
                        });
                    }
                    else
                    {
                        var taskEntity = context.Tasks.Where(task => task.TaskId == value.TaskId).First();
                        taskEntity.Title = value.Title;
                        taskEntity.Content = value.Content;
                        taskEntity.LastUpdate = value.LastUpdate;
                    }

                    context.SaveChanges();

                }

                return getTasksFor(userName);

            }

            return new object[] { };

        }

        // PUT api/tasks/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/tasks/5
        [Authorize]
        public IEnumerable<object> Delete(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                string userName = User.Identity.Name;

                using (TasksContext context = new TasksContext())
                {
                    var taskEntity = context.Tasks.Where(task => task.TaskId == id).First();
                    context.Tasks.Remove(taskEntity);
                    context.SaveChanges();

                }

                return getTasksFor(userName);

            }

            return new object[] { };

        }

        // Список задач по имени пользователя
        private IEnumerable<object> getTasksFor(string userName)
        {
            List<object> tasks = new List<object>();

            using (TasksContext context = new TasksContext())
            {
                var query = from items in context.Tasks
                            where items.Owner.UserName == userName
                            orderby items.LastUpdate ascending
                            select items;

                foreach (var item in query)
                {
                    tasks.Add(new
                    {
                        TaskId = item.TaskId,
                        Title = item.Title,
                        Content = item.Content,
                        LastUpdate = item.LastUpdate.ToString(@"yyyy-MM-dd HH:mm")
                    });
                }

            }

            return tasks;

        }

    }
}