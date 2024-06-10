using project_get_discount_back._1_Domain.Entities;

namespace project_get_discount_back.Interfaces
{
    public interface IRefreshTokenRepository : IBaseRepository<RefreshToken>
    {
        Task<RefreshToken?> GetByTokenUser(Guid userId, CancellationToken cancellationToken);
        Task<RefreshToken?> GetByToken(string refreshToken, CancellationToken cancellationToken);
    }
}
