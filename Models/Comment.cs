using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorBlog.Models
{
    public class Comment
    {
        /*  public int Id { get; set; }
          public string Author { get; set; }
          public string Text { get; set; }
          public int PostId { get; set; }
          public long Timestamp { get; set; }
          public int VoteUp { get; set; }
          public int Votedown { get; set; } */

        public int Id { get; set; }
        public string Content { get; set; }
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
        public long Timestamp { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public int VoteUp { get; set; }
        public int Votedown { get; set; }

        [NotMapped]
        public List<string> Votelistup { get; set; } = new List<string>();

        public void AddUserIdToVotelistup(string userId)
        {
            if (!Votelistup.Contains(userId))
            {
                Votelistup.Add(userId);
            }
        }

        public string? VotelistupJson
        {
            get => JsonConvert.SerializeObject(Votelistup);
            set => Votelistup = string.IsNullOrEmpty(value) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(value);
        }

        [NotMapped]
        public List<string> Votelistdwn { get; set; } = new List<string>();

        public void AddUserIdToVotelistdwn(string userId)
        {
            if (!Votelistdwn.Contains(userId))
            {
                Votelistdwn.Add(userId);
            }
        }

        public string? VotelistdwnJson
        {
            get => JsonConvert.SerializeObject(Votelistdwn);
            set => Votelistdwn = string.IsNullOrEmpty(value) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(value);
        }

    }
}
