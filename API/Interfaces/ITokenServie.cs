

using API.Entities;

namespace API.Interfaces
{
    
    public interface ITokenServie
    {
        Task<string> CreateToken(AppUser user);   
    }
}