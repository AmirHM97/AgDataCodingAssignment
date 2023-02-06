using AgDataCodingAssignment.Application.Contracts;
using AgDataCodingAssignment.Application.Features.User.Commands.Create;
using AgDataCodingAssignment.Application.Features.User.Commands.Delete;
using AgDataCodingAssignment.Application.Features.User.Commands.Update;
using AgDataCodingAssignment.Application.Features.User.Queries;
using AgDataCodingAssignment.Application.Models.Common;
using AgDataCodingAssignment.Application.Models.Dtos;
using AgDataCodingAssignment.Application.ServiceConfiguration;
using AgDataCodingAssignment.Domain.Entities;
using AgDataCodingAssignment.Persistence;
using AgDataCodingAssignment.Persistence.Repositories;
using Mediator;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using Raven.Client.Documents.Commands.MultiGet;
using Raven.Client.ServerWide.Operations.Configuration;
using static Raven.Client.Constants;

namespace AgDataCodingAssignment.Test
{

    public class AgDataCodingAssignmentTest
    {
    
        private Mock<IMediator> _mediator;

        public void Setup()
        {
            _mediator= new Mock<IMediator>();
        }
        

        [Fact]
        public async Task Add_CreateUser_ReturnsTrue()
        {
            

            Setup();

            var command = new CreateUserCommand("Amir","Guelph");
            _mediator.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
               .Returns(ValueTask.FromResult(OperationResult<bool>.SuccessResult(true)));

            var response= await _mediator.Object.Send(command);

            Assert.True(response.IsSuccess);
           
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
               .Returns(ValueTask.FromResult(OperationResult<GetOneUserQueryResponseModel>.SuccessResult(new GetOneUserQueryResponseModel(new User {Address= "Guelph",Name="Amir" }))));

            var qResponse = await _mediator.Object.Send(query);

            Assert.True(cResponse.IsSuccess);
            Assert.Equal("Amir", qResponse.Result.User.Name);
            Assert.Equal("Guelph", qResponse.Result.User.Address);
       
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

            var query = new GetOneUserQuery( "Amir");
            _mediator.Setup(x => x.Send(query, It.IsAny<CancellationToken>()))
               .Returns(ValueTask.FromResult(OperationResult<GetOneUserQueryResponseModel>.NotFoundResult("User Not found!!")));
            var qResponse = await _mediator.Object.Send(query);

            Assert.False(qResponse.IsSuccess);
            Assert.True(cResponse.IsSuccess);
            Assert.True(dResponse.IsSuccess);
            Assert.Equal("User Not found!!", qResponse.ErrorMessage);

        
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
               .Returns(ValueTask.FromResult(OperationResult<GetOneUserQueryResponseModel>.SuccessResult(new GetOneUserQueryResponseModel( new User { Address="Guelph",Name="Amir"}))));
            var qResponse = await _mediator.Object.Send(query);

            Assert.True(qResponse.IsSuccess);
            Assert.True(cResponse.IsSuccess);
            Assert.False(dResponse.IsSuccess);
            Assert.Equal("User Not found!!", dResponse.ErrorMessage);

    
        }
    }
}