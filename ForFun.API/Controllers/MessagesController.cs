using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using ForFun.API.Data;
using ForFun.API.Helpers;
using ForFun.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ForFun.API.Dtos;

using System;


namespace ForFun.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/users/{userid}/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {

        private readonly IFunRepository _repo;
        private readonly IMapper _mapper;
        public MessagesController(IFunRepository repo,IMapper mapper)
        {
            _repo=repo;
            _mapper=mapper;
        }
        [HttpGet("{id}",Name="GetMessage")]

        public async Task<IActionResult> GetMessage(int userid, int id) {
               
              if(userid !=int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            return  Unauthorized();
           
           var messagerepo=await _repo.GetMessage(id);
           if(messagerepo ==null)
             return NotFound();

             return Ok(messagerepo);
            
        }

        [HttpGet] 

        public async Task<IActionResult> GetMessagesForUser(int userId,[FromQuery]MessageParams messageparams) {
      if(userId !=int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            return  Unauthorized();
             messageparams.UserId=userId;
           var messagesfromrepo= await _repo.GetMessagesforUser(messageparams);

           var messages=_mapper.Map<IEnumerable<MessageToReturnDto>>(messagesfromrepo);
        Response.AddPagination(messagesfromrepo.currentPage,messagesfromrepo.sizePage,messagesfromrepo.totalCount,messagesfromrepo.totalPages);
        return Ok(messages);

        }
        [HttpGet("thread/{recipientId}")] 

        public async Task<IActionResult> GetMessageThread(int userId,int recipientId) {

             if(userId !=int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            return  Unauthorized();
            var messagerepo=await _repo.GetMessageThread(userId,recipientId);

            var messagethread= _mapper.Map<IEnumerable<MessageToReturnDto>>(messagerepo);
            return Ok(messagethread);
        }

        [HttpPost]

        public async Task<IActionResult> CreateMessage(int userid, MessageForCreationDto messageForcreation) {
            var sender= await _repo.GetUser(userid);
            if(sender.Id !=int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            return  Unauthorized();
             messageForcreation.SenderId=userid;

             var recipient= await _repo.GetUser(messageForcreation.RecipientId);

             if(recipient== null)
             return BadRequest("Could not find user");
             var message= _mapper.Map<Message>(messageForcreation);
             _repo.Add(message);
        

             if(await _repo.SaveAll())
             {
                  var messagetoreturn=_mapper.Map<MessageToReturnDto>(message);
       return CreatedAtRoute("GetMessage",new {id=message.Id},messagetoreturn );
             }
             

             throw new Exception("Could not create a message");
    }

    [HttpPost("{id}")]
    public async Task<IActionResult> MessageDelete(int id, int userid) {

              if(userid !=int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            return  Unauthorized();

   var messagefromrepo= await _repo.GetMessage(id);
      if(messagefromrepo.SenderId==userid)
        messagefromrepo.SenderDeleted=true;

        if(messagefromrepo.RecipientId==userid)
        messagefromrepo.RecipientDeleted=true;
        
        if(messagefromrepo.SenderDeleted && messagefromrepo.RecipientDeleted)
          _repo.Delete(messagefromrepo);
      
      if(await _repo.SaveAll())
      return NoContent();
      
      throw new Exception("error deleting messages");

    }

     [HttpPost("{id}/read")]
      
      public async Task<IActionResult> MarMessageAsRead(int userId, int id) {

            if(userId !=int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            return  Unauthorized();

           var message= await _repo.GetMessage(id);

           if(message.RecipientId != userId)
           return Unauthorized();

           message.IsRead=true;
           message.DateRead=DateTime.Now;
           await _repo.SaveAll();
           return NoContent();

      }

    }
}
