using BlogSite.Web.Areas.Reader.Models.Blog;

namespace BlogSite.Web.Areas.Reader.Models
{
    public class PublicPagesBuilderModel
    {
        public IList<PublicPagesBlogSiteModel> BlogSites { get; set; }
        public IList<PublicPagesTagModel> Tags { get; set; }
    }
}
