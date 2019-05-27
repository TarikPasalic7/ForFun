using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForFun.API.Helpers;
using ForFun.API.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace ForFun.API.Data
{
    public class FunRepository : IFunRepository
    {
          private readonly DataContext _context;
        public FunRepository(DataContext context)
        {
            _context=context;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<Like> GetLike(int userId, int recipientId)
        {
            return await _context.Likes.FirstOrDefaultAsync(u => u.LikerId == userId && u.LikeeId == recipientId);
        }

        public  async Task<Photo> GetMainPhotoUser(int Userid)
        {
            return await _context.Photos.Where(u=>u.UserId==Userid).FirstOrDefaultAsync(p=>p.mainphoto);
        }

        public async Task<Photo> GetPhoto(int id)
        {
             var photo=await _context.Photos.FirstOrDefaultAsync(p=>p.Id==id);
             return photo;
        }

        public async Task<User> GetUser(int id)
        {
            var user= await _context.Users.Include(p=>p.Photos).FirstOrDefaultAsync(u=>u.Id==id);
            return user;
        }


       

        public async Task<PagedList<User>> Getusers(UserParams userParams)
        {
            var users=  _context.Users.Include(p=>p.Photos).OrderByDescending(u=> u.LastActive).AsQueryable();
            users= users.Where(u => u.Id != userParams.UserId);
            users= users.Where(u => u.Gender == userParams.Gender);


            if(userParams.Likers) {

              var userlikers= await GetLikes(userParams.UserId,userParams.Likers);
              users=users.Where(u => userlikers.Contains(u.Id));
            }

            if(userParams.Likees) {

                    var userlikees= await GetLikes(userParams.UserId,userParams.Likers);
              users=users.Where(u => userlikees.Contains(u.Id));
            }

            if(userParams.MinAge!=18 || userParams.MaxAge!=99){
                  var mindate=DateTime.Today.AddYears(-userParams.MaxAge-1);
                  var maxdate=DateTime.Today.AddYears(-userParams.MinAge);
                   users= users.Where(u => u.BirthDate>=mindate && u.BirthDate<=maxdate);

            }
           if(!string.IsNullOrEmpty(userParams.OrderBy)){
                   
                   switch(userParams.OrderBy)
                   {
                      case "created":
                      users=users.OrderByDescending(u=>u.Created);
                      break;
                      default:
                      users=users.OrderByDescending(u=>u.LastActive);
                      break;


                   }

           }
            return await PagedList<User>.CreateAsync(users,userParams.pageNumber,userParams.PageSize);
        }

        private async Task<IEnumerable<int>> GetLikes(int id,bool likers) {

        var user= await _context.Users.Include(u => u.Likers).Include(us => us.Likees).FirstOrDefaultAsync(i => i.Id==id);

        if(likers)
        {
            return user.Likers.Where(u => u.LikeeId==id).Select(i => i.LikerId);
        }
        else {
              
               return user.Likees.Where(u => u.LikerId==id).Select(i => i.LikeeId);

        }

        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() >0;
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FirstOrDefaultAsync(m=>m.Id ==id);
        }

        public async Task<PagedList<Message>> GetMessagesforUser(MessageParams messageParams)
        {
            var messages = _context.Messages.Include(u=> u.Sender).ThenInclude(p=>p.Photos).Include(u=> u.Recipient).ThenInclude(p=>p.Photos).AsQueryable();

            switch(messageParams.MessageContainer) {
               case "Inbox":
               messages=messages.Where(u=>u.RecipientId==messageParams.UserId && u.RecipientDeleted==false);
               break;
               case "Outbox":
                messages=messages.Where(u=>u.SenderId==messageParams.UserId && u.SenderDeleted==false);
                break;
                default:
                messages=messages.Where(u=>u.RecipientId==messageParams.UserId && u.RecipientDeleted==false && u.IsRead==false);
                break;
            }

            messages= messages.OrderByDescending(d=>d.MessageSend);
            return await PagedList<Message>.CreateAsync(messages,messageParams.pageNumber,messageParams.PageSize);
        }

        public async Task<IEnumerable<Message>> GetMessageThread(int userid, int recipientid)
        {
            var messages = await _context.Messages.Include(u=> u.Sender).ThenInclude(p=>p.Photos).Include(u=> u.Recipient).ThenInclude(p=>p.Photos)
            .Where(m=>m.RecipientId ==userid && m.RecipientDeleted==false &&
             m.SenderId==recipientid || m.RecipientId==recipientid && m.SenderId==userid && m.SenderDeleted==false)
            .OrderByDescending(m=>m.MessageSend).ToListAsync();
            return messages;
        }
    }
}