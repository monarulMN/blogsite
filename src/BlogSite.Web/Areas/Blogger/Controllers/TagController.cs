using BlogSite.Application.Contracts;
using BlogSite.Application.Features.Tags.Commands;
using BlogSite.Application.Features.Tags.Queries;
using BlogSite.Domain.Entities;
using BlogSite.Web.Areas.Blogger.Models;
using Cortex.Mediator;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Web.Areas.Blogger.Controllers
{
    [Area("Blogger"), Authorize(Roles = "Blogger")]
    public class TagController(IServerTime serverTime,
        ILogger<TagController> logger,
        IMapper mapper,
        IMediator mediator) : Controller
    {
        private readonly IServerTime _serverTime = serverTime;
        private readonly ILogger<TagController> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IMediator _mediator = mediator;
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddTagModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(null);
            }

            try
            {
                var query = new TagExistCheckQuery { Name = model.Name };
                var isExist = await _mediator.SendQueryAsync<TagExistCheckQuery, bool>(query);

                if (!isExist)
                {
                    var command = _mapper.Map<TagAddCommand>(model);
                    command.CreatedAt = _serverTime.DateTime;
                    command.UpdatedAt = _serverTime.DateTime;
                    var tag = await _mediator.SendCommandAsync<TagAddCommand, Tag>(command);

                    return Json(new { id = tag.Id, text = tag.Name });
                }
                else
                {
                    return Json(null);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "Faild to create tag.");

                _logger.LogError(ex, "Error occurred while creating tag");

                return Json(null);
            }
        }


        [HttpPost]
        public async Task<IActionResult> GetTagSearchData([FromBody] Select2SearchModel model)
        {
            try
            {
                var command = new TagSearchQuery { Name = model.SearchText };
                var tags = await _mediator.SendQueryAsync<TagSearchQuery, 
                    IList<Tag>>(command);

                var data = (from tag in tags 
                            select new 
                            { 
                                id = tag.Id, 
                                text = tag.Name 
                            }).ToArray();

                return Json(data);
            }
            catch (Exception)
            {
                return Json(null);
            }
        }
    }
}
