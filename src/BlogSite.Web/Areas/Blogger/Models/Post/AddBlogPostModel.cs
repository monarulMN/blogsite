using BlogSite.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace BlogSite.Web.Areas.Blogger.Models.Post
{
    public class AddBlogPostModel
    {
        [Required(ErrorMessage = "Title not should be empty."), 
            Display(Name = "Title:")]
        public string Title { get; set; } = null!;


        [Required(ErrorMessage = "Thumbnail not should be empty.")]
        public IFormFile Thumbnail { get; set; } = null!;

        [Required(ErrorMessage = "Please select post status.")]
        public BlogStatus PostStatus { get; set; }

        [Required(ErrorMessage = "Body not should be empty."), 
            Display(Name = "Body:")]
        public string Body { get; set; } = null!;


        [Required(ErrorMessage = "You should select atleast one category."), 
            Display(Name = "Categories"), 
            MinLength(1, ErrorMessage = "You should select at least one category.")]
        public IList<Guid> Categories { get; set; } = null!;


        [Required(ErrorMessage = "You should select atleast one category."), 
            Display(Name = "Tags"), 
            MinLength(1, ErrorMessage = "You should select at least one tag.")]
        public IList<Guid> Tags { get; set; } = null!;



        public async Task<string?> SaveImageFile(IWebHostEnvironment environment)
        {
            if (Thumbnail is not null)
            {
                var uploadsFolder = Path.Combine(environment.WebRootPath, "uploads/blogPost");
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
