/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
*/
using ToDo.Domain.Entities;

namespace ToDo.ServiceInterface
{
    public interface ITaskService
    {
        Task AddTask(Tasks task);

        Task DeleteTask(int id);

        Task<List<Tasks>> GetAllTasks(); 

        Task<Tasks> GetTaskById(int id);

        Task UpdateTask(Tasks task);
    }
}
