using BackPack.Dependency.Library.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace BackPack.Dependency.Library.JWT
{
    public static class Startup
    {
        public static IServiceCollection AddJWT(this IServiceCollection services, IConfiguration _configuration)
        {
            Aes256Helper aes256Helper = new(_configuration);

            string? _issuer = aes256Helper.Aes256Decryption(_configuration.GetSection("JWTSettings").GetSection("Issuer").Value!);
            string? _audience = aes256Helper.Aes256Decryption(_configuration.GetSection("JWTSettings").GetSection("Audience").Value!);
            string? _key = aes256Helper.Aes256Decryption(_configuration.GetSection("JWTSettings").GetSection("Key").Value!);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = _issuer,
                        ValidAudience = _audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(_key!)),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            return services;
        }
    }
}
