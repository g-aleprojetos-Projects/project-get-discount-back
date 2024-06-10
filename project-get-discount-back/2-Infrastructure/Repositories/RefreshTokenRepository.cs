using project_get_discount_back._1_Domain.Interfaces;
using project_get_discount_back.Common;
using project_get_discount_back.Context;
using project_get_discount_back.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using project_get_discount_back._1_Domain.Entities;

namespace project_get_discount_back.Repositories
{
    public class RefreshTokenRepository : BaseRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(DataContext context, IUserService userService) : base(context, userService)
        {
        }

        public async Task<RefreshToken?> GetByTokenUser(Guid userId, CancellationToken cancellationToken)
        {
            return await Context.RefreshToken.FirstOrDefaultAsync(x => x.UserId == userId && x.Deleted == false, cancellationToken);
        }

        public async Task<RefreshToken?> GetByToken(string refreshToken, CancellationToken cancellationToken)
        {
            return await Context.RefreshToken.FirstOrDefaultAsync(x => x.Token == refreshToken && x.Deleted == false, cancellationToken);
        }
    }
}
