namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }        
        ImessageRepository MessageRepository { get; }
        IlikesRepository LikesRepository { get;}
        Task<bool> Complete();
        bool hasChanges();
    }
}