using AgDataCodingAssignment.Application.Models.Common;
using Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgDataCodingAssignment.Application.Features.User.Queries
{
    public record GetOneUserQuery(string Name): IRequest<OperationResult<GetOneUserQueryResponseModel>>;
   
}
