using AgDataCodingAssignment.Application.Contracts;
using AgDataCodingAssignment.Application.Models.Common;
using AgDataCodingAssignment.Application.Models.Dtos;
using Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgDataCodingAssignment.Application.Features.User.Queries
{
    public class GetOneUserQueryHandler : IRequestHandler<GetOneUserQuery, OperationResult<GetOneUserQueryResponseModel>>
    {
        private readonly IUserRepository _userRepository;

        public GetOneUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async ValueTask<OperationResult<GetOneUserQueryResponseModel>> Handle(GetOneUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetOneCachedAsync(new GetUserDto { Name = request.Name });
            if(user == null)
            {
                return OperationResult<GetOneUserQueryResponseModel>.NotFoundResult("User Not found!!");
            }

            return OperationResult<GetOneUserQueryResponseModel>.SuccessResult(new GetOneUserQueryResponseModel(user));
        }
    }
}
