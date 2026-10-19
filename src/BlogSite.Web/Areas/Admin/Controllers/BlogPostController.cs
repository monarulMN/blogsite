using BlogSite.Application.Contracts;
using BlogSite.Application.Features.BlogPosts.Commands;
using BlogSite.Application.Features.BlogPosts.Queries;
using BlogSite.Domain.Dtos;
using BlogSite.Domain.Entities;
using BlogSite.Domain.Utilities;
using BlogSite.Infrastructure.Extensions;
using BlogSite.Web.Areas.Admin.Models;
using BlogSite.Web.Areas.Admin.Models.BlogPost;
using BlogSite.Web.Areas.Admin.Models.Category;
using BlogSite.Web.Areas.Admin.Models.Tag;
using BlogSite.Web.Codes;
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
        IServerTime serverTime,
        IWebHostEnvironment environment) : Controller
    {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<BlogPostController> _logger = logger;
        private readonly IServerTime _serverTime = serverTime;
        private readonly IWebHostEnvironment _environment = environment;

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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching blog posts data");

                return Json(DataTables.EmptyResult);
            }
        }

        public async Task<IActionResult> Update(Guid id)
        {
            try
            {
                var query = new GetBlogPostQuery { Id = id };
                var post = await _mediator.SendQueryAsync<GetBlogPostQuery, BlogPost?>(query);
                var model = _mapper.Map<UpdateBlogPostModel>(post);
                model.BlogSiteId = post.BlogSiteId;

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
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching blog post for edit with Id: {id}");

                TempData.Put(Constants.ResponseTemkey,
                    new ResponseModel
                    {
                        Message = "Failed to load blogPost for updating data",
                        Type = ResponseType.Danger
                    });

                return RedirectToAction(nameof(Index));

            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateBlogPostModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData.Put(Constants.ResponseTemkey,
                    new ResponseModel
                    {
                        Message = "Model state is invalid while updating blog post",
                        Type = ResponseType.Danger
                    });

                ModelState.AddModelError("Error", "Model state not valid");
                return View();
            }

            try
            {
                var command = _mapper.Map<BlogPostUpdateCommand>(model);

                if (model.Thumbnail is not null)
                {
                    command.ThumbnailPath = await model.SaveImageFileAsync(_environment);
                }

                command.UpdatedAt = _serverTime.DateTime;
                await _mediator.SendCommandAsync<BlogPostUpdateCommand, BlogPost>(command);

                TempData.Put(Constants.ResponseTemkey,
                    new ResponseModel
                    {
                        Message = "Blog post updated successfully ",
                        Type = ResponseType.Success
                    });

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occourred while updating blog post");

                TempData.Put(Constants.ResponseTemkey,
                    new ResponseModel
                    {
                        Message = "Error occourred while updating blog post",
                        Type = ResponseType.Danger
                    });

                return View(model);
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var deleteCommand = new BlogPostDeleteCommand { Id = id };
                var deleteId = await _mediator.SendCommandAsync<BlogPostDeleteCommand, Guid>(deleteCommand);

                TempData.Put(Constants.ResponseTemkey,
                    new ResponseModel
                    {
                        Message = "Blog post deleted successfully",
                        Type = ResponseType.Success
                    });

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete post with id: {id}");

                ModelState.AddModelError("Error", "Failed to delete post");

                TempData.Put(Constants.ResponseTemkey,
                    new ResponseModel
                    {
                        Message = "Failed to delete post",
                        Type = ResponseType.Danger
                    });

            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult AdvanceSearch()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetBlogsDataAdvanceSearch([FromBody] BlogPostListModel model)
        {
            try
            {
                var query = new GetBlogPostsAdvanceSearchQuery
                {
                    PageIndex = model.PageIndex,
                    PageSize = model.PageSize,
                    Title = model.SearchItem.Title,
                    DateFrom = model.SearchItem.DateFrom,
                    DateTo = model.SearchItem.DateTo,
                    SortOrder = model.FormatSortExpression("Title", "Body", "CreatedAt")
                };

                var (blogPosts, total, totalDisplay) = await _mediator.SendQueryAsync<GetBlogPostsAdvanceSearchQuery,
                    (IList<BlogPostDto>, int, int)>(query);

                var posts = new
                {
                    recordsTotal = total,
                    recordsFiltered = totalDisplay,
                    data = (from post in blogPosts
                            select new string[]
                    {
                        HttpUtility.HtmlEncode(post.Title),
                        HttpUtility.HtmlEncode(post.Body),
                        HttpUtility.HtmlEncode(post.CreatedAt),
                        post.Id.ToString()
                    }).ToArray()
                };

                return Json(posts);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occourred while getting post data.");
                return Json(DataTables.EmptyResult);
            }
        }
    }
}
