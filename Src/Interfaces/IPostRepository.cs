using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Src.Models;

namespace api.Src.Interfaces
{
    public interface IPostRepository
    {
        Task AddNewPostAsync(Post post);
        Task<IEnumerable<Post>> GetAllPostsAsync();
    }
}
