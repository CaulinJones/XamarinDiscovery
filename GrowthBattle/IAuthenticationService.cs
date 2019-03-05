using System.Threading.Tasks;
using IdentityModel.OidcClient;

namespace GrowthBattler {
    public interface IAuthenticationService
    {
        Task<LoginResult> Authenticate();
    }

}