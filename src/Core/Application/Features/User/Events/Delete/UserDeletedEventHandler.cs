using AgDataCodingAssignment.Application.Contracts;
using Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgDataCodingAssignment.Application.Features.User.Events.Delete
{
    internal class UserDeletedEventHandler:INotificationHandler<UserDeletedEvent>
    {
        private readonly IMemoryCacheRepository _memoryCacheRepository;

        public UserDeletedEventHandler(IMemoryCacheRepository memoryCacheRepository)
        {
            _memoryCacheRepository = memoryCacheRepository;
        }

        public async ValueTask Handle(UserDeletedEvent notification, CancellationToken cancellationToken)
        {
             await _memoryCacheRepository.DeleteAsync(notification.Name);
        }
    }
}
