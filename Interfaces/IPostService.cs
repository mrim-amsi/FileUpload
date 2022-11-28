using FileUpload.Requests;
using FileUpload.Response;

namespace FileUpload.Interfaces
{
    public interface IPostService
    {
        Task SavePostImageAsync(PostRequest postRequest);
        Task<PostResponse> CreatePostAsync(PostRequest postRequest);
    }
}