using BlogSite.Domain.Enums;
using BlogSite.Web.Areas.Admin.Models.Category;
using BlogSite.Web.Areas.Admin.Models.Tag;
using System.ComponentModel.DataAnnotations;

namespace BlogSite.Web.Areas.Admin.Models.BlogPost
{
    public class UpdateBlogPostModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Title not should be empty."), 
            Display(Name = "Title:")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Body not should be empty."), 
            Display(Name = "Body:")]
        public string Body { get; set; } = null!;

        public string ThumbnailPath { get; set; } = null!;
        public IFormFile? Thumbnail { get; set; }


        [Required(ErrorMessage = "Please select post status.")]
        public BlogStatus PostStatus { get; set; }

        [Required(ErrorMessage = "You should select atleast one category."), 
            Display(Name = "Categories"), 
            MinLength(1, ErrorMessage = "You should select at least one category.")]
        public List<Guid>? Categories { get; set; }

        [Required(ErrorMessage = "You should select atleast one category."), 
            Display(Name = "Tags"), 
            MinLength(1, ErrorMessage = "You should select at least one tag.")]
        public List<Guid>? Tags { get; set; }
        public Guid BlogSiteId { get; set; }

        public List<CategoryViewModel> CategoriesData { get; set; } = new();
        public List<TagViewModel> TagsData { get; set; } = new();


        public async Task<string?> SaveImageFile(IWebHostEnvironment environment)
        {
            if(Thumbnail is not null)
            {
                var uploadsFolder = Path.Combine(environment.WebRootPath, "uploads/blogPost");

                if(ThumbnailPath is not null)
                {
                    var oldImageFilePath = Path.Combine(uploadsFolder, ThumbnailPath);
                    if (File.Exists(oldImageFilePath)) File.Delete(oldImageFilePath);
                }

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString() + "_" + Thumbnail.FileName;
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Thumbnail.CopyToAsync(fileStream);
                }

                return fileName;
            }
            return null;
        }
    }
}
