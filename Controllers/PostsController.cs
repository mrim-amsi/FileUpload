using FileUpload.Entities;
using FileUpload.Interfaces;
using FileUpload.Requests;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace FileUpload.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly ILogger<PostsController> logger;
        private readonly IPostService postService;

        public PostsController(ILogger<PostsController> logger, IPostService postService)
        {
            this.logger = logger;
            this.postService = postService;
        }

        [HttpPost]
        [Route("")]
        [RequestSizeLimit(5 * 1024 * 1024)]
        public async Task<IActionResult> SubmitPost([FromForm] PostRequest postRequest)
        {
            if (postRequest == null)
            {
                return BadRequest("Invalid post request");
            }

            if (string.IsNullOrEmpty(Request.GetMultipartBoundary()))
            {
                return BadRequest("Invalid post header");
            }

            if (postRequest.Image != null)
            {
                await postService.SavePostImageAsync(postRequest);
            }
            var post = new Entities.Post
            {
                UserId = postRequest.UserId,
                Description = postRequest.Description,
                Imagepath = postRequest.ImagePath,
                Ts = DateTime.Now,
                Published = true
            };



            var postResponse = await postService.CreatePostAsync(postRequest);

            return Ok();

        }
    }
}
