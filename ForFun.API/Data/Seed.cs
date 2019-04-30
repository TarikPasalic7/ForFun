using System.Collections.Generic;
using ForFun.API.Models;
using Newtonsoft.Json;

namespace ForFun.API.Data
{
    public class Seed
    {
        private readonly DataContext _context;
        public Seed(DataContext context)
        {

            _context=context;
        }

        public void Seedusers(){

         var userdata=System.IO.File.ReadAllText("Data/UserSeed.json");
         var users=JsonConvert.DeserializeObject<List<User>>(userdata);

         foreach (var user in users)
         {
             byte[] passwordHash,passwordSalt;
             CreatePasswordHash("password",out passwordHash,out passwordSalt);
             user.PaswordHash=passwordHash;
             user.PasswordSalt=passwordSalt;
             user.Username=user.Username.ToLower();
             _context.Users.Add(user);
         }
          _context.SaveChanges();
        }
          private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hash=new System.Security.Cryptography.HMACSHA512()){
                      
                      passwordSalt=hash.Key;
                      passwordHash=hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                  

            }
        }
    }
}