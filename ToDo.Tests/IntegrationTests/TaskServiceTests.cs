using Microsoft.EntityFrameworkCore;
using Moq;
using ToDo.Domain.Entities;
using ToDo.Persistance;
using ToDo.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ToDo.Tests.IntegrationTests
{
    public class TaskServiceTests
    {

        private readonly IDbContextFactory<ToDoContext> _factory;

        // الهدف هو ضمان أن كل مرة يتم فيها استدعاء الدالة، يتم إنشاء قاعدة
        // بيانات جديدة وفريدة في الذاكرة للاستخدام في الاختبارات أو لأغراض أخرى
        private DbContextOptions<ToDoContext> CreateNewContextOptions()
        {
            return new DbContextOptionsBuilder<ToDoContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        private IDbContextFactory<ToDoContext> GetDbContextFactoryAsync(DbContextOptions<ToDoContext> options)
        {
            var mockDbFactory = new Mock<IDbContextFactory<ToDoContext>>();
            mockDbFactory.Setup(f => f.CreateDbContext()).Returns(() => new ToDoContext(options));
            return mockDbFactory.Object;

        }


        // اختبار دالة الاضافة
        [Fact]
        public async Task AddTask_ShouldAddTask()
        {
            // Arrange // تجهيز الاختبار
            var options = CreateNewContextOptions();
            var factory = GetDbContextFactoryAsync(options);
            var service = new TaskService(factory);
           
            var task = new Tasks { TaskName = "Task1", Ststus = false  };

            // Act // استدعاء الدالة
            await service.AddTask(task);

            // Assert // التأكد
            using var context = new ToDoContext(options);
            var savedTask = await context.Task.FirstOrDefaultAsync(a => a.TaskName == "Task1");
            Assert.NotNull(savedTask);
        }

        // اختبار دالة حذف
        [Fact]
        public async Task Delete_ShouldRemoveTask()
        {
            // Arrange
            var options = CreateNewContextOptions();
            var factory = GetDbContextFactoryAsync(options);
            var service = new TaskService(factory);
            var task = new Tasks { TaskName = "Task1", Ststus = false };
            await service.AddTask(task);

            // Act
            await service.DeleteTask(task.TaskId);

            // Assert
            using var context = new ToDoContext(options);
            var deletedAuthor = await context.Task.FindAsync(task.TaskId);
            Assert.Null(deletedAuthor);
        }


        // اختبار دالة إرجاع جميع المهام
        [Fact]
     public async Task GetAllTasks_ShouldReturnAllTasks()
        {
            // Arrange
            var options = CreateNewContextOptions();
            var factory = GetDbContextFactoryAsync(options);
            var service = new TaskService(factory);
            await service.AddTask(new Tasks { TaskName = "Task1", Ststus = false });
            await service.AddTask(new Tasks { TaskName = "Task2", Ststus = false });

            // Act
            var authors = await service.GetAllTasks();

            // Assert
            Assert.Equal(2, authors.Count);
        }

        // اختبار دالة إرجاع مهمة بالمعرف
        [Fact]
        public async Task GetTaskById_ShouldReturnTaskById()
        {
            // Arrange
            var options = CreateNewContextOptions();
            var factory = GetDbContextFactoryAsync(options);
            var service = new TaskService(factory);
            var task = new Tasks { TaskName = "Task1", Ststus = false };
            await service.AddTask(task);

            // Act
            var fetchedTask = await service.GetTaskById(task.TaskId);

            // Assert
            Assert.NotNull(fetchedTask);
            Assert.Equal(task.TaskName, fetchedTask.TaskName);
        }

        // اختبار دالة التعديل
        [Fact]
        public async Task UpdateTask_ShouldModifyTask()
        {
            // Arrange
            var options = CreateNewContextOptions();
            var factory = GetDbContextFactoryAsync(options);
            var service = new TaskService(factory);
            var task = new Tasks { TaskName = "Task1", Ststus = false };
            await service.AddTask(task);

            // Act
            task.TaskName = "Updated Task1";
            task.Ststus = false;
            
            await service.UpdateTask(task);

            // Assert
            using var context = new ToDoContext(options);
            var updatedTask = await context.Task.FindAsync(task.TaskId);
            Assert.Equal("Updated Task1", updatedTask.TaskName);
            Assert.Equal(false, updatedTask.Ststus);
        
        }

    }

}
