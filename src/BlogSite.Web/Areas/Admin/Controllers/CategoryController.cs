using BlogSite.Application.Contracts;
using BlogSite.Application.Features.Categories.Queries;
using BlogSite.Domain.Entities;
using BlogSite.Domain.Utilities;
using BlogSite.Web.Areas.Admin.Models.Category;
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
                                    .ToString("dd MMM yyyy, hh: mm tt")),
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
    }
}
