namespace ForFun.API.Helpers
{
    public class MessageParams
    {
           private const int maxPageSize=30;
        public int pageNumber { get; set; }= 1;

        private int pageSize=10;
        public int PageSize
        {
            get { return pageSize;}
            set { pageSize=(value>maxPageSize)? maxPageSize: value;}
        }
        
        public int UserId { get; set; }

        public string  MessageContainer { get; set; } ="Unread";
    }
}