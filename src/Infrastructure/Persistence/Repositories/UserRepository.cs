using AgDataCodingAssignment.Application.Contracts;
using AgDataCodingAssignment.Application.Models.ApiResult;
using AgDataCodingAssignment.Application.Models.Dtos;
using AgDataCodingAssignment.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AgDataCodingAssignment.Persistence.Repositories
{


    public class UserRepository : IUserRepository
    {
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly IRavenDbRepository _ravenDbRepository;

        public UserRepository(IMemoryCacheRepository memoryCacheRepository, IRavenDbRepository ravenDbRepository)
        {
            _memoryCacheRepository = memoryCacheRepository;
            _ravenDbRepository = ravenDbRepository;
        }

        public async Task<UserDocument> CreateOneAsync(CreateUserDto createUserDto)
        {

            var exists = await GetOneAsync(new GetUserDto { Name = createUserDto.Name }) is null;
            if (exists)
            {
                var result = await _ravenDbRepository.AddAsync(new UserDocument { Name = createUserDto.Name, Address = createUserDto.Address });
                return result;
            }
            else
                return null;
        }
        public async Task<UserDocument> GetOneAsync(GetUserDto getUserDto)
        {
            var user = await _ravenDbRepository.GetByNameAsync(getUserDto.Name);
            return user;
        }
        public async Task<User> GetOneCachedAsync(GetUserDto getUserDto)
        {
            var user = await _memoryCacheRepository.GetAsync<User>(getUserDto.Name);
            return user;
        }
        public async Task<UserDocument> UpdateOneAsync(UpdateUserDto updateUserDto)
        {

            var result = await _ravenDbRepository.UpdateAsync(updateUserDto);
            return result;

        }
        public async Task<bool> DeleteOneAsync(DeleteUserDto deleteUserDto)
        {
            if (await GetOneAsync(new GetUserDto { Name = deleteUserDto.Name }) is null)
            {
                return false;
            }
            else
            { await _ravenDbRepository.Delete(deleteUserDto.Name); return true; }
        }
    }
}
