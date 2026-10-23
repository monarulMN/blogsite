namespace BlogSite.Web.Areas.Reader.Models
{
    public class CommentViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Text { get; set; } = null!;
        public DateTime CreateAt { get; set; }
    }
}
