using BlogSite.Application.Contracts;
using BlogSite.Application.Features.Categories.Commands;
using BlogSite.Application.Features.Categories.Queries;
using BlogSite.Domain.Entities;
using BlogSite.Domain.Utilities;
using BlogSite.Infrastructure.Extensions;
using BlogSite.Web.Areas.Admin.Models;
using BlogSite.Web.Areas.Admin.Models.Category;
using BlogSite.Web.Codes;
using Cortex.Mediator;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace BlogSite.Web.Areas.Admin.Controllers
{
    public class CategoryController(IMediator mediator,
        IMapper mapper,
        ILogger<CategoryController> logger,
        IServerTime serverTime) : Controller
    {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<CategoryController> _logger = logger;
        private readonly IServerTime _serverTime = serverTime;


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetCategoryData([FromBody] CategoryListModel model)
        {
            try
            {
                var query = _mapper.Map<GetCategoriesAdminQuery>(model);
                query.SearchText = model.Search.Value;
                query.SortOrder = model.FormatSortExpression("Name", "CreatedAt");

                var (items, total, totalDisplay) = await _mediator.SendQueryAsync<GetCategoriesAdminQuery, 
                    (IList<Category>, int, int)>(query);

                var categories = new
                {
                    recordsTotal = total,
                    recordsFiltered = totalDisplay,

                    data = (from category in items
                            select new string[]
                            {
                                HttpUtility.HtmlEncode(category.Name),
                                HttpUtility.HtmlEncode(category.Blogsite.Name),
                                HttpUtility.HtmlEncode(category.CreatedAt
                                    .ToLocalTime()
                                    .ToString("dd MMM yyyy, hh:mm tt")),
                                category.Id.ToString()
                            }).ToArray()
                };

                return Json(categories);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching category data");
                return Json(DataTables.EmptyResult);
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var deleteCommand = new CategoryDeleteCommand { Id = id };
                var deletedId = await _mediator.SendCommandAsync<CategoryDeleteCommand, 
                    Guid>(deleteCommand);

                TempData.Put(Constants.ResponseTemkey,
                    new ResponseModel
                    {
                        Message = "Category deleted successfully",
                        Type = ResponseType.Success
                    });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete category with id: {id}");

                TempData.Put(Constants.ResponseTemkey,
                    new ResponseModel
                    {
                        Message = " Failed to delete category",
                        Type = ResponseType.Danger
                    });
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(Guid id)
        {
            try
            {
                var query = new GetCategoryQuery { Id = id };
                var category = await _mediator.SendQueryAsync<GetCategoryQuery, 
                    Category?>(query);
                if (category is null)
                {
                    TempData.Put(Constants.ResponseTemkey,
                        new ResponseModel
                        {
                            Message = "Category is not found",
                            Type = ResponseType.Danger
                        });
                    return RedirectToAction(nameof(Index));
                }

                var model = _mapper.Map<UpdateCategoryModel>(category);
                return View(model);

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, 
                    $"Error occurred while fetching category for edit with Id {id}");

                TempData.Put(Constants.ResponseTemkey,
                    new ResponseModel
                    {
                        Message = "Failed to load category for editing",
                        Type = ResponseType.Danger
                    });
                return RedirectToAction(nameof(Index));
            }
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
                var checkQuery = new CategoryExistCheckQuery { Name = model.Name, Id = model.Id };
                var IsExists = await _mediator.SendQueryAsync<CategoryExistCheckQuery, bool>(checkQuery);

                if (!IsExists)
                {
                    var command = _mapper.Map<CategoryUpdateCommand>(model);
                    command.UpdatedAt = _serverTime.DateTime;

                    await _mediator.SendCommandAsync<CategoryUpdateCommand, Category>(command);

                    TempData.Put(Constants.ResponseTemkey,
                        new ResponseModel
                        {
                            Message = "Category updated successfully",
                            Type = ResponseType.Success
                        });

                    return RedirectToAction(nameof(Index));
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
                ModelState.AddModelError("Error", "Failed to update category");

                _logger.LogError(ex, "Error occurred while updating category");

                TempData.Put(Constants.ResponseTemkey,
                    new ResponseModel
                    {
                        Message = "Failed to update category",
                        Type = ResponseType.Danger
                    });

                return View(model);
            }
        }

    }
}
