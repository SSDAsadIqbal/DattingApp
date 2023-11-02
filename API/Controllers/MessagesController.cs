using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class MessagesController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public MessagesController(IUnitOfWork uow , IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
           

        }
        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            var username = User.GetUsername();

            if (username == createMessageDto.RecipientUserName.ToLower()) return BadRequest("You cant Message yourself");   

            var sender = await _uow.UserRepository.GetUserByUsernameAsync(username);
            var recipient = await _uow.UserRepository.GetUserByUsernameAsync(createMessageDto.RecipientUserName);

            if (recipient == null) return NotFound();

            var message = new Message 
            {
                Sender = sender,
                Recipient = recipient,
                SenderUserName = sender.UserName,
                RecipientUserName = recipient.UserName,
                Content = createMessageDto.Content
            };


            _uow.MessageRepository.AddMessage(message);

            if (await _uow.Complete()) return Ok(_mapper.Map<MessageDto>(message));

            return BadRequest("Failed to send message");
        }


        [HttpGet]
        public async Task<ActionResult<PagedList<MessageDto>>> GetMessagesForUser([FromQuery]MessageParams messageParams)
        {
            messageParams.UserName = User.GetUsername();

            var messages = await _uow.MessageRepository.GetMessagesForUSer(messageParams);

            Response.AddPaginationHeader(new PaginationHeader(messages.CurrentPage, messages.PageSize,messages.TotalCount,messages.TotalPages));

            return messages;
        }




        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var username = User.GetUsername();

            var message = await _uow.MessageRepository.GetMessage(id);

            if(message.SenderUserName != username && message.RecipientUserName != username) return Unauthorized();

            if(message. SenderUserName == username) message.SenderDeleted = true;
            if(message.RecipientUserName == username) message.RecipientDeleted = true;

            if(message.SenderDeleted && message.RecipientDeleted)
            {
                _uow.MessageRepository.DeleteMessage(message);
            }

            if(await _uow.Complete()) return Ok();

            return BadRequest("problem while Deleting message");
        }
    }
}