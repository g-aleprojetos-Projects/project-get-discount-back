using project_get_discount_back.Common;
using project_get_discount_back.Helpers;

namespace project_get_discount_back._1_Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public string Device { get; set; }
        public DateTimeOffset ExpiresAt { get; set; }

        public RefreshToken(Guid userId, string token, string device)
        {
            UserId = userId;
            Token = Encrypting(token);
            ExpiresAt = DateTimeOffset.UtcNow.AddDays(7);
            Device = device;
        }

        private static string Encrypting(string valor)
        {
            var encryptedRefreshToken = new Cryptography();
            return encryptedRefreshToken.Encrypt(valor);
        }
    }
}
