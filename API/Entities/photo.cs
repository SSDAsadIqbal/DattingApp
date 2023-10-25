

using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("photos  ")]
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool isMain { get; set; }
        public string PublicId { get; set; }


        public int AppUserId { get; set; }
        public AppUser appUser { get; set; }
    }
}