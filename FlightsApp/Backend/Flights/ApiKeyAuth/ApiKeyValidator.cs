using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.ApiKeyAuth
{
    public class ApiKeyValidator : IApiKeyValidator
    {
        private readonly ApiKeyRepository _apiKeyRepository;


        public ApiKeyValidator(ApiKeyRepository repository)
        {
            _apiKeyRepository = repository;
        }

        public bool IsValid(string apiKey)
        {
            ApiKey key;

            try
            {
                key = _apiKeyRepository.GetApiKey(apiKey);
            }
            catch
            {
                return false;
            }

            if (key != null)
            {
                if (key.ValidUntil == new DateTime(1, 1, 1))
                    return true;
                else if (DateTime.Now > key.ValidUntil)
                    return false;
                else return true;
            }
            else 
                return false;
        }
    }

    public interface IApiKeyValidator
    {
        bool IsValid(string apiKey);
    }
}
