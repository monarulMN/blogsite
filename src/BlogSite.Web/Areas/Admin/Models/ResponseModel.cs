namespace BlogSite.Web.Areas.Admin.Models
{
    public enum ResponseType
    {
        Success,
        Danger
    }
    public class ResponseModel
    {
        public string? Message { get; set; }
        public ResponseType Type { get; set; }
    }
}
