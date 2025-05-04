using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Repositories;
using Domain.Extensions;
using Domain.Models;

namespace Business.Services
{
    public class ClientService(IClientRepository clientRepository) : IClientService
    {
        private readonly IClientRepository _clientRepository = clientRepository;

        public async Task<ClientResult> CreateClientAsync(ClientFormData formData)
        {
            if (formData == null)
            {
                return new ClientResult
                {
                    Succeeded = false,
                    StatusCode = 400,
                    Error = "Form data is null",
                };
            }

            var clientEntity = formData.MapTo<ClientEntity>();

            var result = await _clientRepository.AddAsync(clientEntity);
            if (!result.Succeeded)
            {
                return new ClientResult
                {
                    Succeeded = false,
                    StatusCode = 500,
                    Error = result.Error,
                };
            }

            return result.MapTo<ClientResult>();
        }

        public async Task<ClientResult> GetClientsAsync()
        {
            var result = await _clientRepository.GetAllAsync();
            return result.MapTo<ClientResult>();
        }
    }
}
