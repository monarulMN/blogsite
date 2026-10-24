namespace BlogSite.Web.Areas.Reader.Models.Blog
{
    public class PublicPagesBlogSiteModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public int PostCount { get; set; }
    }
}
