

using API.Entities;

namespace API.Interfaces
{
    
    public interface ITokenServie
    {
        string CreateToken(AppUser user);   
    }
}