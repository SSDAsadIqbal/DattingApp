using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface ImessageRepository
    {
        void AddMessage(Message message);        
        void DeleteMessage(Message message);        
        Task<Message> GetMessage(int id);
        Task<PagedList<MessageDto>> GetMessagesForUSer(MessageParams messageParams);
        Task<IEnumerable<MessageDto>> GetMessageThread(string currentUserName, string RecipientUserName);
        void AddGroup(Group group);
        void RemoveConnection(Connection connection);
        Task<Connection> GetConnection(string connectionId);
        Task<Group> GetMessageGroup(string groupName);
        Task<Group> GetGroupForconnection(string connectionId);

    }
}