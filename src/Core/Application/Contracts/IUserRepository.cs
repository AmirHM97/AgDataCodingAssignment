using AgDataCodingAssignment.Application.Models.Dtos;
using AgDataCodingAssignment.Domain.Entities;

namespace AgDataCodingAssignment.Application.Contracts
{
    public interface IUserRepository
    {
        Task<UserDocument> CreateOneAsync(CreateUserDto createUserDto);
        Task<bool> DeleteOneAsync(DeleteUserDto deleteUserDto);
        Task<UserDocument> GetOneAsync(GetUserDto getUserDto);
        Task<User> GetOneCachedAsync(GetUserDto getUserDto);
        Task<UserDocument> UpdateOneAsync(UpdateUserDto updateUserDto);
    }
}
