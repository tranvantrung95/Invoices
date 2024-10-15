using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace WebAPI
{
    public class TokenService : IDisposable
    {
        private readonly RSA _rsa;
        private readonly RsaSecurityKey _rsaSecurityKey;

        public TokenService()
        {
            // Tạo RSA với độ dài khóa 2048 bit và giữ nó trong suốt vòng đời của ứng dụng
            _rsa = RSA.Create(2048);
            _rsaSecurityKey = new RsaSecurityKey(_rsa);
        }

        // Trả về RsaSecurityKey để dùng trong cấu hình JWT
        public RsaSecurityKey GetRsaSecurityKey()
        {
            return _rsaSecurityKey;
        }

        // Giải phóng tài nguyên khi không sử dụng nữa
        public void Dispose()
        {
            _rsa?.Dispose();
        }
    }

}
