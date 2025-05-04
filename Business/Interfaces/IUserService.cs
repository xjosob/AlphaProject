using Business.Models;
using Domain.Models;

namespace Business.Interfaces
{
    public interface IUserService
    {
        Task<UserResult> GetUsersAsync();
        Task<UserResult> CreateUserAsync(SignUpFormData formData);
    }
}
