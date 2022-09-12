using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly NZWalksDbContext _context;

        public UserRepository(NZWalksDbContext context)
        {
            _context = context;
        }
        public async Task<User> AuthentificateAsync(string username, string password)
        {
          var user  = await _context.Users.FirstOrDefaultAsync(x=> x.Username.ToLower() == username.ToLower() && x.Password.ToLower() == password.ToLower());

            if (user == null)
            {
                return null;
            }

            var userRoles = await _context.User_Role.Where(x => x.UserId == user.Id).ToListAsync();

            if (userRoles.Any())
            {
                user.Roles = new List<string>();
                foreach (var userRole in userRoles)
                {
                    var role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == userRole.RoleId);

                    if (role !=null)
                    {
                        user.Roles.Add(role.Name);
                    }
                }
            }

            user.Password = null;
            return user;
        }
    }
}
