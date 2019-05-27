using System.Linq;
using AutoMapper;
using ForFun.API.Dtos;
using ForFun.API.Models;

namespace ForFun.API.Helpers
{
    public class AutomapperProfiles: Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<User,UserforListDto>().ForMember(d=>d.photoURL,p=>{
              
              p.MapFrom(s=>s.Photos.FirstOrDefault(t=>t.mainphoto).URL);

            }).ForMember(mem=>mem.age,op=>{

                op.ResolveUsing(d=>d.BirthDate.CalculateAge());
            });
            CreateMap<User,UserDetailDto>().ForMember(d=>d.photoURL,p=>{
              
              p.MapFrom(s=>s.Photos.FirstOrDefault(t=>t.mainphoto).URL);

            }).ForMember(mem=>mem.age,op=>{

                op.ResolveUsing(d=>d.BirthDate.CalculateAge());
            });;
            CreateMap<Photo,PhotoDetailsDto>();
            CreateMap<UserForUpdateDto,User>();
            CreateMap<Photo,PhotoForReturnDto>();
            CreateMap<PhotoCreatingDto,Photo>();
            CreateMap<UserForRegisterDto,User>();
            CreateMap<MessageForCreationDto,Message>().ReverseMap();
            CreateMap<Message,MessageToReturnDto>()
            .ForMember(m=>m.SenderPhotoUrl,opt=>opt.MapFrom(u=>u.Sender.Photos.FirstOrDefault(p=>p.mainphoto).URL))
             .ForMember(m=>m.RecipientPhotoUrl,opt=>opt.MapFrom(u=>u.Recipient.Photos.FirstOrDefault(p=>p.mainphoto).URL));
        }
    }
}