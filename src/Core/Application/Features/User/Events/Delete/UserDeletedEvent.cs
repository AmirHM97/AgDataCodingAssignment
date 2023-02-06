using Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgDataCodingAssignment.Application.Features.User.Events.Delete
{
    public record UserDeletedEvent(string Name):INotification;
   
}
