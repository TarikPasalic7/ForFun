
using System.Threading.Tasks;
using ForFun.API.Models;

namespace ForFun.API.Data

{
    public interface IAuthRepository
    {
         Task<User> Register(User user,string password);
         Task<User> Login(string username,string password);
         Task<bool> UserExusts(string name);
    }
}