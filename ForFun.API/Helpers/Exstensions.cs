using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ForFun.API.Helpers
{
    public static class Exstensions
    {
        public static int CalculateAge(this DateTime date){

                  var age=DateTime.Today.Year-date.Year;
                  if(date.AddYears(age)>DateTime.Today)
                  age--;
                  return age;
        }

        public static void AddPagination(this HttpResponse response,int currentPage,int itemsPerPage,
        int totalItems,int totalPages ) {
            var camelCase=new JsonSerializerSettings();
           camelCase.ContractResolver= new CamelCasePropertyNamesContractResolver();
         var paginationHeader=new PaginationHeader(currentPage,itemsPerPage,totalItems,totalPages);
         response.Headers.Add("Pagination",JsonConvert.SerializeObject(paginationHeader,camelCase));
         response.Headers.Add("Access-Control-Expose-Headers","Pagination");
        }
    }
}