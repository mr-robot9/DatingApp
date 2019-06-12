using System.Threading.Tasks;
using DatingApp.API.Models;

namespace DatingApp.API.Repositories
{
    public interface IAuthRepository
    {
        #region Methods
        
        Task<User> Register(User user, string password);

        Task<User> Login(User user, string password);

        Task<bool> UserExists(string username);
        
        #endregion
        
        
    }
}