using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgDataCodingAssignment.Application.Contracts
{
    public interface IMemoryCacheRepository
    {
        Task<T> AddAsync<T>(string key, T value) where T : class;
        Task DeleteAsync(string key);
        Task<T?> GetAsync<T>(string key, CancellationToken token = default) where T : class;
    }
}
