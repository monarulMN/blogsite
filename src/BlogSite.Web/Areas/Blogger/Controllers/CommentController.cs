using BlogSite.Application.Contracts;
using BlogSite.Application.Features.BlogSites.Queries;
using BlogSite.Application.Features.Comments.Commands;
using BlogSite.Application.Features.Comments.Queries;
using BlogSite.Domain.Entities;
using BlogSite.Domain.Utilities;
using BlogSite.Infrastructure.Extensions;
using BlogSite.Infrastructure.Identity;
using BlogSite.Web.Areas.Blogger.Models;
using BlogSite.Web.Areas.Blogger.Models.Comment;
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
    public class CommentController(UserManager<ApplicationUser> userManager,
        IServerTime serverTime,
        ILogger<CommentController> logger,
        IMapper mapper,
        IMediator mediator) : Controller
    {

        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<CommentController> _logger = logger;
        private readonly IServerTime _serverTime = serverTime;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetCommentData([FromBody] CommentListModel model)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var blogSiteQuery = new GetBlogSiteByUserQuery { AuthorId = user.Id };
                var blogSite = await _mediator.SendQueryAsync<GetBlogSiteByUserQuery, 
                    Blogsite>(blogSiteQuery);

                var query = _mapper.Map<GetCommentsForBlogSiteManagerQuery>(model);
                query.SearchText = model.Search.Value;
                query.BlogSiteId = blogSite.Id;
                query.SortOrder = model.FormatSortExpression("Name", "Email", "Text", "Title", "Status", "CreateAt");

                var (items, total, totalDisplay) = await _mediator.SendQueryAsync<GetCommentsForBlogSiteManagerQuery,
                    (IList<Comment>, int, int)>(query);

                var comments = new
                {
                    recordsTotal = total,
                    recordsFiltered = totalDisplay,

                    data = (from comment in items
                            select new string[]
                    {
                        HttpUtility.HtmlEncode(comment.Name),
                        HttpUtility.HtmlEncode(comment.Email),
                        HttpUtility.HtmlEncode(comment.Text),
                        HttpUtility.HtmlEncode(comment.BlogPost.Title),
                        HttpUtility.HtmlEncode(comment.Status),
                        HttpUtility.HtmlEncode(comment.CreateAt
                            .ToLocalTime().ToString("dd MMM yyyy, hh:mm tt")),
                        comment.Id.ToString()
                    }).ToArray()
                };


                return Json(comments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching comments data");
                return Json(DataTables.EmptyResult);
            }
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var deleteCommand = new CommentDeleteCommand { CommentId = id };
                var deletedId = await _mediator.SendCommandAsync<CommentDeleteCommand, 
                    Guid>(deleteCommand);

                TempData.Put(Constants.ResponseTemkey, 
                    new ResponseModel
                    {
                        Message = $"Comment Deleted Successfully",
                        Type = ResponseType.Success
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete comment with Id: {id}");

                TempData.Put(Constants.ResponseTemkey, 
                    new ResponseModel
                    {
                        Message = "Failed To Delete comment",
                        Type = ResponseType.Danger
                    });
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> ApproveComment(Guid id)
        {
            try
            {
                var command = new CommentApproveCommand { CommentId = id };
                var deletedId = await _mediator.SendCommandAsync<CommentApproveCommand, 
                    Guid>(command);

                TempData.Put(Constants.ResponseTemkey, 
                    new ResponseModel
                    {
                        Message = $"Comment approved successfully",
                        Type = ResponseType.Success
                    });

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to approve comment with Id: {id}");

                TempData.Put(Constants.ResponseTemkey, 
                    new ResponseModel
                    {
                        Message = "Failed To approve comment",
                        Type = ResponseType.Danger
                    });

                return StatusCode(500);
            }
        }
    }
}
