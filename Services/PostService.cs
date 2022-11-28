using FileUpload.Entities;
using FileUpload.Interfaces;
using FileUpload.Requests;
namespace FileUpload.Services
{
    public class PostService : IPostService
    {
        private readonly SocialDbContext socialDbContext;
        private readonly IWebHostEnvironment environment;

        public PostService(SocialDbContext socialDbContext, IWebHostEnvironment environment)
        {
            this.socialDbContext = socialDbContext;
            this.environment = environment;
        }

        public async Task<string> CreatePostAsync(PostRequest postRequest)
        {
            var post = new Entities.Post
            {
                UserId = postRequest.UserId,
                Description = postRequest.Description,
                Imagepath = postRequest.ImagePath,
                Ts = DateTime.Now,
                Published = true
            };

            var postEntry = await socialDbContext.Post.AddAsync(post);

            var saveResponse = await socialDbContext.SaveChangesAsync();

            if (saveResponse < 0)
            {
                return "Issue while saving the post";
            }

            var postEntity = postEntry.Entity;
            var postModel = new Post
            {
                Id = postEntity.Id,
                Description = postEntity.Description,
                Ts = postEntity.Ts,
                Imagepath = Path.Combine(postEntity.Imagepath),
                UserId = postEntity.UserId

            };

            return "sc";

        }

        public async Task SavePostImageAsync(PostRequest postRequest)
        {
            var FileName = Path.GetFileName(postRequest.Image.FileName);
            var uniqueFileName = string.Concat(Path.GetFileNameWithoutExtension(FileName)
                                , "_"
                                , Guid.NewGuid().ToString().AsSpan(0, 4)
                                , Path.GetExtension(FileName));

              var uploads = Path.Combine(environment.WebRootPath, "users", "posts", postRequest.UserId.ToString());
            
            var filePath = Path.Combine(uploads, uniqueFileName);
            
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            await postRequest.Image.CopyToAsync(new FileStream(filePath, FileMode.Create));
            
            postRequest.ImagePath = filePath;

            return;
        }
    }
}
