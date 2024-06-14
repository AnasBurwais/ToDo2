/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;*/

using ToDo.Domain.Entities;

namespace ToDo.ServiceInterface
{
    public interface IUserService
    {
        
        Task<User> GetById(int id);

        Task<List<User>> GetAllUsers ();

        Task UpdateUser (User user); 

        Task AddUser (User user);

        Task DeleteUser (int id);

        

    }
}
