using DB;
using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class UserService
{
    private readonly CrudContext _context;
    
    public UserService( CrudContext context ){
        _context = context;
    }

    public List<User> GetAllUsers()
    {
        return _context.Users.ToList();
    }

    public User? GetUserById(int id)
    {
        return _context.Users.Find(id);
    }

    public async Task<User>? GetUserByEmail(string email)
    { 
        
        return await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == email);
        
        return null;
        
    }
    
}