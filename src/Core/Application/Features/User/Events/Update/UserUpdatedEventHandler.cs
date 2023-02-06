using AgDataCodingAssignment.Application.Contracts;
using AgDataCodingAssignment.Domain.Entities;
using Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgDataCodingAssignment.Application.Features.User.Events.Update
{
    public class UserUpdatedEventHandler:INotificationHandler<UserUpdatedEvent>
    {
        private readonly IMemoryCacheRepository _memoryCacheRepository;

        public UserUpdatedEventHandler(IMemoryCacheRepository memoryCacheRepository)
        {
            _memoryCacheRepository = memoryCacheRepository;
        }

        public async ValueTask Handle(UserUpdatedEvent notification, CancellationToken cancellationToken)
        {
            var result = await _memoryCacheRepository.AddAsync(notification.Name, new AgDataCodingAssignment.Domain.Entities.User { Address = notification.Address, Name = notification.Name });
            if (result is null)
            {
                throw new Exception("Adding user to cache operation failed!!");
            }
        }
    }
}
