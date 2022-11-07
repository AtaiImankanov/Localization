using lavAspMvclast.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace lavAspMvclast.Services
{
    public class TaskService

    {

        private MobileContext db;
        private  UserManager<User> _userManager;
        private IMemoryCache cache;

        public TaskService(MobileContext context, IMemoryCache memoryCache, UserManager<User> userManager)

        {

            _userManager= userManager;
            db = context;

            cache = memoryCache;

        }


        public void AddUser(ToDoTask toDoTask)
        {

                cache.Set(toDoTask.Id, toDoTask, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });

        }

        public async Task<ToDoTask> GetUser(int? id)

        {

            ToDoTask toDoTask = null;

            if (!cache.TryGetValue(id, out toDoTask))
            {
                toDoTask = await db.ToDoTasks.FirstOrDefaultAsync(m => m.Id == id);
                if (toDoTask != null)
                {
                    cache.Set(toDoTask.Id, toDoTask,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
            }


            return toDoTask;

        }

    }


}
