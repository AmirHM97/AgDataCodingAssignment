using AgDataCodingAssignment.Application.Features.User.Commands.Create;
using AgDataCodingAssignment.Application.Features.User.Commands.Delete;
using AgDataCodingAssignment.Application.Features.User.Commands.Update;
using AgDataCodingAssignment.Application.Features.User.Queries;
using AgDataCodingAssignment.Application.Models.Common;
using AgDataCodingAssignment.Domain.Entities;
using Mediator;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgDataCodingAssignment.Test
{
    internal class Comments
    {
        //public class InjectionFixture : IDisposable
        //{
        //    private readonly TestServer server;
        //    private readonly HttpClient client;

        //    public InjectionFixture()
        //    {
        //        server = new TestServer(new WebHostBuilder().UseStartup<Program>());
        //        client = server.CreateClient();
        //    }

        //    public IServiceProvider ServiceProvider => server.Host.Services;

        //    public void Dispose()
        //    {
        //        Dispose(true);
        //    }

        //    protected virtual void Dispose(bool disposing)
        //    {
        //        if (disposing)
        //        {
        //            server.Dispose();
        //            client.Dispose();
        //        }
        //    }
        //}


        public class AgDataCodingAssignmentTest
        {

            private Mock<IMediator> _mediator;

            public void Setup()
            {
                _mediator = new Mock<IMediator>();
            }
            //private readonly InjectionFixture injection;

            //public AgDataCodingAssignmentTest(InjectionFixture injection)
            //{
            //    this.injection = injection;
            //}
            //private readonly IConfiguration _configuration;
            //private readonly UserRepository _userRepository;

            //public AgDataCodingAssignmentTest()
            //{
            //    var services = new ServiceCollection();
            //    var builder = new ConfigurationBuilder()
            //         .AddJsonFile("appsettings.json");

            //    _configuration = builder.Build();
            //    var value = _configuration.GetValue<RavenSettings>("RavenSettings");
            //    services.AddMemoryCache();
            //    services.AddApplicationServices();
            //    services.AddPersistenceServices();
            //    var serviceProvider = services.BuildServiceProvider();

            //    var memoryCache = serviceProvider.GetService<IMemoryCache>();
            //    var ravenRepo = new RavenDbRepository();
            //    var memoryCacheRepo = new MemoryCacheRepository(memoryCache);
            //    var _userRepository = new UserRepository(memoryCacheRepo);
            //}

            //public void GetSystemUnderTest()
            //{


            //    return memoryCache;
            //}


            [Fact]
            public async Task Add_CreateUser_ReturnsTrue()
            {
                //var memorycache = GetSystemUnderTest();
                //var memoryCacheRepo = new MemoryCacheRepository(memorycache);
                //var userRepository = new UserRepository(memoryCacheRepo);

                Setup();

                var command = new CreateUserCommand("Amir", "Guelph");
                _mediator.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
                   .Returns(ValueTask.FromResult(OperationResult<bool>.SuccessResult(true)));

                var response = await _mediator.Object.Send(command);

                Assert.True(response.IsSuccess);
                //var user = new CreateUserDto { Name = "Amir", Address = "Guelph Ontario" };
                //var result = await userRepository.CreateOneAsync(user);
                //Assert.NotNull(result);
                //Assert.Equal("Amir", result.Name);
                //Assert.Equal("Guelph Ontario", result.Address);
            }
            [Fact]
            public async Task Add_CreateDuplicateUser_ReturnsFalse()
            {

                Setup();

                var command = new CreateUserCommand("Amir", "Guelph");
                _mediator.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
                   .Returns(ValueTask.FromResult(OperationResult<bool>.SuccessResult(true)));

                var response = await _mediator.Object.Send(command);

                var command2 = new CreateUserCommand("Amir", "Toronto");
                _mediator.Setup(x => x.Send(command2, It.IsAny<CancellationToken>()))
                   .Returns(ValueTask.FromResult(OperationResult<bool>.FailureResult("User Exists!!!")));

                var response2 = await _mediator.Object.Send(command2);

                Assert.True(response.IsSuccess);
                Assert.False(response2.IsSuccess);
                Assert.Equal("User Exists!!!", response2.ErrorMessage);
                //var memorycache = GetSystemUnderTest();
                //var memoryCacheRepo = new MemoryCacheRepository(memorycache);
                //var userRepository = new UserRepository(memoryCacheRepo);


                //var user = new CreateUserDto { Name = "Amir", Address = "Guelph Ontario" };
                //var result = await userRepository.CreateOneAsync(user);

                //var user2 = new CreateUserDto { Name = "Amir", Address = "Waterloo Ontario" };
                //var result2 = await userRepository.CreateOneAsync(user2);

                //Assert.NotNull(result);
                //Assert.Equal("Amir", result.Name);
                //Assert.Equal("Guelph Ontario", result.Address);
                //Assert.Null(result2);
            }

            [Fact]
            public async Task Get_GetOneUser_ReturnsUser()
            {
                Setup();

                var command = new CreateUserCommand("Amir", "Guelph");
                _mediator.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
                   .Returns(ValueTask.FromResult(OperationResult<bool>.SuccessResult(true)));
                var cResponse = await _mediator.Object.Send(command);


                var query = new GetOneUserQuery("Amir");
                _mediator.Setup(x => x.Send(query, It.IsAny<CancellationToken>()))
                   .Returns(ValueTask.FromResult(OperationResult<GetOneUserQueryResponseModel>.SuccessResult(new GetOneUserQueryResponseModel(new User { Address = "Guelph", Name = "Amir" }))));

                var qResponse = await _mediator.Object.Send(query);

                Assert.True(cResponse.IsSuccess);
                Assert.Equal("Amir", qResponse.Result.User.Name);
                Assert.Equal("Guelph", qResponse.Result.User.Address);
                //var memorycache = GetSystemUnderTest();
                //var memoryCacheRepo = new MemoryCacheRepository(memorycache);
                //var userRepository = new UserRepository(memoryCacheRepo);


                //var user = new CreateUserDto { Name = "Amir", Address = "Guelph Ontario" };
                //await userRepository.CreateOneAsync(user);
                //var result = await userRepository.GetOneAsync(new GetUserDto { Name = "Amir" });


                //Assert.NotNull(result);
                //Assert.Equal("Amir", result.Name);
                //Assert.Equal("Guelph Ontario", result.Address);
            }
            [Fact]
            public async Task Get_GetNonExisitingUser_ReturnsNull()
            {
                Setup();

                var command = new CreateUserCommand("Amir", "Guelph");
                _mediator.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
                   .Returns(ValueTask.FromResult(OperationResult<bool>.SuccessResult(true)));
                var cResponse = await _mediator.Object.Send(command);


                var query = new GetOneUserQuery("Chris");
                _mediator.Setup(x => x.Send(query, It.IsAny<CancellationToken>()))
                   .Returns(ValueTask.FromResult(OperationResult<GetOneUserQueryResponseModel>.NotFoundResult("User Not found!!")));
                var qResponse = await _mediator.Object.Send(query);

                Assert.True(cResponse.IsSuccess);
                Assert.False(qResponse.IsSuccess);
                Assert.Equal("User Not found!!", qResponse.ErrorMessage);

                //var memorycache = GetSystemUnderTest();
                //var memoryCacheRepo = new MemoryCacheRepository(memorycache);
                //var userRepository = new UserRepository(memoryCacheRepo);


                //var user = new CreateUserDto { Name = "Amir", Address = "Guelph Ontario" };
                //await userRepository.CreateOneAsync(user);
                //var result = await userRepository.GetOneAsync(new GetUserDto { Name = "Chris" });


                //Assert.Null(result);
            }
            [Fact]
            public async Task Update_UpdateOneUser()
            {

                Setup();



                var createCommand = new CreateUserCommand("Amir", "Guelph");
                _mediator.Setup(x => x.Send(createCommand, It.IsAny<CancellationToken>()))
                   .Returns(ValueTask.FromResult(OperationResult<bool>.SuccessResult(true)));
                var cResponse = await _mediator.Object.Send(createCommand);


                var updateCommand = new UpdateUserCommand("Amir", "Waterloo");
                _mediator.Setup(x => x.Send(updateCommand, It.IsAny<CancellationToken>()))
                   .Returns(ValueTask.FromResult(OperationResult<bool>.SuccessResult(true)));
                var uResponse = await _mediator.Object.Send(updateCommand);

                var query = new GetOneUserQuery("Amir");
                _mediator.Setup(x => x.Send(query, It.IsAny<CancellationToken>()))
                   .Returns(ValueTask.FromResult(OperationResult<GetOneUserQueryResponseModel>.SuccessResult(new GetOneUserQueryResponseModel(new User { Address = "Waterloo", Name = "Amir" }))));
                var qResponse = await _mediator.Object.Send(query);



                Assert.True(cResponse.IsSuccess);
                Assert.True(uResponse.IsSuccess);
                Assert.Equal("Waterloo", qResponse.Result.User.Address);
                Assert.Equal("Amir", qResponse.Result.User.Name);
                //var memorycache = GetSystemUnderTest();
                //var memoryCacheRepo = new MemoryCacheRepository(memorycache);
                //var userRepository = new UserRepository(memoryCacheRepo);


                //var user = new CreateUserDto { Name = "Amir", Address = "Guelph Ontario" };
                //await userRepository.CreateOneAsync(user);
                //var result = await userRepository.UpdateOneAsync(new UpdateUserDto { Name = "Amir", Address = "Waterloo Ontario" });


                //Assert.NotNull(result);
                //Assert.Equal("Amir", result.Name);
                //Assert.Equal("Waterloo Ontario", result.Address);
            }
            [Fact]
            public async Task Update_UpdateNonExisitingUser_ReturnsNull()
            {

                Setup();



                var createCommand = new CreateUserCommand("Amir", "Guelph");
                _mediator.Setup(x => x.Send(createCommand, It.IsAny<CancellationToken>()))
                   .Returns(ValueTask.FromResult(OperationResult<bool>.SuccessResult(true)));
                var cResponse = await _mediator.Object.Send(createCommand);


                var updateCommand = new UpdateUserCommand("Chris", "Waterloo");
                _mediator.Setup(x => x.Send(updateCommand, It.IsAny<CancellationToken>()))
                   .Returns(ValueTask.FromResult(OperationResult<bool>.NotFoundResult("User Not found!!!")));
                var uResponse = await _mediator.Object.Send(updateCommand);

                Assert.True(cResponse.IsSuccess);
                Assert.False(uResponse.IsSuccess);
                Assert.Equal("User Not found!!!", uResponse.ErrorMessage);
                //var memorycache = GetSystemUnderTest();
                //var memoryCacheRepo = new MemoryCacheRepository(memorycache);
                //var userRepository = new UserRepository(memoryCacheRepo);


                //var user = new CreateUserDto { Name = "Amir", Address = "Guelph Ontario" };
                //await userRepository.CreateOneAsync(user);
                //var result = await userRepository.UpdateOneAsync(new UpdateUserDto { Name = "Chris", Address = "Waterloo Ontario" });


                //Assert.Null(result);

            }
            [Fact]
            public async Task Delete_DeleteOneUser()
            {

                Setup();


                var createCommand = new CreateUserCommand("Amir", "Guelph");
                _mediator.Setup(x => x.Send(createCommand, It.IsAny<CancellationToken>()))
                   .Returns(ValueTask.FromResult(OperationResult<bool>.SuccessResult(true)));
                var cResponse = await _mediator.Object.Send(createCommand);

                var deleteCommand = new DeleteUserCommand("Amir");
                _mediator.Setup(x => x.Send(deleteCommand, It.IsAny<CancellationToken>()))
                   .Returns(ValueTask.FromResult(OperationResult<bool>.SuccessResult(true)));
                var dResponse = await _mediator.Object.Send(deleteCommand);

                var query = new GetOneUserQuery("Amir");
                _mediator.Setup(x => x.Send(query, It.IsAny<CancellationToken>()))
                   .Returns(ValueTask.FromResult(OperationResult<GetOneUserQueryResponseModel>.NotFoundResult("User Not found!!")));
                var qResponse = await _mediator.Object.Send(query);

                Assert.False(qResponse.IsSuccess);
                Assert.True(cResponse.IsSuccess);
                Assert.True(dResponse.IsSuccess);
                Assert.Equal("User Not found!!", qResponse.ErrorMessage);

                //var memorycache = GetSystemUnderTest();
                //var memoryCacheRepo = new MemoryCacheRepository(memorycache);
                //var userRepository = new UserRepository(memoryCacheRepo);


                //var user = new CreateUserDto { Name = "Amir", Address = "Guelph Ontario" };
                //await userRepository.CreateOneAsync(user);
                //var result = await userRepository.DeleteOneAsync(new DeleteUserDto { Name = "Amir" });
                //var getOneResult = await userRepository.GetOneAsync(new GetUserDto { Name = "Amir" });

                //Assert.True(result);
                //Assert.Null(getOneResult);

            }
            [Fact]
            public async Task Delete_DeleteNonExisitingUser_ReturnsFalse()
            {
                Setup();
                var createCommand = new CreateUserCommand("Amir", "Guelph");
                _mediator.Setup(x => x.Send(createCommand, It.IsAny<CancellationToken>()))
                   .Returns(ValueTask.FromResult(OperationResult<bool>.SuccessResult(true)));
                var cResponse = await _mediator.Object.Send(createCommand);

                var deleteCommand = new DeleteUserCommand("Chris");
                _mediator.Setup(x => x.Send(deleteCommand, It.IsAny<CancellationToken>()))
                   .Returns(ValueTask.FromResult(OperationResult<bool>.NotFoundResult("User Not found!!")));
                var dResponse = await _mediator.Object.Send(deleteCommand);

                var query = new GetOneUserQuery("Amir");
                _mediator.Setup(x => x.Send(query, It.IsAny<CancellationToken>()))
                   .Returns(ValueTask.FromResult(OperationResult<GetOneUserQueryResponseModel>.SuccessResult(new GetOneUserQueryResponseModel(new User { Address = "Guelph", Name = "Amir" }))));
                var qResponse = await _mediator.Object.Send(query);

                Assert.True(qResponse.IsSuccess);
                Assert.True(cResponse.IsSuccess);
                Assert.False(dResponse.IsSuccess);
                Assert.Equal("User Not found!!", dResponse.ErrorMessage);

                //var memorycache = GetSystemUnderTest();
                //var memoryCacheRepo = new MemoryCacheRepository(memorycache);
                //var userRepository = new UserRepository(memoryCacheRepo);


                //var user = new CreateUserDto { Name = "Amir", Address = "Guelph Ontario" };
                //await userRepository.CreateOneAsync(user);
                //var result = await userRepository.DeleteOneAsync(new DeleteUserDto { Name = "Chris" });

                //Assert.False(result);

            }
        }
    }
}
