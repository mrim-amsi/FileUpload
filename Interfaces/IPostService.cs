using FileUpload.Requests;

namespace FileUpload.Interfaces
{
    public interface IPostService
    {
        Task SavePostImageAsync(PostRequest postRequest);
        Task<string> CreatePostAsync(PostRequest postRequest);
    }
}