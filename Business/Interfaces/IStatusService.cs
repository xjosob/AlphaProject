using Business.Models;
using Domain.Models;

namespace Business.Interfaces
{
    public interface IStatusService
    {
        Task<StatusResult<IEnumerable<Status>>> GetStatusAsync();
        Task<StatusResult<Status>> GetStatusByNameAsync(string statusName);
        Task<StatusResult<Status>> GetStatusByIdAsync(int id);
    }
}
