using AgDataCodingAssignment.Application.Contracts;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AgDataCodingAssignment.Persistence.Repositories
{


    public class MemoryCacheRepository : IMemoryCacheRepository
    {
        private readonly IMemoryCache _memoryCache;
        private SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1);

        public MemoryCacheRepository(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public async Task<T> AddAsync<T>(string key, T value) where T : class
        {

            if (value is not null)
            {
                await semaphoreSlim.WaitAsync();
                try
                {
                    var result = _memoryCache.Set(key, value);
                    return (T)result;
                }
                finally
                {
                    semaphoreSlim.Release();
                }

            }
            else
                return null;
        }
        public async Task<T?> GetAsync<T>(string key, CancellationToken token = default) where T : class
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                var result = _memoryCache.Get(key);
                //return string.IsNullOrEmpty(result?.ToString()) ? null : JsonSerializer.Deserialize<T>(result);
                return (T?)result;

            }
            finally
            {
                semaphoreSlim.Release();
            }
        }
        public async Task DeleteAsync(string key)
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                _memoryCache.Remove(key);
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }
    }
}
