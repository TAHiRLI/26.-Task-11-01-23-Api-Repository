using Store.Core.Entities;

namespace StoreApi.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateJwtToken(AppUser user,  IList<string> roles, IConfiguration config );
    }
}
