namespace API.Helpers
{
    public class UserParams : PaginationParams
    {
      
        public string CurrentUerName { get; set; }
        public string Gender { get; set; } 
        public int MinAge { get; set; } = 14;
        public int MaxAge { get; set; } = 100;
        public string OrderBy { get; set; } = "lastActive";

    }
}
