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
    public class UserServiceTests
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
        public async Task AddUser_ShouldAddUser()
        {
            // Arrange // تجهيز الاختبار
            var options = CreateNewContextOptions();
            var factory = GetDbContextFactoryAsync(options);
            var service = new UserService(factory);

            var user = new User { UserName = "AnasBurwais", Password = "123", Email = "burwais@gmail.com"};

            // Act // استدعاء الدالة
            await service.AddUser(user);

            // Assert // التأكد
            using var context = new ToDoContext(options);
            var savedUser = await context.User.FirstOrDefaultAsync(a => a.UserName == "AnasBurwais");
            Assert.NotNull(savedUser);
        }

        // اختبار دالة تعديل المستخدم
        [Fact]
        public async Task UpdateUser_ShouldModifyUser()
        {
            // Arrange
            var options = CreateNewContextOptions();
            var factory = GetDbContextFactoryAsync(options);
            var service = new UserService(factory);
            var user = new User { UserName = "AnasBurwais", Password = "123", Email = "burwais@gmail.com" };
            await service.AddUser(user);

            // Act
            user.UserName = "Updated AnasBurwais";
            user.Password = "Updated 123";
            user.Email = "Updated burwais@gmail.com";

            await service.UpdateUser(user);

            // Assert
            using var context = new ToDoContext(options);
            var updatedUser = await context.User.FindAsync(user.UserId);
            Assert.Equal("Updated AnasBurwais", updatedUser.UserName);
            Assert.Equal("Updated 123", updatedUser.Password);
            //Assert.Equal("Updated burwais@gmail.com", updatedUser.Email);


        }

        // اختبار دالة حذف
        [Fact]
        public async Task DeleteUser_ShouldRemoveUser()
        {
            // Arrange
            var options = CreateNewContextOptions();
            var factory = GetDbContextFactoryAsync(options);
            var service = new UserService(factory);
            var user = new User { UserName = "AnasBurwais", Password = "123", Email = "burwais@gmail.com" };
            await service.AddUser(user);

            // Act
            await service.DeleteUser(user.UserId);

            // Assert
            using var context = new ToDoContext(options);
            var deletedUser = await context.Task.FindAsync(user.UserId);
            Assert.Null(deletedUser);
        }



        // اختبار دالة إرجاع مستخدم بالمعرف
        [Fact]
        public async Task GetById_ShouldReturnUserById()
        {
            // Arrange
            var options = CreateNewContextOptions();
            var factory = GetDbContextFactoryAsync(options);
            var service = new UserService(factory);
            var user = new User { UserName = "AnasBurwais", Password = "123", Email = "burwais@gmail.com" };
            await service.AddUser(user);

            // Act
            var fetchedUser = await service.GetById(user.UserId);

            // Assert
            Assert.NotNull(fetchedUser);
            Assert.Equal(user.UserName, fetchedUser.UserName);
        }


        // اختبار دالة إرجاع جميع المستخدمين
        [Fact]
        public async Task GetAllUsers_ShouldReturnAllUsers()
        {
            // Arrange
            var options = CreateNewContextOptions();
            var factory = GetDbContextFactoryAsync(options);
            var service = new UserService(factory);
            await service.AddUser(new User { UserName = "AnasBurwais", Password = "123", Email = "burwais@gmail.com" });
            await service.AddUser(new User { UserName = "AnasBurwais2", Password = "1234", Email = "burwais2@gmail.com" });
            // Act
            var users = await service.GetAllUsers();

            // Assert
            Assert.Equal(2, users.Count);
        }
    }
}
