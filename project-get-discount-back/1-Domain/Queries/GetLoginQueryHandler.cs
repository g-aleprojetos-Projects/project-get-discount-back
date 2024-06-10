using MediatR;
using project_get_discount_back._1_Domain.Entities;
using project_get_discount_back.Helpers;
using project_get_discount_back.Interfaces;
using project_get_discount_back.Results;
using project_get_discount_back.ViewModel;

namespace project_get_discount_back.Queries
{
    public record GetLoginQuery(string email, string password, string device) : IRequest<Result<LoginViewModel>>;

    public class GetLoginQueryHandler : IRequestHandler<GetLoginQuery, Result<LoginViewModel>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly TokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;

        public GetLoginQueryHandler(IUserRepository userRepository, IRefreshTokenRepository refreshToken, IUnitOfWork unitOfWork, TokenService tokenService)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshToken;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<LoginViewModel>> Handle(GetLoginQuery request, CancellationToken cancellationToken)
        {
            var validationResult = ValidateRequest(request);
            if (validationResult != null)
            {
                return validationResult;
            }

            var user = await _userRepository.GetByEmailPassword(request.email, cancellationToken);

            if (user == null)
            {
                return new Fail<LoginViewModel>(ResultError.NonExistentEmail);
            }

            if (user.Deleted)
            {
                return new Fail<LoginViewModel>(ResultError.EmailDeleted);
            }

            if (user.Password?.PasswordHash == null)
            {
                return new Fail<LoginViewModel>(ResultError.NonExistentPassword);
            }

            var encryptedPassword = new Cryptography();
            if (user.Password.PasswordHash == encryptedPassword.Encrypt(request.password))
            {
                var accessToken = _tokenService.GenerateAccessToken(user);
                var refreshToken = _tokenService.GenerateRefreshToken();

                var tokenUser = await _refreshTokenRepository.GetByTokenUser(user.Id, cancellationToken);

                if (tokenUser != null)
                {
                    _refreshTokenRepository.DeleteLogic(tokenUser);
                }

                var newRefreshToken = new RefreshToken(user.Id, refreshToken, request.device);

                _refreshTokenRepository.Create(newRefreshToken);
                await _unitOfWork.Commit(cancellationToken);

                user.RefreshToken = newRefreshToken;

                _userRepository.Update(user);
                await _unitOfWork.Commit(cancellationToken);


                var loginViewModel = new LoginViewModel
                {
                    Auth = true,
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    Device = request.device,
                    Date = newRefreshToken.CreatedAt
                };
                return new Success<LoginViewModel>(loginViewModel);
            }

            else
            {
                return new Fail<LoginViewModel>(ResultError.WrongEmailOrPassword);
            }

        }

        private static Fail<LoginViewModel>? ValidateRequest(GetLoginQuery request)
        {
            if (string.IsNullOrEmpty(request.email))
            {
                return new Fail<LoginViewModel>(ResultError.LoginEmailEmpty);
            }

            if (string.IsNullOrEmpty(request.password))
            {
                return new Fail<LoginViewModel>(ResultError.LoginPasswordEmpty);
            }

            if (string.IsNullOrEmpty(request.device))
            {
                return new Fail<LoginViewModel>(ResultError.DeviceEmpty);
            }

            return null;
        }
    }
}
