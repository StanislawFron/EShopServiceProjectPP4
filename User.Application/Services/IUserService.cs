using User.Domain.Models.Response;
using User.Domain.Models.Requests;

namespace User.Application.Services
{
    public interface IUserService
    {
        UserResponseDTO GetUser(int Id);

        Task<string> RegisterAsync(RegisterRequest request);

    }
}
