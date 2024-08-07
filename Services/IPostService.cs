using BlazorBlog.Models;

namespace BlazorBlog.Services
{
    public interface IPostService
    {
        Task<List<Post>> GetPostsAsync();
        Task<Post> GetPostByIdAsync(int id);
        Task AddPostAsync(Post post);
        Task UpdatePostAsync(Post post);
        Task DeletePostAsync(int id);
        Task AddCommentAsync(Comment comment);
        Task<List<Comment>> GetCommentsForPostAsync(int postId);
        Task VoteUpAsync(int postId, string userId);
        Task VoteDownAsync(int postId, string userId);
        Task VoteUpCommentAsync(int commentId, string userId);
        Task VoteDownCommentAsync(int commentId, string userId);
        Task EditCommentAsync(int commentId, string content);
        Task DeleteCommentAsync(int commentId);

    }
}
