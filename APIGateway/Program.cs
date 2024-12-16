using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace APIGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddReverseProxy()
                .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "http://localhost:5001";
                    options.Audience = "AccountService";
                    options.RequireHttpsMetadata = false;
                });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            app.MapReverseProxy();
            app.UseAuthentication();
            app.UseAuthorization();
            app.Run();
        }
    }
}
