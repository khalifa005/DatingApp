using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext context;
        public DatingRepository(DataContext context)
        {
            this.context = context;

        }
        public void Add<T>(T entites) where T : class
        {
            context.Add(entites);
        }

        public void Delete<T>(T entites) where T : class
        {
            context.Remove(entites);
        }

        public async Task<User> GetUser(int id)
        {
           var user=await context.Users.Include(u => u.Photos).FirstOrDefaultAsync(u => u.Id==id);
           return user ;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users=await context.Users.Include(u => u.Photos).ToListAsync();
            return users;

        }

        public async Task<bool> SaveAll()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}