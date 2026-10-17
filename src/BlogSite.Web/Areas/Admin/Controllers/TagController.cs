using BlogSite.Application.Contracts;
using BlogSite.Application.Features.Tags.Commands;
using BlogSite.Application.Features.Tags.Queries;
using BlogSite.Domain.Entities;
using BlogSite.Domain.Utilities;
using BlogSite.Infrastructure.Extensions;
using BlogSite.Web.Areas.Admin.Models;
using BlogSite.Web.Areas.Admin.Models.Tag;
using BlogSite.Web.Codes;
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddTagModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var query = new TagExistCheckQuery { Name = model.Name };
                var isExists = await _mediator.SendQueryAsync<TagExistCheckQuery, bool>(query);

                if (!isExists)
                {
                    var command = _mapper.Map<TagAddCommand>(model);
                    command.CreatedAt = _serverTime.DateTime;
                    command.UpdatedAt = _serverTime.DateTime;
                    await _mediator.SendCommandAsync<TagAddCommand, Tag>(command);

                    TempData.Put(Constants.ResponseTemkey,
                        new ResponseModel
                        {
                            Message = "Tag created successfully",
                            Type = ResponseType.Success
                        });
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData.Put(Constants.ResponseTemkey,
                        new ResponseModel
                        {
                            Message = "Tag already exists",
                            Type = ResponseType.Danger
                        });
                    return View(model);
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("Error", "Faild to create tag.");
                _logger.LogError(ex, "Error occurred while creating tag");

                TempData.Put(Constants.ResponseTemkey,
                    new ResponseModel
                    {
                        Message = "Failed to create tag",
                        Type = ResponseType.Danger
                    });

                return View(model);
            }
        }


        public async Task<IActionResult> Update(Guid id)
        {
            try
            {
                var query = new TagGetQuery { Id = id };
                var tag = await _mediator.SendQueryAsync<TagGetQuery, Tag>(query);

                if(tag is null)
                {
                    TempData.Put(Constants.ResponseTemkey,
                        new ResponseModel
                        {
                            Message = " Tag not found",
                            Type = ResponseType.Danger
                        });
                    return RedirectToAction(nameof(Index));
                }

                var model = _mapper.Map<UpdateTagModel>(tag);
                return View(model);

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching tag for edit with id {tagId}", id);
                TempData.Put(Constants.ResponseTemkey,
                    new ResponseModel
                    {
                        Message = "Failed to load tag for editing.",
                        Type = ResponseType.Danger
                    });

                return RedirectToAction("Index");
            }
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateTagModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var query = new TagExistCheckQuery { Name = model.Name, Id = model.Id };
                var isExist = await _mediator.SendQueryAsync<TagExistCheckQuery, bool>(query);
                if (!isExist)
                {
                    var command = _mapper.Map<TagEditCommand>(model);
                    await _mediator.SendCommandAsync<TagUpdateCommand, Tag>(command);

                    TempData.Put(Constants.ResponseTemkey,
                        new ResponseModel
                        {
                            Message = "Tag updated successfully.",
                            Type = ResponseType.Success
                        });

                    return RedirectToAction("Index");
                }
                else
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Tag already exists.",
                        Type = ResponseType.Danger
                    });
                    return View(model);
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("Error", "Failed to update Tag.");
                _logger.LogError(ex, "Error occurred while updating Tag");
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Failed to update tag.",
                    Type = ResponseType.Danger
                });
                return View(model);
            }
        }
    }
}
