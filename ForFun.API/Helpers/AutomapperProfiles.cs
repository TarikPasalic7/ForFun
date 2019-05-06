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
        }
    }
}