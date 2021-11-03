namespace Server.Services.Jwt
{
    using Ccms.Entities.Users;

    using Microsoft.IdentityModel.Tokens;

    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;

    public class JwtService : IJwtService
    {
        /// <summary>
        /// این متود برای ساخت توکن و بازگرداندن توکن به کاربر میباشد
        /// </summary>
        /// <param name="user">اطلاعات کاربر را گرفته و برای ساخت توکن استفاده میکند</param>
        /// <returns> باز میگرداند AccessToken توکن در قالب </returns>
        public JwtSecurityToken Generate(User user, Role role)
        {
            var secretKey = Encoding.UTF8.GetBytes("629a69ab5c13ce6ba6ad1bbcce4b59640d143e35d5e67a343f553066b0d4f5a2"); // longer that 16 character
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);

            var encryptionkey = Encoding.UTF8.GetBytes("16CharEncryptKey"); //must be 16 character
            var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionkey), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);

            //var claims = await GetClaimsAsync(user);

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = "ISQI",
                Audience = "ISQI",
                IssuedAt = DateTime.Now,
                NotBefore = DateTime.Now.AddMinutes(0),
                Expires = DateTime.Now.AddMinutes(20),
                SigningCredentials = signingCredentials,
                EncryptingCredentials = encryptingCredentials,
                Subject = new ClaimsIdentity(GetClaims(user, role))
            };

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

            var tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken securityToken = tokenHandler.CreateJwtSecurityToken(descriptor);
            string encryptedJwt = tokenHandler.WriteToken(securityToken);
            return securityToken;
        }

        /// <summary>
        /// ثبت دسترسی های کاربر در توکن
        /// </summary>
        /// <param name="user">دریافت اطلاعات کاربر</param>
        /// <param name="role">دریافت اطلاعات دسترسی کاربر</param>
        /// <returns>Task<IEnumerable<Claim>></returns>
        private static IEnumerable<Claim> GetClaims(User user, Role role)
        {
            //add custom claims
            List<Claim> list = new List<Claim>()
            {
                new Claim("FullName", user.FullName),
                new Claim("RoleName", role.RoleName),
                new Claim("RoleCaption", role.RoleCaption),
            };
            return list;
        }
    }
}
