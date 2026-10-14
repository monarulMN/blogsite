using BlogSite.Application.Contracts;
using BlogSite.Application.Features.Users.Queries;
using BlogSite.Domain.Entities;
using BlogSite.Domain.Enums;
using BlogSite.Domain.Utilities;
using BlogSite.Infrastructure.Extensions;
using BlogSite.Infrastructure.Identity;
using BlogSite.Web.Areas.Admin.Models;
using BlogSite.Web.Areas.Admin.Models.UserManagement;
using BlogSite.Web.Codes;
using Cortex.Mediator;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace BlogSite.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles ="Admin, Support")]
    public class UserManagementController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<UserManagementController> _logger;
        private readonly IServerTime _serverTime;
        private readonly ApplicationUserManager _userManager;

        public UserManagementController(IMediator mediator,
            IMapper mapper,
            ILogger<UserManagementController> logger,
            IServerTime serverTime,
            ApplicationUserManager userManager)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
            _serverTime = serverTime;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> GetUserDataAsync([FromBody] UserListModel model)
        {
            try
            {
                var query = _mapper.Map<GetAllUsersByPagingQuery>(model);
                query.SearchText = model.Search.Value;
                query.SortOrder = model.FormatSortExpression("FirstName", "Email", "HasBlog", "BlogSite");

                var (data, total, totalDisplay) = await _mediator.SendQueryAsync<GetAllUsersByPagingQuery, 
                    (IList<User> data, int total, int totalDisplay)>(query);

                var roleDictionary = new Dictionary<Guid, IList<string>>();
                foreach(var user in data)
                {
                    ApplicationUser? appUser = await _userManager.FindByEmailAsync(user.Email);
                    var roles = await _userManager.GetRolesAsync(appUser);
                    roleDictionary[user.Id] = roles;
                }

                var users = new
                {
                    recordsTotal = total,
                    recordsFiltered = totalDisplay,

                    data = data.Select(user => new string[]
                    {
                        HttpUtility.HtmlEncode($"{user.FirstName} {user.LastName}"),
                        HttpUtility.HtmlEncode($"{user.Email}"),
                        HttpUtility.HtmlEncode($"{user.HasBlog.ToString()}"),

                        HttpUtility.HtmlEncode(
                            user.BlogSite == null
                                ? "None"
                                : user.BlogSite.Status == Status.Inactive
                                    ? "<span class = 'badge bg-danger'> Restricted </span>"
                                    : user.BlogSite.Name
                        ),

                        HttpUtility.HtmlEncode(
                            roleDictionary.ContainsKey(user.Id)
                                ? string.Join(" ", roleDictionary[user.Id]
                                    .Select(r => $"<span class= 'badge bg-info me-1' onclick=\"removeRole('{user.Id}', '{r}'\" style='cursor:pointer'>{r}</span>"))
                                : ""
                        ),

                        user.Id.ToString()
                    }).ToArray()
                };

                return Json(model);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Eror occurred while fetching user data");
                return Json(DataTables.EmptyResult);
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> BlockAsync(Guid id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                user.LockoutEnabled = true;
                user.LockoutEnd = DateTimeOffset.MaxValue;
                user.IsBlocked = true;
                await _userManager.UpdateAsync(user);

                TempData.Put(Constants.ResponseTemkey,
                    new ResponseModel
                    {
                        Message = $"User Blocked successfully",
                        Type = ResponseType.Success
                    });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Failed to block user with Id: {id}");
                TempData.Put(Constants.ResponseTemkey,
                    new ResponseModel
                    {
                        Message = "Failed to block user",
                        Type = ResponseType.Danger
                    });
            }

            return RedirectToAction("Index");
        }


        public IActionResult BlackListedUserList()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> GetBlockedUserData([FromBody] UserListModel model)
        {
            try
            {
                var query = _mapper.Map<GetAllBlockedUsersQuery>(model);
                query.SearchText = model.Search.Value;
                query.SortOrder = model.FormatSortExpression("FirstName", "Email", "HasBlog", "BlogSite");

                var (data, total, totalDisplay) = await _mediator.SendQueryAsync<GetAllBlockedUsersQuery,
                    (IList<User> data, int total, int totalDisplay)>(query);

                var users = new
                {
                    recordsTotal = total,
                    recordsFiltered = totalDisplay,

                    data = (from user in data
                            select new string[]
                            {
                                HttpUtility.HtmlEncode($"{user.FirstName} {user.LastName}"),
                                HttpUtility.HtmlEncode(user.Email),
                                HttpUtility.HtmlEncode(user.HasBlog.ToString()),
                                HttpUtility.HtmlEncode(
                                    user.BlogSite == null?
                                        "None" :
                                            user.BlogSite.Status == Status.Inactive ?
                                                "<span class='badge bg-danger'>Restricted</span>" :
                                                    user.BlogSite.Name),
                                user.Id.ToString()
                            }).ToArray()
                };

                return Json(users);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching user data");
                return Json(DataTables.EmptyResult);
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> UnBlockAsync(Guid id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                user.LockoutEnabled = true;
                user.LockoutEnd = null;
                user.IsBlocked = false;
                await _userManager.UpdateAsync(user);

                TempData.Put(Constants.ResponseTemkey,
                    new ResponseModel
                    {
                        Message = $"User Unblocked successfully",
                        Type = ResponseType.Success
                    });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Failed to unblocked user with Id: {id}");
                TempData.Put(Constants.ResponseTemkey,
                    new ResponseModel
                    {
                        Message = "Failed to unblock user",
                        Type = ResponseType.Danger
                    });
            }

            return RedirectToAction("BlackListedUserList");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUserRoleAsync(string userId, string roleName)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                await _userManager.RemoveFromRoleAsync(user, roleName);
                await _userManager.UpdateAsync(user);

                TempData.Put(Constants.ResponseTemkey,
                    new ResponseModel
                    {
                        Message = $"Successfully removed role",
                        Type = ResponseType.Success
                    });
                return Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Failed to remove user role");
                TempData.Put(Constants.ResponseTemkey,
                    new ResponseModel
                    {
                        Message = "Failed to remove user role",
                        Type = ResponseType.Danger
                    });

                return StatusCode(500);
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUserRoleAsync(AddUserRoleModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData.Put(Constants.ResponseTemkey,
                    new ResponseModel
                    {
                        Message = $"Model state is not valid",
                        Type = ResponseType.Danger
                    });

                return BadRequest();
            }

            try
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                await _userManager.AddToRoleAsync(user, model.RoleName);
                await _userManager.UpdateAsync(user);

                TempData.Put(Constants.ResponseTemkey,
                    new ResponseModel
                    {
                        Message = $"Successfully removed role",
                        Type = ResponseType.Success
                    });
                return Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Failed to Add user role");
                TempData.Put(Constants.ResponseTemkey,
                    new ResponseModel
                    {
                        Message = "Failed to add user role",
                        Type = ResponseType.Danger
                    });

                return StatusCode(500);
            }
        }
    }
}
