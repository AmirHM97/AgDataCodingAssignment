using AgDataCodingAssignment.Application.Models.Common;
using Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgDataCodingAssignment.Application.Features.User.Commands.Delete
{
    public record DeleteUserCommand(string Name):IRequest<OperationResult<bool>>;
  
}
