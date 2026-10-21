using BlogSite.Application.Contracts;
using BlogSite.Application.Features.Reports.Commands;
using BlogSite.Application.Features.Reports.Queries;
using BlogSite.Domain.Entities;
using BlogSite.Domain.Utilities;
using BlogSite.Infrastructure.Extensions;
using BlogSite.Web.Areas.Admin.Models;
using BlogSite.Web.Areas.Admin.Models.Report;
using BlogSite.Web.Codes;
using Cortex.Mediator;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace BlogSite.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin, Support")]
    public class ReportController(IMediator mediator, IMapper mapper, 
        ILogger<ReportController> logger, 
        IServerTime serverTime) : Controller
    {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<ReportController> _logger = logger;
        private readonly IServerTime _serverTime = serverTime;

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetReportData([FromBody] ReportListModel model)
        {
            try
            {
                var query = _mapper.Map<GetReportsAdminQuery>(model);
                query.SearchText = model.Search.Value;
                query.SortOrder = model.FormatSortExpression("ReportText", "Title", "BlogSite", "CreatedAt");

                var (items, total, totalDisplay) = await _mediator.SendQueryAsync<GetReportsAdminQuery,
                    (IList<Report> reports, int total, int totalDisplay)>(query);

                var reports = new
                {
                    recordsTotal = total,
                    recordsFiltered = totalDisplay,

                    data = (from report in items
                            select new string[]
                    {
                        HttpUtility.HtmlEncode(report.ReportText),
                        HttpUtility.HtmlEncode(report.BlogPost.Title),
                        HttpUtility.HtmlEncode(report.BlogPost.Blogsite.Name),
                        HttpUtility.HtmlEncode(report.CreatedAt.ToLocalTime().ToString("dd MMM yyyy, hh:mm tt")),
                        report.Id.ToString()

                    }).ToArray()
                };

                return Json(reports);

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching report data");

                return Json(DataTables.EmptyResult);
            }
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var deleteCommand = new ReportDeleteCommand { Id = id };
                var deletedId = await _mediator.SendCommandAsync<ReportDeleteCommand, Guid>(deleteCommand);

                TempData.Put(Constants.ResponseTemkey, 
                    new ResponseModel
                    {
                        Message = $"Report Deleted Successfully",
                        Type = ResponseType.Success
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete report with Id: {id}");
                TempData.Put(Constants.ResponseTemkey, 
                    new ResponseModel
                    {
                        Message = "Failed To Delete report",
                        Type = ResponseType.Danger
                    });
            }

            return RedirectToAction("Index");
        }
    }
}
