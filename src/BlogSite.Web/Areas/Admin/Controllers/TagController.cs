using BlogSite.Application.Contracts;
using BlogSite.Application.Features.Tags.Queries;
using BlogSite.Domain.Entities;
using BlogSite.Domain.Utilities;
using BlogSite.Web.Areas.Admin.Models.Tag;
using Cortex.Mediator;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace BlogSite.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles ="Admin, Support")]
    public class TagController(IMediator mediator, 
        IMapper mapper, 
        ILogger<TagController> logger, 
        IServerTime serverTime) 
        : Controller
    {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<TagController> _logger = logger;
        private readonly IServerTime _serverTime = serverTime;

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetTagData([FromBody] TagListModel model)
        {
            try
            {
                var query = _mapper.Map<GetTagsQuery>(model);
                query.SearchText = model.Search.Value;
                query.SortOrder = model.FormatSortExpression("Name", "CreatedAt");

                var (items, total, totalDisplay) = await _mediator.SendQueryAsync< GetTagsQuery, 
                    (IList<Tag>, int, int) > (query);

                var tags = new
                {
                    recordsTotal = total,
                    recordsFiltered = totalDisplay,

                    data = (from tag in items
                            select new string[]
                            {
                                HttpUtility.HtmlEncode(tag.Name),
                                HttpUtility.HtmlEncode(tag.CreatedAt
                                    .ToLocalTime()
                                    .ToString("dd MMM yyyy, hh:mm tt")),
                                tag.Id.ToString()
                            }).ToArray()
                };
                return Json(tags);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching tag data");
                return Json(DataTables.EmptyResult);
            }
        }
    }
}
