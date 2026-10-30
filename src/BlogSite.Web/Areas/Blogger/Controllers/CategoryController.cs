using BlogSite.Application.Contracts;
using BlogSite.Application.Features.BlogSites.Queries;
using BlogSite.Application.Features.Categories.Commands;
using BlogSite.Application.Features.Categories.Queries;
using BlogSite.Domain.Entities;
using BlogSite.Infrastructure.Extensions;
using BlogSite.Infrastructure.Identity;
using BlogSite.Web.Areas.Blogger.Models.Category;
using BlogSite.Web.Areas.Blogger.Models;
using BlogSite.Web.Codes;
using Cortex.Mediator;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Web.Areas.Blogger.Controllers
{
    [Area("Blogger"), Authorize(Roles ="Blogger")]
    public class CategoryController(UserManager<ApplicationUser> userManager,
        IServerTime serverTime,
        ILogger<CategoryController> logger,
        IMediator mediator,
        IMapper mapper) : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IServerTime _serverTime = serverTime;
        private readonly ILogger<CategoryController> _logger = logger;
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddCategoryModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var user = await _userManager.GetUserAsync(User);
                var query = new GetBlogSiteByUserQuery { AuthorId = user.Id };

                var blogSite = await _mediator.SendQueryAsync<GetBlogSiteByUserQuery, Blogsite>(query);

                var checkQuery = new CategoryExistCheckQuery { Name = model.Name, BlogSiteId = blogSite.Id };
                var isExists = await _mediator.SendQueryAsync<CategoryExistCheckQuery, bool>(checkQuery);

                if (!isExists)
                {
                    var command = _mapper.Map<CategoryAddCommand>(model);
                    var result = await _mediator.SendCommandAsync<CategoryAddCommand, Category>(command);

                    TempData.Put(Constants.ResponseTemkey,
                        new ResponseModel
                        {
                            Message = "Category created successfully",
                            Type = ResponseType.Success
                        });

                    return RedirectToAction("Create");

                }
                else
                {
                    TempData.Put(Constants.ResponseTemkey,
                        new ResponseModel
                        {
                            Message = "Category already exists",
                            Type = ResponseType.Danger
                        });

                    return View(model);
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("Error", "Faild to create category.");

                _logger.LogError(ex, "Error occurred while creating category");

                TempData.Put(Constants.ResponseTemkey,
                    new ResponseModel
                    {
                        Message = "Faild to create category.",
                        Type = ResponseType.Danger
                    });

                return View(model);
            }
        }


        public async Task<IActionResult> Update(Guid id)
        {
            var query = new GetCategoryQuery { Id = id };
            var category = await _mediator.SendQueryAsync<GetCategoryQuery, Category>(query);

            var model = _mapper.Map<UpdateCategoryModel>(category);

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateCategoryModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var user = await _userManager.GetUserAsync(User);
                var query = new GetBlogSiteByUserQuery { AuthorId = user.Id };
                var blogSite = await _mediator.SendQueryAsync<GetBlogSiteByUserQuery, Blogsite>(query);

                var checkQuery = new CategoryExistCheckQuery
                {
                    Id = model.Id,
                    Name = model.Name,
                    BlogSiteId = blogSite.Id
                };

                var IsExists = await _mediator.SendQueryAsync<CategoryExistCheckQuery, bool>(checkQuery);


                if (!IsExists)
                {
                    var command = new CategoryUpdateCommand();
                    command.Id = model.Id;
                    command.Name = model.Name;
                    command.UpdatedAt = _serverTime.DateTime;

                    await _mediator.SendCommandAsync<CategoryUpdateCommand, Category>(command);


                    TempData.Put(Constants.ResponseTemkey, 
                        new ResponseModel
                        {
                            Message = "Category updated successfully.",
                            Type = ResponseType.Success
                        });

                    return RedirectToAction("Index");
                }
                else
                {
                    TempData.Put(Constants.ResponseTemkey,
                        new ResponseModel
                        {
                            Message = "Category already exists.",
                            Type = ResponseType.Danger
                        });

                    return View(model);
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "Faild to update category.");

                _logger.LogError(ex, "Error occurred while update category");

                TempData.Put(Constants.ResponseTemkey,
                    new ResponseModel
                    {
                        Message = "Faild to update category.",
                        Type = ResponseType.Danger
                    });

                return View(model);
            }
        }
    }
}
