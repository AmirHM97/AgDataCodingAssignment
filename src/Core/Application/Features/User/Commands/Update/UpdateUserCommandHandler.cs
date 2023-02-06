using AgDataCodingAssignment.Application.Contracts;
using AgDataCodingAssignment.Application.Features.User.Events.Update;
using AgDataCodingAssignment.Application.Models.Common;
using AgDataCodingAssignment.Application.Models.Dtos;
using Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgDataCodingAssignment.Application.Features.User.Commands.Update
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, OperationResult<bool>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;

        public UpdateUserCommandHandler(IUserRepository userRepository, IMediator mediator)
        {
            _userRepository = userRepository;
            _mediator = mediator;
        }

        public async ValueTask<OperationResult<bool>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _userRepository.UpdateOneAsync(new UpdateUserDto { Name = request.Name, Address = request.Address });
            if (result is not null)
            {
                await _mediator.Publish(new UserUpdatedEvent(result.Name, result.Address));
                
                return OperationResult<bool>.SuccessResult(true);
            }
            else
                return OperationResult<bool>.NotFoundResult("User Not found!!!");           
        }
    }
}
