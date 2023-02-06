using AgDataCodingAssignment.Application.Contracts;
using AgDataCodingAssignment.Application.Features.User.Events.Delete;
using AgDataCodingAssignment.Application.Models.Common;
using AgDataCodingAssignment.Application.Models.Dtos;
using Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AgDataCodingAssignment.Application.Features.User.Commands.Delete
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, OperationResult<bool>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;

        public DeleteUserCommandHandler(IUserRepository userRepository, IMediator mediator)
        {
            _userRepository = userRepository;
            _mediator = mediator;
        }

        public async ValueTask<OperationResult<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _userRepository.DeleteOneAsync(new DeleteUserDto { Name = request.Name });
            if (result)
            {
                await _mediator.Publish(new UserDeletedEvent(request.Name));
                return OperationResult<bool>.SuccessResult(result);
            }
            else
                return OperationResult<bool>.NotFoundResult("User not found!!!");
        }
    }
}
