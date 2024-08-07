using BlazorBlog.Data;
using BlazorBlog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
namespace BlazorBlog.Services
{
   
    public class PostService : IPostService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PostService> _logger;

        public PostService(ApplicationDbContext context, ILogger<PostService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Post>> GetPostsAsync()
        {
            _logger.LogInformation("Fetching posts from the database.");
            var posts = await _context.Posts.Include(p => p.Author).Include(p => p.Comments).ThenInclude(c => c.Author).ToListAsync();
            if (posts == null || posts.Count == 0)
            {
                _logger.LogWarning("No posts found in the database.");
            }
            else
            {
                _logger.LogInformation($"Retrieved {posts.Count} posts from the database.");
            }
            return posts;
        }

        public async Task<Post> GetPostByIdAsync(int id)
        {
            var post = await _context.Posts.Include(p => p.Comments).ThenInclude(c => c.Author).FirstOrDefaultAsync(p => p.Id == id);
            if (post == null)
            {
                _logger.LogWarning($"Post with ID {id} not found.");
            }
            return post;
        }

        public async Task AddPostAsync(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Post with ID {post.Id} added to the database.");
        }

        /* public async Task UpdatePostAsync(Post post)
         {
             _context.Posts.Update(post);
             await _context.SaveChangesAsync();
             _logger.LogInformation($"Post with ID {post.Id} updated in the database.");
         } */
        public async Task UpdatePostAsync(Post post)
        {
            var existingPost = await _context.Posts.FindAsync(post.Id);
            if (existingPost != null)
            {
                existingPost.Title = post.Title;
                existingPost.Content = post.Content;
                existingPost.ContentType = post.ContentType;
                existingPost.Data = post.Data;

                _context.Posts.Update(existingPost);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeletePostAsync(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Post with ID {id} deleted from the database.");
            }
            else
            {
                _logger.LogWarning($"Post with ID {id} not found and cannot be deleted.");
            }
        }
        public async Task VoteUpAsync(int postId, string userId)
        {
            var post = await _context.Posts.FindAsync(postId);
            if (post != null)
            {
               
                if (post.Votelistup.Contains(userId))
                {
                    post.Votelistup.Remove(userId);
                    post.VoteUp--;
                }
                else
                {
                    post.Votelistup.Add(userId);
                    post.VoteUp++;
                    post.Votelistdwn.Remove(userId);  // Remove from downvotes if present
                    post.Votedown = Math.Max(0, post.Votedown - 1);  // Decrease downvote count if applicable
                }
                post.VotelistupJson = JsonConvert.SerializeObject(post.Votelistup);
                post.VotelistdwnJson = JsonConvert.SerializeObject(post.Votelistdwn);
                _context.Posts.Update(post);
                await _context.SaveChangesAsync();
            }
        }
        public async Task VoteDownAsync(int postId, string userId)
        {
            var post = await _context.Posts.FindAsync(postId);
            if (post != null)
            {
                if (post.Votelistdwn.Contains(userId))
                {
                    post.Votelistdwn.Remove(userId);
                    post.Votedown--;
                }
                else
                {
                    post.Votelistdwn.Add(userId);
                    post.Votedown++;
                    post.Votelistup.Remove(userId);  // Remove from upvotes if present
                    post.VoteUp = Math.Max(0, post.VoteUp - 1);  // Decrease upvote count if applicable
                }
                post.VotelistupJson = JsonConvert.SerializeObject(post.Votelistup);
                post.VotelistdwnJson = JsonConvert.SerializeObject(post.Votelistdwn);
                _context.Posts.Update(post);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddCommentAsync(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Comment with ID {comment.Id} added to the database.");
        }
        public async Task<List<Comment>> GetCommentsForPostAsync(int postId)
        {
            return await _context.Comments
                .Where(c => c.PostId == postId)
                .Include(c => c.Author)
                .ToListAsync();
        }
        public async Task VoteUpCommentAsync(int commentId, string userId)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null) return;

            if (comment.Votelistup.Contains(userId))
            {
                comment.Votelistup.Remove(userId);
                comment.VoteUp--;
                
            }
            else
            {
                comment.Votelistup.Add(userId);
                comment.VoteUp++;
                comment.Votelistdwn.Remove(userId);  // Remove from downvotes if present
                comment.Votedown = Math.Max(0, comment.Votedown - 1);
            }
            comment.VotelistupJson = JsonConvert.SerializeObject(comment.Votelistup);
            comment.VotelistdwnJson = JsonConvert.SerializeObject(comment.Votelistdwn);
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();

        }
        public async Task VoteDownCommentAsync(int commentId, string userId)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null) return;

            if (comment.Votelistdwn.Contains(userId))
            {
                comment.Votelistdwn.Remove(userId);
                comment.Votedown--;

               
            }
            else
            {
                comment.Votelistdwn.Add(userId);
                comment.Votedown++;
                comment.Votelistup.Remove(userId);  // Remove from upvotes if present
                comment.VoteUp = Math.Max(0, comment.VoteUp - 1);
            }
            comment.VotelistupJson = JsonConvert.SerializeObject(comment.Votelistup);
            comment.VotelistdwnJson = JsonConvert.SerializeObject(comment.Votelistdwn);
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();

        }
        public async Task EditCommentAsync(int commentId, string content)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment != null)
            {
                comment.Content = content;
                _context.Comments.Update(comment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteCommentAsync(int commentId)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }

    }



}
