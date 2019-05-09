using System;
using System.Threading.Tasks;
using ForFun.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ForFun.API.Data
{
    public class AuthRepository : IAuthRepository
    {

        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            
            _context=context;
        }
        public async Task<User> Login(string username, string password)
        {

            var user= await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(x=>x.Username==username);
            if(user==null)
            return null;
            if(!VerifyPassword(password,user.PasswordSalt,user.PaswordHash))
            return null;
            return user;
            
        }

        private bool VerifyPassword(string password, byte[] passwordSalt, byte[] paswordHash)
        {
                 using(var hash=new System.Security.Cryptography.HMACSHA512(passwordSalt)){
                      
                
                     var computedhash=hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                  for(int i=0;i<computedhash.Length;i++)
                  {
                          if(computedhash[i]!=paswordHash[i])
                          return false;
                  }
                 return true;
            }
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash,passwordSalt;
            CreatePasswordHash(password,out passwordHash,out passwordSalt);// out je znak za referencu(stvarnu vrijednost)
            user.PasswordSalt=passwordSalt;
            user.PaswordHash=passwordHash;
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hash=new System.Security.Cryptography.HMACSHA512()){
                      
                      passwordSalt=hash.Key;
                      passwordHash=hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                  

            }
        }

        public async Task<bool> UserExusts(string name)
        {
            if(await _context.Users.AnyAsync(x=>x.Username==name))
            return true;

            return false;
        }
    }
}