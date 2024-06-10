using MediatR;
using project_get_discount_back._1_Domain.Entities;
using project_get_discount_back.Helpers;
using project_get_discount_back.Interfaces;
using project_get_discount_back.Results;
using project_get_discount_back.ViewModel;

namespace project_get_discount_back.Queries
{
    public record GetTokenQuery(string refreshToken, string device) : IRequest<Result<TokenViewModel>>;

    public class GetTokenQueryHandler : IRequestHandler<GetTokenQuery, Result<TokenViewModel>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly TokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;

        public GetTokenQueryHandler(IUserRepository userRepository, IRefreshTokenRepository refreshToken, IUnitOfWork unitOfWork, TokenService tokenService)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshToken;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<TokenViewModel>> Handle(GetTokenQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.refreshToken))
            {
                return new Fail<TokenViewModel>(ResultError.RefreshTokenEmpty);
            }

            var encryptedPassword = new Cryptography();
            var refreshTokenEncrypted = encryptedPassword.Encrypt(request.refreshToken);

            var user = await _userRepository.GetUserRefreshToken(refreshTokenEncrypted, cancellationToken);

            if (user != null)
            {
                var accessToken = _tokenService.GenerateAccessToken(user);

                if (user.RefreshToken == null || user.RefreshToken?.ExpiresAt <= DateTime.UtcNow)
                {
                    if (user.RefreshToken != null)
                    {
                        _refreshTokenRepository.DeleteLogic(user.RefreshToken);
                        await _unitOfWork.Commit(cancellationToken);
                    }

                    var refreshToken = _tokenService.GenerateRefreshToken();
                    var newRefreshToken = new RefreshToken(user.Id, refreshToken, request.device);

                    _refreshTokenRepository.Create(newRefreshToken);
                    await _unitOfWork.Commit(cancellationToken);


                    user.RefreshToken = newRefreshToken;

                    _userRepository.Update(user);
                    await _unitOfWork.Commit(cancellationToken);

                    var tokenViewModel = new TokenViewModel
                    {
                        Auth = true,
                        AccessToken = accessToken,
                        RefreshToken = refreshToken,
                        Device = request.device,
                        Date = newRefreshToken.CreatedAt
                    };
                    return new Success<TokenViewModel>(tokenViewModel);

                }
                else
                {
                    var tokenViewModel = new TokenViewModel
                    {
                        Auth = true,
                        AccessToken = accessToken,
                        RefreshToken = request.refreshToken,
                        Device = request.device,
                        Date = user.RefreshToken.CreatedAt
                    };
                    return new Success<TokenViewModel>(tokenViewModel);

                }
            }
            else
            {
                var tokenViewModel = new TokenViewModel
                {
                    Auth = false,
                };
                return new Success<TokenViewModel>(tokenViewModel);
            }
        }
    }
}
