using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class MessageRepository : ImessageRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public MessageRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void AddGroup(Group group)
        {
            _context.Add(group);
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

        public async Task<Connection> GetConnection(string connectionId)
        {
            return await _context.Connections.FindAsync(connectionId);
        }

        public async Task<Group> GetGroupForconnection(string connectionId)
        {   
            return await _context.Groups
                    .Include(x => x.Connections)
                    .Where(x => x.Connections.Any(c => c.ConnectionId == connectionId))
                    .FirstOrDefaultAsync(); 
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FindAsync(id);
        }

        public async Task<Group> GetMessageGroup(string groupName)
        {
           return await _context.Groups
                    .Include(x => x.Connections)
                    .FirstOrDefaultAsync(x => x.Name == groupName);
        }

        public async Task<PagedList<MessageDto>> GetMessagesForUSer(MessageParams messageParams)
        {
            var query = _context.Messages
                 .OrderByDescending(m => m.MessagesSent)
                 .AsQueryable();

            query = messageParams.Container switch
            {
                "inbox" => query.Where(m => m.RecipientUserName == messageParams.UserName && m.RecipientDeleted == false),
                "outbox" => query.Where(m => m.SenderUserName == messageParams.UserName && m.SenderDeleted == false),
                _ => query.Where(m => m.RecipientUserName == messageParams.UserName && m.DateRead == null && m.RecipientDeleted == false)
            };

            var messages = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);

            return await PagedList<MessageDto>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }


        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUserName, string RecipientUserName)
        {
            var query = _context.Messages
                     .Where(
                         m => m.RecipientUserName == currentUserName && m.RecipientDeleted == false && 
                         m.SenderUserName == RecipientUserName ||
                         m.RecipientUserName == RecipientUserName &&m.SenderDeleted == false &&
                         m.SenderUserName == currentUserName
                     ).OrderBy(m => m.MessagesSent)
                     .AsQueryable();

            var unreadMessages = query.Where(m => m.DateRead == null && m.RecipientUserName == currentUserName).ToList();

            if (unreadMessages.Any())
            {
                foreach (var message in unreadMessages)
                {
                    message.DateRead = DateTime.UtcNow;
                }

                
            }

            return await query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public void RemoveConnection(Connection connection)
        {
            _context.Connections.Remove(connection);
        }

    }
}