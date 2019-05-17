using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ForFun.API.Helpers
{
    public class PagedList<T> : List<T>
    
    {
        public int currentPage { get; set; }
         public int totalPages { get; set; }
          public int sizePage { get; set; }
           public int totalCount { get; set; }
        public PagedList(List<T> items,int count ,int pageNumber,int pageSize)
        {
            totalCount=count;
            sizePage=pageSize;
            currentPage=pageNumber;

           totalPages=(int)Math.Ceiling(count/(double)sizePage);
           this.AddRange(items);
        }
        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source,int pageNumber,int pageSize){
         var count = await source.CountAsync();
         var items= await source.Skip((pageNumber-1) * pageSize).Take(pageSize).ToListAsync();
         return new PagedList<T>(items,count,pageNumber,pageSize);
        }
    }
}