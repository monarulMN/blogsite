using BlogSite.Application.Contracts;
using BlogSite.Application.Features.BlogSites.Commands;
using BlogSite.Application.Features.BlogSites.Queries;
using BlogSite.Domain.Dtos;
using BlogSite.Domain.Entities;
using BlogSite.Domain.Utilities;
using BlogSite.Infrastructure.Extensions;
using BlogSite.Web.Areas.Admin.Models;
using BlogSite.Web.Areas.Admin.Models.BlogSite;
using BlogSite.Web.Codes;
using Cortex.Mediator;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using System.Web;

namespace BlogSite.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin, Support")]
    public class BlogSiteController(IMediator mediator, 
        IMapper mapper, 
        ILogger<BlogSiteController> logger, 
        IServerTime serverTime ) : Controller
    {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<BlogSiteController> _logger = logger;
        private readonly IServerTime _serverTime = serverTime;

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Update(Guid id)
        {
            var query = new GetBlogSiteQuery { Id = id };
            var blogSite = await _mediator.SendQueryAsync<GetBlogSiteQuery, Blogsite>(query);
            var model = _mapper.Map<UpdateBlogSiteModel>(blogSite);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateBlogSiteModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty,
                    "An error occurred while updateing your blog. Please try again later.");

                TempData.Put(Constants.ResponseTemkey,
                    new ResponseModel
                    {
                        Message = "BlogSite not update",
                        Type = ResponseType.Danger
                    });

                return View(model);
            }

            try
            {
                var command = _mapper.Map<BlogSiteUpdateCommand>(model);
                command.UpdatedAt = _serverTime.DateTime;
                await _mediator.SendCommandAsync<BlogSiteUpdateCommand, Blogsite>(command);

                TempData.Put("ResponseMessage", new Models.ResponseModel
                {
                    Message = "BlogSite updated successfully.",
                    Type = Models.ResponseType.Success
                });

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "BlogSite not updated.",
                    Type = Models.ResponseType.Danger
                });

                _logger.LogError(ex, "Error occurred while updateing blog site");
                ModelState.AddModelError(string.Empty, "An error occurred while updateing your blog. Please try again later.");
                return RedirectToAction(nameof(Index));
            }

        }

        [HttpPost]
        public async Task<IActionResult> GetBlogSiteData([FromBody] BlogSiteListModel model)
        {
            try
            {
                var query = _mapper.Map<GetAllBlogSitesQueries>(model);
                query.SearchText = model.Search.Value;
                query.SortOrder = model.FormatSortExpression("Name");

                var (items, total, totalDisplay) = await _mediator.SendQueryAsync<GetAllBlogSitesQueries,
                    (IList<BlogSiteDto>, int, int)>(query);

                var categories = new
                {
                    recordsTotal = total,
                    recordsFiltered = totalDisplay,

                    data = (from blogSite in items
                            select new string[]
                            {
                                HttpUtility.HtmlEncode(blogSite.Name),
                                HttpUtility.HtmlEncode(blogSite.UserName),
                                HttpUtility.HtmlEncode(blogSite.Email),
                                HttpUtility.HtmlEncode(blogSite.TotalPost),
                                HttpUtility.HtmlEncode(blogSite.TotalPost),
                                HttpUtility.HtmlEncode(blogSite.CreatedAt
                                    .ToLocalTime().ToString("dd MMM yyyy, hh:mm tt")),
                                blogSite.Id.ToString()
                            }).ToArray()
                };

                return Json(categories);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching blog sites data");
                return Json(DataTables.EmptyResult);
            }
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var deleteCommand = new BlogSiteDeleteCommand { Id = id };
                var deletedId = await _mediator.SendCommandAsync<BlogSiteDeleteCommand, Guid>(deleteCommand);

                TempData.Put(Constants.ResponseTemkey,
                    new ResponseModel
                    {
                        Message = $"Blogsite Deleted Successfully",
                        Type = ResponseType.Success
                    });

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete blog site with Id: {id}");
                TempData.Put(Constants.ResponseTemkey,
                    new ResponseModel
                    {
                        Message = "Failed To Delete Blogsite",
                        Type = ResponseType.Danger
                    });
            }
            return RedirectToAction(nameof(Index));
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Active(Guid id)
        {
            try
            {
                var activeCommand = new BlogSiteActiveCommand { Id = id };
                await _mediator.SendCommandAsync<BlogSiteActiveCommand, Guid>(activeCommand);

                TempData.Put(Constants.ResponseTemkey,
                    new ResponseModel
                    {
                        Message = "BlogSite Active Successfully",
                        Type = ResponseType.Danger
                    });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Failed to Active blog site with Id: {id}");
                TempData.Put(Constants.ResponseTemkey, 
                    new ResponseModel
                    {
                        Message = "Failed To Active Blogsite",
                        Type = ResponseType.Danger
                    });
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult InactiveList()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetBlogSiteRestrictedData([FromBody] BlogSiteListModel model)
        {
            try
            {
                var query = _mapper.Map<GetRestrictedBlogSitesQuery>(model);
                query.SearchText = model.Search.Value;
                query.SortOrder = model.FormatSortExpression("Name");

                var (items, total, totalDisplay) = await _mediator.SendQueryAsync<GetRestrictedBlogSitesQuery,
                    (IList<BlogSiteDto>, int , int)>(query);

                var categories = new
                {
                    recordsTotal = total,
                    recordsFiltered = totalDisplay,

                    data = (from blogSite in items
                            select new string[]
                    {
                        HttpUtility.HtmlEncode(blogSite.Name),
                        HttpUtility.HtmlEncode(blogSite.UserName),
                        HttpUtility.HtmlEncode(blogSite.Email),
                        HttpUtility.HtmlEncode(blogSite.TotalPost),
                        HttpUtility.HtmlEncode(blogSite.TotalPost),
                        HttpUtility.HtmlEncode(blogSite.CreatedAt
                            .ToLocalTime()
                            .ToString("dd MMM yyyy, hh:mm tt")),
                        blogSite.Id.ToString()

                    }).ToArray()
                };

                return Json(model);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching blog sites data");
                return Json(DataTables.EmptyResult);
            }
        }
    }
}
