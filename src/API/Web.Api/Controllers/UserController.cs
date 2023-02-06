using AgDataCodingAssignment.Application.Contracts;
using AgDataCodingAssignment.Application.Features.User.Commands.Create;
using AgDataCodingAssignment.Application.Features.User.Commands.Delete;
using AgDataCodingAssignment.Application.Features.User.Commands.Update;
using AgDataCodingAssignment.Application.Features.User.Queries;
using AgDataCodingAssignment.Application.Models.Dtos;
using AgDataCodingAssignment.Persistence.Repositories;
using AgDataCodingAssignment.Web.Api.ApiModels.User;
using AgDataCodingAssignment.WebFramework.BaseController;
using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace AgDataCodingAssignment.Web.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetOneAsync(string Name)
        {
            var command = await _mediator.Send(new GetOneUserQuery(Name));
           
            return OperationResult(command);
        }
        [HttpPost]
        public async Task<IActionResult> AddOneAsync(CreateUserViewModel createUserViewModel)
        {
            var commaand = await _mediator.Send(new CreateUserCommand(createUserViewModel.Name, createUserViewModel.Address));
            return OperationResult(commaand);
            //var result = await _userRepository.CreateOneAsync(new CreateUserDto { Name = createUserViewModel.Name, Address = createUserViewModel.Address });
            //if (result is not null)
            //    return Ok(result);
            //else return BadRequest("User already exists!!!");
        }
        [HttpPut]
        public async Task<IActionResult> UpdateOneAsync(UpdateUserViewModel updateUserViewModel)
        {
            var commaand = await _mediator.Send(new UpdateUserCommand(updateUserViewModel.Name, updateUserViewModel.Address));
            return OperationResult(commaand);
            //var result = await _userRepository.UpdateOneAsync(new UpdateUserDto { Name = updateUserViewModel.Name, Address = updateUserViewModel.Address });
            //if (result is not null)
            //    return Ok(result);
            //else return BadRequest("User does not exist!!!");
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteOneAsync(string Name)
        {
            var commaand = await _mediator.Send(new DeleteUserCommand(Name));
            return OperationResult(commaand);
            //var result = await _userRepository.DeleteOneAsync(new DeleteUserDto { Name = Name });
            //if (result)
            //    return Ok();
            //else return BadRequest("User does not exist!!!");
        }
    }
}
