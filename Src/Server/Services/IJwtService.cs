namespace Server.Services.Jwt
{
    using Ccms.Entities.Users;
using System.IdentityModel.Tokens.Jwt;

    public interface IJwtService
    {

        /// <summary>
        /// این متود برای ساخت توکن و بازگرداندن توکن به کاربر میباشد
        /// </summary>
        /// <param name="user">اطلاعات کاربر را گرفته و برای ساخت توکن استفاده میکند</param>
        /// <returns> باز میگرداند AccessToken توکن در قالب </returns>
        JwtSecurityToken Generate(User user, Role role);
    }
}
