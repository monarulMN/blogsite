using BlogSite.Application.Contracts;
using BlogSite.Application.Features.BlogPosts.Commands;
using BlogSite.Application.Features.BlogPosts.Queries;
using BlogSite.Application.Features.BlogSites.Queries;
using BlogSite.Domain.Entities;
using BlogSite.Domain.Utilities;
using BlogSite.Infrastructure.Extensions;
using BlogSite.Infrastructure.Identity;
using BlogSite.Web.Areas.Blogger.Models;
using BlogSite.Web.Areas.Blogger.Models.Category;
using BlogSite.Web.Areas.Blogger.Models.Post;
using BlogSite.Web.Codes;
using Cortex.Mediator;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace BlogSite.Web.Areas.Blogger.Controllers
{
    [Area("Blogger"), Authorize(Roles = "Blogger")]
    public class PostController(UserManager<ApplicationUser> userManager,
        IServerTime serverTime,
        ILogger<PostController> logger,
        IMapper mapper,
        IMediator mediator,
        IWebHostEnvironment environmen) : Controller
    {

        private readonly IWebHostEnvironment _environment = environmen;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IServerTime _serverTime = serverTime;
        private readonly ILogger<PostController> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IMediator _mediator = mediator;

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddBlogPostModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Error", "Model state not valid.");
                return View();
            }

            try
            {
                var user = await _userManager.GetUserAsync(User);
                var query = new GetBlogSiteByUserQuery { AuthorId = user.Id };
                var blogSite = await _mediator.SendQueryAsync<GetBlogSiteByUserQuery, 
                    Blogsite>(query);

                var imageFileName = await model.SaveImageFile(_environment);
                var command = _mapper.Map<BlogPostAddCommand>(model);
                command.BlogSiteId = blogSite.Id;
                command.ThumbnailPath = imageFileName;
                command.CreatedAt = _serverTime.DateTime;
                command.UpdatedAt = _serverTime.DateTime;


                await _mediator.SendCommandAsync<BlogPostAddCommand, BlogPost>(command);

                TempData.Put(Constants.ResponseTemkey, 
                    new ResponseModel
                    {
                        Message = "blog post created successfully.",
                        Type = ResponseType.Success
                    });

                return RedirectToAction("Create");

            }
            catch (Exception ex)
            {
                TempData.Put(Constants.ResponseTemkey, 
                    new ResponseModel
                    {
                        Message = "Error occourred while creating blog post.",
                        Type = ResponseType.Danger
                    });

                _logger.LogError(ex, "Error occourred while creating blog post.");

                return View(model);
            }
        }


        [HttpPost]
        public async Task<IActionResult> GetPostData([FromBody] BlogPostListModel model)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var blogSiteQuery = new GetBlogSiteByUserQuery { AuthorId = user.Id };
                var blogSite = await _mediator.SendQueryAsync<GetBlogSiteByUserQuery, 
                    Blogsite>(blogSiteQuery);

                var query = _mapper.Map<GetAllBlogPostsQuery>(model);
                query.SearchText = model.Search.Value;
                query.BlogSiteId = blogSite.Id;
                query.SortOrder = model.FormatSortExpression("Title", "PostStatus", "Body", "CreatedAt");

                var (items, total, totalDisplay) = await _mediator.SendQueryAsync<GetAllBlogPostsQuery, 
                    (IList<BlogPost>, int , int)>(query);

                var posts = new
                {
                    recordsTotal = total,
                    recordsFiltered = totalDisplay,

                    data = (from post in items
                            select new string[]
                    {
                        HttpUtility.HtmlEncode(post.ThumbnailPath),
                        HttpUtility.HtmlEncode(post.Title),
                        HttpUtility.HtmlEncode(post.PostStatus),
                        HttpUtility.HtmlEncode(post.Body),
                        HttpUtility.HtmlEncode(post.CreatedAt
                            .ToLocalTime().ToString("dd MMM yyyy, hh:mm tt")),
                        post.Id.ToString()
                    }).ToArray()
                };

                return Json(posts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching post data");

                return Json(DataTables.EmptyResult);
            }
        }


        public async Task<IActionResult> Update(Guid Id)
        {
            var query = new GetBlogPostQuery { Id = Id };
            var post = await _mediator.SendQueryAsync<GetBlogPostQuery, BlogPost>(query);
            var model = _mapper.Map<UpdateBlogPostModel>(post);

            model.CategoriesData = post.BlogPostCategories
                .Select(x => new CategoryViewModel
                {
                    Id = x.Category.Id,
                    Name = x.Category.Name
                }).ToList();

            model.TagsData = post.BlogPostTags
                .Select(x => new TagViewModel
                {
                    Id = x.Tag.Id,
                    Name = x.Tag.Name
                }).ToList();

            return View(model);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateBlogPostModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Error", "Model state not valid.");
                return View();
            }

            try
            {
                var command = _mapper.Map<BlogPostUpdateCommand>(model);

                if (model.Thumbnail is not null)
                    command.ThumbnailPath = await model.SaveImageFile(_environment);

                command.UpdatedAt = _serverTime.DateTime;
                await _mediator.SendCommandAsync<BlogPostUpdateCommand, 
                    BlogPost>(command);

                TempData.Put(Constants.ResponseTemkey, 
                    new ResponseModel
                    {
                        Message = "blog post updated successfully.",
                        Type = ResponseType.Success
                    });

                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                TempData.Put(Constants.ResponseTemkey, 
                    new ResponseModel
                    {
                        Message = "Error occourred while updating blog post.",
                        Type = ResponseType.Danger
                    });

                _logger.LogError(ex, "Error occourred while updating blog post.");

                return View(model);
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var deleteCommand = new BlogPostDeleteCommand { Id = id };
                var deletedId = await _mediator.SendCommandAsync<BlogPostDeleteCommand, 
                    Guid>(deleteCommand);

                TempData.Put(Constants.ResponseTemkey, 
                    new ResponseModel
                    {
                        Message = $"Post Deleted Successfully",
                        Type = ResponseType.Success
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete Post with Id: {id}");
                TempData.Put(Constants.ResponseTemkey,
                    new ResponseModel
                    {
                        Message = "Failed To Delete Post",
                        Type = ResponseType.Danger
                    });
            }

            return RedirectToAction("Index");
        }
    }
}
