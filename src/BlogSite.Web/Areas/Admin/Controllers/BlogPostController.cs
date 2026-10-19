using BlogSite.Application.Contracts;
using BlogSite.Application.Features.BlogPosts.Queries;
using BlogSite.Domain.Entities;
using BlogSite.Domain.Utilities;
using BlogSite.Web.Areas.Admin.Models.BlogPost;
using Cortex.Mediator;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace BlogSite.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin, Support")]
    public class BlogPostController(IMediator mediator,
        IMapper mapper,
        ILogger<BlogPostController> logger,
        IServerTime serverTime) : Controller
    {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<BlogPostController> _logger = logger;
        private readonly IServerTime _serverTime = serverTime;

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> GetBlogsData([FromBody] BlogPostListModel model)
        {
            try
            {
                var query = _mapper.Map<GetBlogPostsAdminQuery>(model);
                query.SearchText = model.Search.Value;
                query.SortOrder = model.FormatSortExpression("ThumbnailPath", "Title", "Body", "PostStatus", "BlogSite", "CreatedAt");

                var (items, total, totalDisplay) = await _mediator.SendQueryAsync<GetBlogPostsAdminQuery, 
                    (IList<BlogPost>, int, int)>(query);

                var blogs = new
                {
                    recordsTotal = total,
                    recordsFiltered = totalDisplay,

                    data = (from post in items
                            select new string[]
                            {
                               HttpUtility.HtmlEncode(post.ThumbnailPath),
                               HttpUtility.HtmlEncode(post.Title),
                               HttpUtility.HtmlEncode(post.Body),
                               HttpUtility.HtmlEncode(post.PostStatus),
                               HttpUtility.HtmlEncode(post.Blogsite.Name),
                               HttpUtility.HtmlEncode(post.CreatedAt
                                .ToLocalTime()
                                .ToString("dd MMM yyyy, hh:mm tt")),
                               post.Id.ToString()
                            }).ToArray()
                };

                return Json(blogs);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching blog posts data");

                return Json(DataTables.EmptyResult);
            }
        }
    }
}
