using AgDataCodingAssignment.Application.Models.Dtos;
using AgDataCodingAssignment.Persistence.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace AgDataCodingAssignment.Test
{
    public class AgDataCodingAssignmentTest
    {
        public IMemoryCache? GetSystemUnderTest()
        {

            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();

            var memoryCache = serviceProvider.GetService<IMemoryCache>();
            return memoryCache;
        }
        [Fact]
        public async Task Add_CreateUser_ReturnsUser()
        {
            var memorycache = GetSystemUnderTest();
            var memoryCacheRepo = new MemoryCacheRepository(memorycache);
            var userRepository = new UserRepository(memoryCacheRepo);


            var user = new CreateUserDto { Name = "Amir", Address = "Guelph Ontario" };
            var result = await userRepository.CreateOneAsync(user);
            Assert.NotNull(result);
            Assert.Equal("Amir", result.Name);
            Assert.Equal("Guelph Ontario", result.Address);
        }
        [Fact]
        public async Task Add_CreateDuplicateUser_ReturnsNull()
        {
            var memorycache = GetSystemUnderTest();
            var memoryCacheRepo = new MemoryCacheRepository(memorycache);
            var userRepository = new UserRepository(memoryCacheRepo);


            var user = new CreateUserDto { Name = "Amir", Address = "Guelph Ontario" };
            var result = await userRepository.CreateOneAsync(user);

            var user2 = new CreateUserDto { Name = "Amir", Address = "Waterloo Ontario" };
            var result2 = await userRepository.CreateOneAsync(user2);

            Assert.NotNull(result);
            Assert.Equal("Amir", result.Name);
            Assert.Equal("Guelph Ontario", result.Address);
            Assert.Null(result2);
        }

        [Fact]
        public async Task Get_GetOneUser_ReturnsUser()
        {
            var memorycache = GetSystemUnderTest();
            var memoryCacheRepo = new MemoryCacheRepository(memorycache);
            var userRepository = new UserRepository(memoryCacheRepo);


            var user = new CreateUserDto { Name = "Amir", Address = "Guelph Ontario" };
            await userRepository.CreateOneAsync(user);
            var result = await userRepository.GetOneAsync(new GetUserDto { Name="Amir"});


            Assert.NotNull(result);
            Assert.Equal("Amir", result.Name);
            Assert.Equal("Guelph Ontario", result.Address);
        }
        [Fact]
        public async Task Get_GetNonExisitingUser_ReturnsNull()
        {
            var memorycache = GetSystemUnderTest();
            var memoryCacheRepo = new MemoryCacheRepository(memorycache);
            var userRepository = new UserRepository(memoryCacheRepo);


            var user = new CreateUserDto { Name = "Amir", Address = "Guelph Ontario" };
            await userRepository.CreateOneAsync(user);
            var result = await userRepository.GetOneAsync(new GetUserDto { Name = "Chris" });


            Assert.Null(result);
        }
        [Fact]
        public async Task Update_UpdateOneUser()
        {
            var memorycache = GetSystemUnderTest();
            var memoryCacheRepo = new MemoryCacheRepository(memorycache);
            var userRepository = new UserRepository(memoryCacheRepo);


            var user = new CreateUserDto { Name = "Amir", Address = "Guelph Ontario" };
            await userRepository.CreateOneAsync(user);
            var result = await userRepository.UpdateOneAsync(new UpdateUserDto{ Name = "Amir" ,Address="Waterloo Ontario"});


            Assert.NotNull(result);
            Assert.Equal("Amir", result.Name);
            Assert.Equal("Waterloo Ontario", result.Address);
        }
        [Fact]
        public async Task Update_UpdateNonExisitingUser_ReturnsNull()
        {
            var memorycache = GetSystemUnderTest();
            var memoryCacheRepo = new MemoryCacheRepository(memorycache);
            var userRepository = new UserRepository(memoryCacheRepo);


            var user = new CreateUserDto { Name = "Amir", Address = "Guelph Ontario" };
            await userRepository.CreateOneAsync(user);
            var result = await userRepository.UpdateOneAsync(new UpdateUserDto { Name = "Chris", Address = "Waterloo Ontario" });


            Assert.Null(result);
            
        }
        [Fact]
        public async Task Delete_DeleteOneUser()
        {
            var memorycache = GetSystemUnderTest();
            var memoryCacheRepo = new MemoryCacheRepository(memorycache);
            var userRepository = new UserRepository(memoryCacheRepo);


            var user = new CreateUserDto { Name = "Amir", Address = "Guelph Ontario" };
            await userRepository.CreateOneAsync(user);
            var result = await userRepository.DeleteOneAsync(new DeleteUserDto { Name = "Amir"});
            var getOneResult = await userRepository.GetOneAsync(new GetUserDto { Name = "Amir" });

            Assert.True(result);
            Assert.Null(getOneResult);
           
        }
        [Fact]
        public async Task Delete_DeleteNonExisitingUser_ReturnsFalse()
        {
            var memorycache = GetSystemUnderTest();
            var memoryCacheRepo = new MemoryCacheRepository(memorycache);
            var userRepository = new UserRepository(memoryCacheRepo);


            var user = new CreateUserDto { Name = "Amir", Address = "Guelph Ontario" };
            await userRepository.CreateOneAsync(user);
            var result = await userRepository.DeleteOneAsync(new DeleteUserDto { Name = "Chris" });

            Assert.False(result);

        }
    }
}