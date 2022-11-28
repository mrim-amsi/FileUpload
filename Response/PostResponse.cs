using FileUpload.Response.Models;

namespace FileUpload.Response
{
    public class PostResponse : BaseResponse
    {
        public PostModel Post { get; set; }
    }
}
