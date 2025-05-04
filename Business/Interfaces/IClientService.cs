using Business.Models;
using Domain.Models;

namespace Business.Interfaces
{
    public interface IClientService
    {
        Task<ClientResult> CreateClientAsync(ClientFormData formData);
        Task<ClientResult> GetClientsAsync();
    }
}
