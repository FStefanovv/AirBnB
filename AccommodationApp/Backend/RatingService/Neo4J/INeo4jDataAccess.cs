using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RatingService.Neo4J
{
    public interface INeo4jDataAccess : IAsyncDisposable
    {
        Task<List<string>> ExecuteReadListAsync(string query, string returnObjectKey, IDictionary<string, object>? parameters = null);

        Task<List<Dictionary<string, object>>> ExecuteReadDictionaryAsync(string query, string returnObjectKey, IDictionary<string, object>? parameters = null);

        Task<T> ExecuteReadScalarAsync<T>(string query, IDictionary<string, object>? parameters = null);

        Task<T> ExecuteWriteTransactionAsync<T>(string query, IDictionary<string, object>? parameters = null);
    }
}
