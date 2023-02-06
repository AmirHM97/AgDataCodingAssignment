using AgDataCodingAssignment.Application.Contracts;
using AgDataCodingAssignment.Domain.Entities;
using Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgDataCodingAssignment.Application.Features.User.Events.Create
{
    public class UserCreatedEventHandler : INotificationHandler<UserCreatedEvent>
    {
        private readonly IMemoryCacheRepository _memoryCacheRepository;

        public UserCreatedEventHandler(IMemoryCacheRepository memoryCacheRepository)
        {
            _memoryCacheRepository = memoryCacheRepository;
        }

        public async ValueTask Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
        {
            var result= await _memoryCacheRepository.AddAsync(notification.Name, new AgDataCodingAssignment.Domain.Entities.User { Name = notification.Name, Address = notification.Address });
            if (result is null)
            {
                throw new Exception("Adding user to cache operation failed!!");
            }
        }
    }
}
