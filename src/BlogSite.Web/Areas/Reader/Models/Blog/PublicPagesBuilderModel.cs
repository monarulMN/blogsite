namespace BlogSite.Web.Areas.Reader.Models.Blog
{
    public class PublicPagesBuilderModel
    {
        public IList<PublicPagesBlogSiteModel> BlogSites { get; set; }
        public IList<PublicPagesTagModel> Tags { get; set; }
    }
}
