using BlazorBlog.Data;
using BlazorBlog.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorBlog.Services
{
    public interface ICommentService
    {
        Task<List<Comment>> GetCommentsByPostIdAsync(int postId);
        Task AddCommentAsync(Comment comment);
    }

    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext _context;

        public CommentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Comment>> GetCommentsByPostIdAsync(int postId)
        {
            return await _context.Comments
                .Where(c => c.PostId == postId)
                .Include(c => c.Author)
                .ToListAsync();
        }

        public async Task AddCommentAsync(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
        }
    }

}
