namespace BlogSite.Web.Areas.Reader.Models
{
    public class PublicPagesTagModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int PostCount { get; set; }
    }
}
