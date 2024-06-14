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
    public class UserService : IUserService
    {
        private readonly IDbContextFactory<ToDoContext> _contextFactory;

        public UserService(IDbContextFactory<ToDoContext> dbContextFactory)
        {
            _contextFactory = dbContextFactory;
        }
        
        // إضافة مستخدم جديد للنظام
        public async Task  AddUser(User user)
        {
            var context = _contextFactory.CreateDbContext();
            context.User.Add(user);
           await context.SaveChangesAsync();
        }

        // تعديل بيانات لمستخدم موجود في النظام
        public async Task UpdateUser(User user)
        {
            using var context = _contextFactory.CreateDbContext();
            var rowsUpdated = context.User.FirstOrDefault(u => u.UserId == user.UserId);

            if (rowsUpdated != null)
            {
                rowsUpdated.UserName = user.UserName;
                rowsUpdated.Password = user.Password;
                await context.SaveChangesAsync();
            }
        }

        // حذف مستخدم من النظام
        public async Task DeleteUser(int id)
        {
            using var context = _contextFactory.CreateDbContext(); // Create a DbContext instance
            var rowsDeleted = context.User.FirstOrDefault(u => u.UserId == id);
            if (rowsDeleted != null)
            {
                context.User.Remove(rowsDeleted);
               await context.SaveChangesAsync();
            }
            

            
            
        }

        // الاستعلام عن مستخدم معين في النظام
        public async Task<User> GetById(int id)
        {
          using var context = _contextFactory.CreateDbContext();
          var row = await context.User.FirstOrDefaultAsync(u => u.UserId == id);
            return row;
        }

        // جلب بيانات جميع المستخدمين في النظام
        public async Task<List<User>> GetAllUsers()
        {
            using var context = _contextFactory.CreateDbContext();
            return [.. await context.User.ToListAsync()];
        }

      

        

        
    }
}
