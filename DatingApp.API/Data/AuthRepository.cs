using System;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext context;
        public AuthRepository(DataContext context)
        {
            this.context = context;

        }
        public async Task<User> Login(string username, string password)
        {
            var user=await context.Users.FirstOrDefaultAsync(x =>x.Username==username);
            if(user==null){
                return null;
            }

            if(!VerifyPasswordHash(password,user.PasswordSalt,user.PasswordHash))
            return null;
            
            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordSalt, byte[] passwordHash)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
               
                //because couputehash take byte []
               var computedHash= hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
               for(int i=0;i>computedHash.Length;i++){
                   if(computedHash[i]!=passwordHash[i]) 
                   return false;

               }

            }
            return true;
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] HashPassword, SaltPassword;
            CreatePasswordHash(password, out HashPassword, out SaltPassword);
            user.PasswordSalt = SaltPassword;
            user.PasswordHash = HashPassword;
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return user;
        }

        private void CreatePasswordHash(string password, out byte[] hashPassword, out byte[] saltPassword)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                saltPassword = hmac.Key;
                //because couputehash take byte []
                hashPassword = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            }
        }

        public async Task<bool> UserExist(string username)
        {
            if(await context.Users.AnyAsync(x =>x.Username==username))
            return true;
           
           return false;
        }
    }
}