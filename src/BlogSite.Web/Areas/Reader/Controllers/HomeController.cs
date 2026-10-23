using BlogSite.Application.Contracts;
using BlogSite.Application.Features.BlogPosts.Queries;
using BlogSite.Application.Features.BlogSites.Queries;
using BlogSite.Application.Features.Categories.Queries;
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

            var blogSites = await _mediator.SendQueryAsync<GetBlogSiteByStatusQuery, 
                IList<Blogsite>>(blogSiteQuery);

            var tagsQuery = new GetTagsByStatusQuery { Status = Status.Active };

            var tags = await _mediator.SendQueryAsync<GetTagsByStatusQuery, IList<TagDto>>(tagsQuery);

            var model = new PublicPagesBuilderModel
            {
                BlogSites = _mapper.Map<IList<PublicPagesBlogSiteModel>>(blogSites),
                Tags = _mapper.Map<IList<PublicPagesTagModel>>(tags)
            };

            return View(model);
        }

        [Route("blog/{slug}"), HttpGet]
        public async Task<IActionResult> BlogSite(string slug)
        {
            var query = new GetBlogSiteBySlugQuery { Slug = slug };

            var blogSite = await _mediator.SendQueryAsync<GetBlogSiteBySlugQuery, Blogsite>(query);

            var categories = await _mediator.SendQueryAsync<GetCategoriesByBlogSiteIdQuery, 
                IReadOnlyList<CategoryDto>>(new GetCategoriesByBlogSiteIdQuery { BlogSiteId = blogSite.Id });

            var tags = await _mediator.SendQueryAsync<GetTagsUsedInBlogPostUnderBlogSiteQuery, 
                IReadOnlyList<TagDto>>(new GetTagsUsedInBlogPostUnderBlogSiteQuery { BlogSiteId = blogSite.Id });

            var model = _mapper.Map<BlogSiteViewModel>(blogSite);

            model.Tags = _mapper.Map<IReadOnlyList<TagViewModel>>(tags);
            model.Categories = _mapper.Map<IReadOnlyList<CategoryViewModel>>(categories);

            return View(model);
        }

        [Route("blog/{slug}/post/{postId}"), HttpGet]
        public async Task<IActionResult> BlogSitePostView(string slug, Guid postId)
        {
            var blogSiteQuery = new GetBlogSiteBySlugQuery { Slug = slug };
            var blogSite = await _mediator.SendQueryAsync<GetBlogSiteBySlugQuery, Blogsite>(blogSiteQuery);
            var blogPostQuery = new GetPublicBlogPostGetWithTagQuery { BlogPostId = postId };
            var blogPost = await _mediator.SendQueryAsync<GetPublicBlogPostGetWithTagQuery, PublicBlogPostDto>(blogPostQuery);
            var model = _mapper.Map<BlogSitePostViewModel>(blogSite);
            model.Post = _mapper.Map<PostViewModel>(blogPost);
            model.Post.Tags = blogPost.Tags
                        .Select(x => new TagViewModel
                        {
                            Id = x.Id,
                            Name = x.Name,
                            PostCount = x.BlogPostTags.Count
                        })
                        .ToList();

            //model.Comments = blogPost.Comments
            //            .Select(x => new CommentViewModel
            //            {
            //                Id = x.Id,
            //                Name = x.Name,
            //                Email = x.Email,
            //                Text = x.Text,
            //                CreateAt = x.CreateAt
            //            })
            //            .ToList();

            return View(model);
        }
    }
}
