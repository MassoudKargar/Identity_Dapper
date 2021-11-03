using Microsoft.IdentityModel.Tokens;

using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Client.Service
{
    public static class Decoder
    {
        /// <summary>
        /// رمز گشایی اطلاعات توکن
        /// </summary>
        /// <param name="token"> توکن </param>
        /// <returns></returns>
        public static List<Claim> Read(string token)
        {
            List<Claim> keys = new List<Claim>();
            var handler = new JwtSecurityTokenHandler();
            var validations = new TokenValidationParameters
            {
                SaveSigninToken = true,
                ValidateTokenReplay = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("629a69ab5c13ce6ba6ad1bbcce4b59640d143e35d5e67a343f553066b0d4f5a2")),
                TokenDecryptionKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("16CharEncryptKey")),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            handler.ValidateToken(token, validations, out SecurityToken tokenSecure);

            var e = tokenSecure.ToString().Replace("\"", "").Replace("\\","").Split("}.{")[1].Split(',');
            foreach (var item in e)
            {
                var k = item.Split(':');
                keys.Add(new Claim(k[0], k[1]));
            }
            
            return keys;
        }
    }
}
