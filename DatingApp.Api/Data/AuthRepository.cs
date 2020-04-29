using System;
using System.Threading.Tasks;
using DatingApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Api.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<User> Login(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x=>x.Username ==username);
            if(user == null)
            {
                return null;
            } 
            if(!VerifyPasswordHash(password,user.PasswordHash, user.PasswordSalt))
            return null;
            //throw new System.NotImplementedException();
            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmca = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmca.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i=0; i < computedHash.Length; i++)
                {
                    if(computedHash[i]!=passwordHash[i])
                    return false;
                }
                return true;
            }
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash , passworrdSalt;
            CreatePasswordHash(password,out passwordHash,out passworrdSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt=passworrdSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync(); 

            return user;
            //throw new System.NotImplementedException();
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passworrdSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passworrdSalt=hmac.Key;
                passwordHash =hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            }
            //throw new NotImplementedException();
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _context.Users.AnyAsync(x =>x.Username == username ))
            return true;

            //throw new System.NotImplementedException();
            return false;
        }
    }
}