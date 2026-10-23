namespace BlogSite.Web.Areas.Reader.Models
{
    public class CategoryViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int PostCount { get; set; }
    }
}
