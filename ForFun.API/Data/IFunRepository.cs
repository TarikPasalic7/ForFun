using System.Threading.Tasks;
using System.Collections.Generic;
using ForFun.API.Models;

namespace ForFun.API.Data
{
    public interface IFunRepository
    {
         void Add<T>(T entity) where T:class;
         void Delete<T>(T entity) where T:class;

         Task <bool> SaveAll();
         Task <IEnumerable<User>> Getusers(); 
         Task<User> GetUser(int id);
    }
}