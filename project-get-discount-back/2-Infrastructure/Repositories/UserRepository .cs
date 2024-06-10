using Microsoft.EntityFrameworkCore;
using project_get_discount_back._1_Domain.Interfaces;
using project_get_discount_back.Context;
using project_get_discount_back.Entities;
using project_get_discount_back.Interfaces;

namespace project_get_discount_back.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(DataContext context, IUserService userService) : base(context, userService)
        {
        }

        public async Task<User?> GetByEmail(string email, CancellationToken cancellationToken)
        {
            return await Context.Users.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
        }

        public async Task<User?> GetByEmailPassword(string email, CancellationToken cancellationToken)
        {
            return await Context.Users
                    .Include(u => u.Password)
                    .FirstOrDefaultAsync(x => x.Email == email && !x.Deleted && x.Password != null && !x.Password.Deleted, cancellationToken);

        }

        public async Task<User?> GetUserRefreshToken(string refreshToken, CancellationToken cancellationToken)
        {
            return await Context.Users
                   .Include(u => u.RefreshToken)
                   .FirstOrDefaultAsync(x => x.RefreshToken != null && x.RefreshToken.Token == refreshToken && !x.Deleted && !x.RefreshToken.Deleted, cancellationToken);
        }
    }
}
