using project_get_discount_back._1_Domain.Entities;
using project_get_discount_back.Entities;

namespace project_get_discount_back.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> GetByEmail(string email, CancellationToken cancellationToken);
        Task<User?> GetByEmailPassword(string email, CancellationToken cancellationToken);
        Task<User?> GetUserRefreshToken(string refreshToken, CancellationToken cancellationToken);
    }
}
