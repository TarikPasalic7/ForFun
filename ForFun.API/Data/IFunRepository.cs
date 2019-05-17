using System.Threading.Tasks;
using System.Collections.Generic;
using ForFun.API.Models;
using ForFun.API.Helpers;

namespace ForFun.API.Data
{
    public interface IFunRepository
    {
         void Add<T>(T entity) where T:class;
         void Delete<T>(T entity) where T:class;

         Task <bool> SaveAll();
         Task <PagedList<User>> Getusers(UserParams userParams); 
         Task<User> GetUser(int id);

         Task<Photo> GetPhoto(int id);
          Task<Photo> GetMainPhotoUser(int Userid);
    }
}