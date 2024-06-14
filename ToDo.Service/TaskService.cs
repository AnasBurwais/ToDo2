using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Domain.Entities;
using ToDo.Persistance;
using ToDo.ServiceInterface;

namespace ToDo.Service
{
    public class TaskService : ITaskService
    {

        private readonly IDbContextFactory<ToDoContext> _contextFactory;

        public TaskService(IDbContextFactory<ToDoContext> dbContextFactory)
        {
            _contextFactory = dbContextFactory;
        }

        // دالة إضافة مهمة جديدة
        public async Task AddTask(Tasks task)
        {
            var context = _contextFactory.CreateDbContext();
            context.Task.Add(task);
            await context.SaveChangesAsync();
        }


        // دالة حذف مهمة من قائمة المهام
        public async Task DeleteTask(int id)
        {
            using var context = _contextFactory.CreateDbContext(); // Create a DbContext instance
            var rowsDeleted = context.Task.FirstOrDefault(u => u.TaskId == id);
            if (rowsDeleted != null)
            {
                context.Task.Remove(rowsDeleted);
                await context.SaveChangesAsync();
            }


        }

        // دالة جلب جميع المهام للقائمة
        public async Task<List<Tasks>> GetAllTasks()
        {
            using var context = _contextFactory.CreateDbContext();
            return [.. await context.Task.ToListAsync()];
        }

        // دالة إرجاع مهمة عن طريق رقم المعرف الخاص بها
        public async Task<Tasks> GetTaskById(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            var row = await context.Task.FirstOrDefaultAsync(u => u.TaskId == id);
              return row;
        }

        // دالة تعديل مهمة في قائمة المهام
        public async Task UpdateTask(Tasks task)
        {
            using var context = _contextFactory.CreateDbContext();
            var rowsUpdated = context.Task.FirstOrDefault(u => u.TaskId == task.TaskId);

            if (rowsUpdated != null)
            {
                rowsUpdated.TaskName = task.TaskName;
                rowsUpdated.Ststus = task.Ststus;
                await context.SaveChangesAsync();
            }
        }
    }
}
