using BlogSite.Application.Contracts;
using BlogSite.Application.Features.BlogPosts.Queries;
using BlogSite.Application.Features.BlogSites.Queries;
using BlogSite.Application.Features.Categories.Queries;
using BlogSite.Application.Features.Tags.Queries;
using BlogSite.Domain.Dtos;
using BlogSite.Domain.Entities;
using BlogSite.Domain.Enums;
using BlogSite.Web.Areas.Reader.Models;
using BlogSite.Web.Areas.Reader.Models.Blog;
using BlogSite.Web.Areas.Reader.Models.Comment;
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

            model.Comments = blogPost.Comments
                        .Select(x => new CommentViewModel
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Email = x.Email,
                            Text = x.Text,
                            CreateAt = x.CreateAt
                        })
                        .ToList();

            return View(model);
        }

        [Route("blog/{slug}/about"), HttpGet]
        public async Task<IActionResult> BlogSiteAbout(string slug)
        {
            var query = new GetBlogSiteBySlugQuery { Slug = slug };
            var blogSite = await _mediator.SendQueryAsync<GetBlogSiteBySlugQuery, Blogsite>(query);

            var model = _mapper.Map<BlogSiteViewModel>(blogSite);

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> LoadMore(int page)
        {
            try
            {
                var query = new GetPublicBlogPostsQuery
                {
                    PageIndex = page,
                    PageSize = 5
                };

                var (result, hasMore) = await _mediator
                    .SendQueryAsync<GetPublicBlogPostsQuery,
                    (IReadOnlyList<PublicBlogPostDto> Posts, bool HasMore)>(query);

                return Json(new { data = result, hasMore });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error fetching blog post data.");
                return Json(new { data = new List<object>(), hasMore = false });
            }
        }


        [HttpPost]
        public async Task<IActionResult> BlogLoadMore(int page, Guid Id)
        {
            try
            {
                var query = new GetPublicBlogPostsByBlogSiteQuery
                {
                    BlogSiteId = Id,
                    PageIndex = page,
                    PageSize = 5
                };

                var (result, hasMore) = await _mediator
                    .SendQueryAsync<GetPublicBlogPostsByBlogSiteQuery,
                    (IReadOnlyList<PublicBlogPostDto> Posts, bool HasMore)>(query);

                return Json(new { data = result, hasMore });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error fetching blog post data.");
                return Json(new { data = new List<object>(), hasMore = false });
            }
        }

        [HttpPost]
        public async Task<IActionResult> BlogSitePostByTag(int page, Guid blogSiteId, Guid tagId)
        {
            try
            {
                var query = new GetPublicBlogPostsByBlogSiteAndTagQuery
                {
                    BlogSiteId = blogSiteId,
                    TagId = tagId,
                    PageIndex = page,
                    PageSize = 5
                };

                var (result, hasMore) = await _mediator
                    .SendQueryAsync<GetPublicBlogPostsByBlogSiteAndTagQuery,
                    (IReadOnlyList<PublicBlogPostDto> Posts, bool HasMore)>(query);

                return Json(new { data = result, hasMore });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error fetching blog post data.");
                return Json(new { data = new List<object>(), hasMore = false });
            }
        }


        [HttpPost]
        public async Task<IActionResult> BlogSitePostByCategory(int page, Guid blogSiteId, Guid categoryId)
        {
            try
            {
                var query = new GetPublicBlogPostsByBlogSiteAndCategoryQuery
                {
                    BlogSiteId = blogSiteId,
                    CategoryId = categoryId,
                    PageIndex = page,
                    PageSize = 5
                };

                var (result, hasMore) = await _mediator
                    .SendQueryAsync<GetPublicBlogPostsByBlogSiteAndCategoryQuery,
                    (IReadOnlyList<PublicBlogPostDto> Posts, bool HasMore)>(query);

                return Json(new { data = result, hasMore });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error fetching blog post data.");
                return Json(new { data = new List<object>(), hasMore = false });
            }
        }


        [Route("blog/post/{postId}"), HttpGet]
        public async Task<IActionResult> PostView(Guid postId)
        {
            var blogPostQuery = new GetPublicBlogPostGetWithTagQuery { BlogPostId = postId };
            var blogPost = await _mediator.SendQueryAsync<GetPublicBlogPostGetWithTagQuery, 
                PublicBlogPostDto?>(blogPostQuery);

            var model = _mapper.Map<BlogSitePostViewModel>(blogPost);
            model.Slug = blogPost.AuthorSlag;
            model.Name = blogPost.Author;



            var blogSiteQuery = new GetBlogSiteBySlugQuery { Slug = blogPost.AuthorSlag };
            var blogSite = await _mediator.SendQueryAsync<GetBlogSiteBySlugQuery, 
                Blogsite>(blogSiteQuery);

            model.AuthorId = blogSite.AuthorId;
            model.Id = blogSite.Id;

            model.Post = _mapper.Map<PostViewModel>(blogPost);
            model.Post.Tags = blogPost.Tags
                        .Select(x => new TagViewModel
                        {
                            Id = x.Id,
                            Name = x.Name,
                            PostCount = x.BlogPostTags.Count
                        })
                        .ToList();

            model.Comments = blogPost.Comments
                        .Select(x => new CommentViewModel
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Email = x.Email,
                            Text = x.Text,
                            CreateAt = x.CreateAt
                        })
                        .ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GetPostByTag(int page, Guid tagId)
        {
            try
            {
                var query = new GetPublicBlogPostsByTagQuery
                {
                    TagId = tagId,
                    PageIndex = page,
                    PageSize = 5
                };

                var (result, hasMore) = await _mediator
                    .SendQueryAsync<GetPublicBlogPostsByTagQuery,
                    (IReadOnlyList<PublicBlogPostDto> Posts, bool HasMore)>(query);

                return Json(new { data = result, hasMore });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error fetching blog post data.");
                return Json(new { data = new List<object>(), hasMore = false });
            }
        }
    }
}
