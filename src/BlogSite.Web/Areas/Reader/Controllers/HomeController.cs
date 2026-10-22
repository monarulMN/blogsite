using BlogSite.Application.Contracts;
using BlogSite.Application.Features.BlogSites.Queries;
using BlogSite.Application.Features.Tags.Queries;
using BlogSite.Domain.Dtos;
using BlogSite.Domain.Entities;
using BlogSite.Domain.Enums;
using BlogSite.Web.Areas.Reader.Models;
using Cortex.Mediator;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Web.Areas.Reader.Controllers
{
    [Area("Reader")]
    public class HomeController(IMediator mediator, IMapper mapper, 
        ILogger<HomeController> logger, IServerTime serverTime) : Controller
    {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<HomeController> _logger = logger;
        private readonly IServerTime _serverTime = serverTime;

        [Route("blog-list")]
        public async Task<IActionResult> Index()
        {
            var blogSiteQuery = new GetBlogSiteByStatusQuery { Status = Status.Active };
            var blogSites = await _mediator.SendQueryAsync<GetBlogSiteByStatusQuery, IList<Blogsite>>(blogSiteQuery);
            var tagsQuery = new GetTagsByStatusQuery { Status = Status.Active };
            var tags = await _mediator.SendQueryAsync<GetTagsByStatusQuery, IList<TagDto>>(tagsQuery);
            var model = new PublicPagesBuilderModel
            {
                BlogSites = _mapper.Map<IList<PublicPagesBlogSiteModel>>(blogSites),
                Tags = _mapper.Map<IList<PublicPagesTagModel>>(tags)
            };

            return View(model);
        }
    }
}
